
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardaFase2 : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;

    [Header("Limite")]
    public float limiteEsquerdo = -12f;

    void Update()
    {
        // Guarda corre na direção de Maxwell.
        transform.position += Vector3.left * velocidade * Time.deltaTime;

        if (transform.position.x < limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ControleFaseCorrida controle = FindFirstObjectByType<ControleFaseCorrida>();

            if (controle != null)
            {
                controle.Derrota();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
