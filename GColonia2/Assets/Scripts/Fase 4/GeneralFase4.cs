
using UnityEngine;

public class GeneralFase4 : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 140;
    public int limitePrimeiraPausa = 90;
    public int limiteSegundaPausa = 40;
    public int pontosAoDerrotar = 500;

    private int vidaAtual;
    private bool primeiraPausa;
    private bool segundaPausa;
    private bool escondido;
    private bool derrotado;

    private SpriteRenderer imagem;
    private Collider2D colisor;

    void Start()
    {
        vidaAtual = vidaMaxima;
        imagem = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
    }

    public void ReceberDano(int dano)
    {
        if (escondido || derrotado) return;

        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        if (!primeiraPausa && vidaAtual <= limitePrimeiraPausa)
        {
            vidaAtual = limitePrimeiraPausa;
            primeiraPausa = true;
            Esconder();
            return;
        }

        if (!segundaPausa && vidaAtual <= limiteSegundaPausa)
        {
            vidaAtual = limiteSegundaPausa;
            segundaPausa = true;
            Esconder();
            return;
        }

        Debug.Log("Vida do General: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            derrotado = true;

            if (PontuacaoFase4.instancia != null)
                PontuacaoFase4.instancia.AdicionarPontos(pontosAoDerrotar);

            Debug.Log("General derrotado!");
            Destroy(gameObject);
        }
    }

    void Esconder()
    {
        escondido = true;
        imagem.enabled = false;
        colisor.enabled = false;

        Debug.Log("General recuou! Onda acelerada!");
    }

    public void Voltar()
    {
        escondido = false;
        imagem.enabled = true;
        colisor.enabled = true;

        Debug.Log("General voltou com " + vidaAtual + " de vida!");
    }

    public bool EstaEscondido()
    {
        return escondido;
    }

    public int ConsultarVida()
{
    return vidaAtual;
}
}
