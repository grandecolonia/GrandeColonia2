
using UnityEngine;

public class CaixaCorreio : MonoBehaviour
{
    [Header("Configuração da Caixa")]

    // Define qual cor de carta deve ser entregue nesta caixa.
    public string corDaCaixa;

    void Start()
    {
        // Se essa caixa já recebeu a carta certa, deixa ela desativada.
        if (ControleEntrega.instancia != null && ControleEntrega.instancia.CaixaJaConcluida(corDaCaixa))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se não foi o jogador que entrou, não tem nada para fazer aqui.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Sem o sistema de entregas funcionando, não dá para abrir o menu.
        if (ControleEntrega.instancia == null)
        {
            return;
        }

        // Se a carta dessa caixa já foi entregue, impede outra interação.
        if (ControleEntrega.instancia.CaixaJaConcluida(corDaCaixa))
        {
            return;
        }

        // Abre o menu de cartas e informa a cor e qual caixa está sendo usada.
        ControleEntrega.instancia.AbrirMenuCartas(corDaCaixa, this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Quando o jogador sai de perto, fecha o menu de cartas.
        if (other.CompareTag("Player") && ControleEntrega.instancia != null)
        {
            ControleEntrega.instancia.FecharMenuCartas();
        }
    }

    public void DesativarCaixa()
    {
        // Desliga a caixa inteira para impedir novas entregas nela.
        gameObject.SetActive(false);
    }
}
