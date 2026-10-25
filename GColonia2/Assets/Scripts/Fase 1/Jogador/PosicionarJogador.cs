
using UnityEngine;

public class PosicionarJogador : MonoBehaviour
{
    [Header("Controle de Entrada")]

    // Guarda de onde Maxwell veio, mesmo depois de trocar de cena.
    public static string entradaAtual = "Elevador";

    [Header("Pontos de Entrada")]

    // Posição onde Maxwell aparece.
    public Transform pontoElevador, pontoEsquerda, pontoDireita;

    void Start()
    {
        // Procura o Maxwell que está presente na cena atual.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        // Se não encontrou o jogador, não tem ninguém para posicionar.
        if (jogador == null)
        {
            return;
        }

        // Se Maxwell veio da esquerda, coloca ele no ponto correspondente.
        if (entradaAtual == "Esquerda")
        {
            jogador.transform.position = pontoEsquerda.position;
        }
        // Se veio da direita, posiciona no ponto do lado direito.
        else if (entradaAtual == "Direita")
        {
            jogador.transform.position = pontoDireita.position;
        }
        else
        {
            // Se não veio de nenhum dos lados, coloca Maxwell perto do elevador.
            jogador.transform.position = pontoElevador.position;
        }

        // Pega o Rigidbody2D para limpar qualquer movimento que tenha sobrado.
        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        // Se encontrou o Rigidbody, zera a velocidade para Maxwell começar parado.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
