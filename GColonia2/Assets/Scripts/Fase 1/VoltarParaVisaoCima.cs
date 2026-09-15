using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarParaVisaoCima : MonoBehaviour
{
    // Define de qual lado o jogador retornará para a visão de cima.
    public string ladoDeRetorno;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora qualquer objeto que não seja o jogador.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Guarda o lado de retorno antes de trocar de cena.
        if (ControleEntrega.instancia != null)
        {
            ControleEntrega.instancia.pontoEntrada = ladoDeRetorno;
        }

        // Retorna para a cena com a visão de cima.
        SceneManager.LoadScene("09_VisaodeCima");
    }
}