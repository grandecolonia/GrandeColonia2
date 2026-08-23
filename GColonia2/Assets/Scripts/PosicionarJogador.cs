using UnityEngine;

public class PosicionarJogador : MonoBehaviour
{
    // Pontos em que o jogador pode aparecer ao entrar na cena.
    public Transform pontoElevador;
    public Transform pontoEsquerda;
    public Transform pontoDireita;

    void Start()
    {
        // Procura o personagem presente na cena.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        // Interrompe o posicionamento caso nenhum jogador seja encontrado.
        if (jogador == null)
        {
            return;
        }

        // Usa o elevador como entrada padrão.
        string entrada = "Elevador";

        // Recupera o local pelo qual o jogador entrou na cena.
        if (ControleEntrega.instancia != null)
        {
            entrada = ControleEntrega.instancia.pontoEntrada;
        }

        // Posiciona o jogador no ponto correspondente à entrada utilizada.
        if (entrada == "Esquerda")
        {
            jogador.transform.position = pontoEsquerda.position;
        }
        else if (entrada == "Direita")
        {
            jogador.transform.position = pontoDireita.position;
        }
        else
        {
            jogador.transform.position = pontoElevador.position;
        }

        // Remove qualquer velocidade que o jogador tinha antes da troca de cena.
        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}