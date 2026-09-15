using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardaFase2 : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade que o guarda anda da direita para a esquerda.
    public float velocidade = 5f;

    [Header("Limite")]

    // Posição em que o guarda é apagado depois de sair da tela.
    public float limiteEsquerdo = -12f;

    void Update()
    {
        // Faz o guarda correr para a esquerda.
        transform.position += Vector3.left * velocidade * Time.deltaTime;

        // Apaga o guarda quando ele passa do limite da tela.
        if (transform.position.x < limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Reinicia a fase se o guarda encostar no jogador.
        if (collision.CompareTag("Player"))
        {
            ReiniciarFase();
        }
    }

    void ReiniciarFase()
    {
        // Recarrega a cena atual.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}