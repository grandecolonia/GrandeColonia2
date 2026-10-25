
using UnityEngine;

public class InteracaoEscada : MonoBehaviour
{
    [Header("Escada")]

    // Objeto que aparece quando o jogador chega perto da escada.
    public GameObject escada;

    // Guarda se o jogador está dentro da área de interação.
    public bool jogadorPerto = false;

    void Start()
    {
        // Começa com o objeto escondido até o jogador se aproximar.
        escada.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se foi o jogador que entrou na área, libera a interação.
        if (other.CompareTag("Player"))
        {
            // Marca que o jogador está perto e mostra o objeto da escada.
            jogadorPerto = true;
            escada.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se o jogador saiu da área, cancela a interação.
        if (other.CompareTag("Player"))
        {
            // Marca que o jogador se afastou e esconde o objeto.
            jogadorPerto = false;
            escada.SetActive(false);
        }
    }

    public void EsconderEscada()
    {
        // Esconde o objeto da escada quando outro script mandar.
        escada.SetActive(false);
    }
}
