using UnityEngine;

public class TriggerDialogo : MonoBehaviour
{
    // Sistema responsável por exibir o diálogo na tela.
    public SistemaDialogo sistemaDialogo;

    [Header("Diálogo")]

    // Diálogo que será iniciado quando o jogador entrar no gatilho.
    public DialogoData dialogo;

    // Impede que o mesmo diálogo seja ativado mais de uma vez.
    private bool ativado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Inicia o diálogo apenas quando o jogador entra pela primeira vez.
        if (other.CompareTag("Player") && !ativado)
        {
            ativado = true;
            sistemaDialogo.IniciarDialogo(dialogo);
        }
    }
}