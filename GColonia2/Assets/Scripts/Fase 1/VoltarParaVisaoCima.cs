
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarParaVisaoCima : MonoBehaviour
{
    public string ladoDeRetorno;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Guarda de qual lado Maxwell está voltando.
        PosicionarJogador.entradaAtual = ladoDeRetorno;

        // Mantém o sistema de entregas sincronizado.
        if (ControleEntrega.instancia != null)
        {
            ControleEntrega.instancia.pontoEntrada = ladoDeRetorno;
        }

        SceneManager.LoadScene("09_VisaodeCima");
    }
}
