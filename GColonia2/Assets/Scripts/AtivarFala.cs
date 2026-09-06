using UnityEngine;

public class AtivarFala : MonoBehaviour
{
    [Header("Falas")]

    // Balões que aparecem quando o jogador entra nesta área.
    public CanvasGroup[] falas;

    void Start()
    {
        // Começa com todos os balões escondidos.
        AlterarVisibilidade(0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("ENTROU NO GATILHO: " + collision.gameObject.name);

    if (collision.CompareTag("Player"))
    {
        Debug.Log("É O PLAYER - MOSTRANDO FALAS");
        AlterarVisibilidade(1f);
    }
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AlterarVisibilidade(0f);
        }
    }

    void AlterarVisibilidade(float alpha)
    {
        foreach (CanvasGroup fala in falas)
        {
            fala.alpha = alpha;
        }
    }
}