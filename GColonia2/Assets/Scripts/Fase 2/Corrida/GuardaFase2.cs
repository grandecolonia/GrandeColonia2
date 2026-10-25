
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardaFase2 : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade com que o guarda corre pelo cenário.
    public float velocidade = 5f;

    [Header("Limite")]

    // Posição em que o guarda será destruído depois de sair pela esquerda.
    public float limiteEsquerdo = -12f;

    void Update()
    {
        // Faz o guarda correr para a esquerda durante a fase.
        transform.position += Vector3.left * velocidade * Time.deltaTime;

        // Se o guarda passou do limite do cenário, não precisa continuar existindo.
        if (transform.position.x < limiteEsquerdo)
        {
            // Remove o guarda para não acumular inimigos fora da tela.
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o guarda encostou em Maxwell, começa a derrota.
        if (collision.CompareTag("Player"))
        {
            // Procura o sistema responsável por controlar a fase de corrida.
            ControleFaseCorrida controle = FindFirstObjectByType<ControleFaseCorrida>();

            // Se encontrou o controlador, deixa ele cuidar da derrota.
            if (controle != null)
            {
                controle.Derrota();
            }
            else
            {
                // Se não encontrou o controlador, reinicia a cena diretamente.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
