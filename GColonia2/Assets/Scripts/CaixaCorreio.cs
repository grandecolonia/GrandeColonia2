using UnityEngine;

public class CaixaCorreio : MonoBehaviour
{
    // Define qual cor de carta pertence a esta caixa de correio.
    public string corDaCaixa;

    void Start()
    {
        // Mantém a caixa desativada caso ela já tenha recebido a carta correta.
        if (ControleEntrega.instancia != null && ControleEntrega.instancia.CaixaJaConcluida(corDaCaixa))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora qualquer objeto que não seja o jogador.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Evita continuar caso o sistema de entregas não esteja disponível.
        if (ControleEntrega.instancia == null)
        {
            return;
        }

        // Impede que uma caixa já concluída abra o menu novamente.
        if (ControleEntrega.instancia.CaixaJaConcluida(corDaCaixa))
        {
            return;
        }

        // Abre o menu de cartas e informa qual caixa está sendo utilizada.
        ControleEntrega.instancia.AbrirMenuCartas(corDaCaixa, this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Fecha o menu quando o jogador se afasta da caixa.
        if (other.CompareTag("Player") && ControleEntrega.instancia != null)
        {
            ControleEntrega.instancia.FecharMenuCartas();
        }
    }

    public void DesativarCaixa()
    {
        // Desativa a área de interação e o ícone flutuante desta caixa.
        gameObject.SetActive(false);
    }
}