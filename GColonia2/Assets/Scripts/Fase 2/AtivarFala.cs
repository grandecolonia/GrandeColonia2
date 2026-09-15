using UnityEngine;

public class AtivarFala : MonoBehaviour
{
    [Header("Falas")]

    // Balões que aparecem quando o jogador entra nessa área.
    public CanvasGroup[] falas;

    void Start()
    {
        // Começa com todas as falas escondidas.
        AlterarVisibilidade(0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTROU NO GATILHO: " + collision.gameObject.name);

        // Se quem entrou for o jogador, mostra as falas.
        if (collision.CompareTag("Player"))
        {
            Debug.Log("É O PLAYER - MOSTRANDO FALAS");
            AlterarVisibilidade(1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Esconde as falas quando o jogador sai da área.
        if (collision.CompareTag("Player"))
        {
            AlterarVisibilidade(0f);
        }
    }

    void AlterarVisibilidade(float alpha)
    {
        // Altera a transparência de todas as falas de uma vez.
        foreach (CanvasGroup fala in falas)
        {
            fala.alpha = alpha;
        }
    }
}