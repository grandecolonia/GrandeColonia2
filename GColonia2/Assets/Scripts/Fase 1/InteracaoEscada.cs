using UnityEngine;

public class InteracaoEscada : MonoBehaviour
{
    // Objeto visual que indica que o jogador pode usar a escada.
    public GameObject escada;

    void Start()
    {
        // O ícone da escada começa escondido.
        escada.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mostra o ícone quando o jogador entra na área de interação.
        if (other.CompareTag("Player"))
        {
            escada.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Esconde o ícone quando o jogador sai da área.
        if (other.CompareTag("Player"))
        {
            escada.SetActive(false);
        }
    }

    public void EsconderEscada()
    {
        // Permite que outro script esconda o ícone da escada.
        escada.SetActive(false);
    }
}