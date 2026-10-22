using UnityEngine;

public class PosicionarJogador : MonoBehaviour
{
    // Guarda a entrada mesmo quando a cena muda.
    public static string entradaAtual = "Elevador";

    public Transform pontoElevador;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    void Start()
    {
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador == null)
        {
            return;
        }

        // Posiciona Maxwell conforme o lado de retorno.
        if (entradaAtual == "Esquerda")
        {
            jogador.transform.position = pontoEsquerda.position;
        }
        else if (entradaAtual == "Direita")
        {
            jogador.transform.position = pontoDireita.position;
        }
        else
        {
            jogador.transform.position = pontoElevador.position;
        }

        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
