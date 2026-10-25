
using UnityEngine;

public class AtivarFala : MonoBehaviour
{
    [Header("Falas")]

    // Guarda todos os balões de fala que vão aparecer quando Maxwell chegar perto.
    public CanvasGroup[] falas;

    void Start()
    {
        // Começa com todas as falas escondidas para não aparecerem antes da hora.
        AlterarVisibilidade(0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se foi Maxwell que entrou na área, mostra todos os balões de fala.
        if (collision.CompareTag("Player"))
        {
            AlterarVisibilidade(1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Quando Maxwell sair da área, esconde os balões novamente.
        if (collision.CompareTag("Player"))
        {
            AlterarVisibilidade(0f);
        }
    }

    void AlterarVisibilidade(float alpha)
    {
        // Passa por todos os balões para mudar a transparência de uma vez.
        foreach (CanvasGroup fala in falas)
        {
            // Aplica a transparência recebida, deixando a fala visível ou escondida.
            fala.alpha = alpha;
        }
    }
}
