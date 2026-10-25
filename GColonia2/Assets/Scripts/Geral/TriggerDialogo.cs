
using UnityEngine;

public class TriggerDialogo : MonoBehaviour
{
    [Header("Diálogo")]

    // Sistema que vai mostrar as falas quando Maxwell entrar na área.
    public SistemaDialogo sistemaDialogo;

    // Guarda o diálogo que será iniciado ao encontrar o jogador.
    public DialogoData dialogo;

    // Marca se o diálogo já foi ativado para não ficar repetindo a conversa.
    private bool ativado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se foi Maxwell que entrou na área e o diálogo ainda não aconteceu, inicia a conversa.
        if (other.CompareTag("Player") && !ativado)
        {
            // Marca que a conversa já foi ativada para impedir novas chamadas.
            ativado = true;

            // Manda o sistema de diálogo mostrar as falas configuradas.
            sistemaDialogo.IniciarDialogo(dialogo);
        }
    }
}
