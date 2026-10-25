
using UnityEngine;

public class GeneralFase4 : MonoBehaviour
{
    [Header("Vida")]

    // Quantidade máxima de vida que o General possui.
    public int vidaMaxima = 140;

    // Quantidade de vida em que o General recua pela primeira vez.
    public int limitePrimeiraPausa = 90;

    // Quantidade de vida em que o General recua pela segunda vez.
    public int limiteSegundaPausa = 40;

    // Pontos que Maxwell recebe quando derrota o General.
    public int pontosAoDerrotar = 500;

    // Guarda a quantidade atual de vida do General.
    private int vidaAtual;

    // Impede que a primeira pausa aconteça novamente.
    private bool primeiraPausa = false;

    // Impede que a segunda pausa aconteça novamente.
    private bool segundaPausa = false;

    // Guarda se o General está escondido durante uma pausa.
    private bool escondido = false;

    // Impede que o General seja derrotado mais de uma vez.
    private bool derrotado = false;

    // SpriteRenderer usado para esconder e mostrar o General.
    private SpriteRenderer imagem;

    // Collider usado para impedir tiros enquanto o General está escondido.
    private Collider2D colisor;

    void Start()
    {
        // Começa o combate com a vida completa.
        vidaAtual = vidaMaxima;

        // Procura o SpriteRenderer do próprio General.
        imagem = GetComponent<SpriteRenderer>();

        // Procura o Collider2D utilizado para receber os tiros.
        colisor = GetComponent<Collider2D>();
    }

    public void ReceberDano(int dano)
    {
        // Se o General está escondido ou já foi derrotado, não recebe mais dano.
        if (escondido || derrotado)
        {
            return;
        }

        // Diminui a vida sem permitir valores negativos.
        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        // Verifica se o General chegou ao limite da primeira pausa.
        if (!primeiraPausa && vidaAtual <= limitePrimeiraPausa)
        {
            // Mantém a vida no limite configurado para essa etapa.
            vidaAtual = limitePrimeiraPausa;

            // Marca que a primeira pausa já aconteceu.
            primeiraPausa = true;

            // Esconde o General temporariamente.
            Esconder();
            return;
        }

        // Verifica se o General chegou ao limite da segunda pausa.
        if (!segundaPausa && vidaAtual <= limiteSegundaPausa)
        {
            // Mantém a vida no limite configurado para essa etapa.
            vidaAtual = limiteSegundaPausa;

            // Marca que a segunda pausa já aconteceu.
            segundaPausa = true;

            // Esconde o General novamente.
            Esconder();
            return;
        }

        // Se a vida chegou a zero, o General foi derrotado.
        if (vidaAtual <= 0)
        {
            // Marca que a derrota já aconteceu.
            derrotado = true;

            // Adiciona os pontos da derrota do General.
            if (PontuacaoFase4.instancia != null)
            {
                PontuacaoFase4.instancia.AdicionarPontos(pontosAoDerrotar);
            }

            // Remove o General da cena.
            Destroy(gameObject);
        }
    }

    void Esconder()
    {
        // Marca que o General está recuando.
        escondido = true;

        // Esconde a imagem para o General desaparecer da tela.
        imagem.enabled = false;

        // Desativa a colisão para impedir novos tiros durante a pausa.
        colisor.enabled = false;
    }

    public void Voltar()
    {
        // Marca que o General voltou para o combate.
        escondido = false;

        // Mostra novamente a imagem do General.
        imagem.enabled = true;

        // Reativa a colisão para permitir que ele receba tiros.
        colisor.enabled = true;
    }

    public bool EstaEscondido()
    {
        // Informa aos outros scripts se o General está escondido.
        return escondido;
    }

    public int ConsultarVida()
    {
        // Devolve a quantidade atual de vida do General.
        return vidaAtual;
    }
}
