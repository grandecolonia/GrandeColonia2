
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarParaVisaoCima : MonoBehaviour
{
    [Header("Retorno")]

    // Define de qual lado Maxwell está voltando para a visão de cima.
    public string ladoDeRetorno;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se não foi Maxwell que entrou no limite, não faz nada.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Guarda o lado de onde Maxwell veio para posicioná-lo corretamente na próxima cena.
        PosicionarJogador.entradaAtual = ladoDeRetorno;

        // Se o sistema de entregas existe, atualiza o lado de retorno nele também.
        if (ControleEntrega.instancia != null)
        {
            ControleEntrega.instancia.pontoEntrada = ladoDeRetorno;
        }

        // Volta para a visão de cima, onde Maxwell será colocado na entrada correspondente.
        SceneManager.LoadScene("09_VisaodeCima");
    }
}
