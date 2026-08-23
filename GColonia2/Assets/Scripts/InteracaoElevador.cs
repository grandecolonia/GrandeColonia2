using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracaoElevador : MonoBehaviour
{
    // Elementos configurados pelo Inspector.
    public GameObject iconeE;
    public string cenaDestino = "09_VisaodeCima";

    // Guarda se o jogador está dentro da área de interação.
    private bool jogadorPerto = false;

    void Start()
    {
        // O ícone começa escondido.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Só permite usar o elevador quando o jogador está perto e aperta E.
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            iconeE.SetActive(false);

            // Informa que o jogador entrou na próxima cena pelo elevador.
            if (ControleEntrega.instancia != null)
            {
                ControleEntrega.instancia.pontoEntrada = "Elevador";
            }

            SceneManager.LoadScene(cenaDestino);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mostra o ícone quando o jogador entra na área do elevador.
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Esconde o ícone quando o jogador sai da área.
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            iconeE.SetActive(false);
        }
    }
}