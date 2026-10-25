using UnityEngine;

public class MovimentoCarteiro : MonoBehaviour
{
    [Header("Movimentação do Carteiro")]

    // Velocidade com que o carteiro anda.
    public float velocidade = 2f;

    // Distância que ele vai andar antes de parar.
    public float distancia = 2f;

    [Header("Diálogo")]

    // Sistema de diálogo e diálogo usado pelo carteiro.
    public SistemaDialogo sistemadialogo;
    public DialogoData dialogo;

    [Header("Visual")]

    // SpriteRenderer usado pra virar o carteiro quando ele for embora.
    public SpriteRenderer spriteRenderer;

    // Animator que controla parado/andando.
    public Animator animator;

    // Ícone de ajuda que aparece quando ele para.
    public GameObject iconeAjuda;

    [Header("Entrega das cartas")]

    // Referências ligadas ao Maxwell e ao momento da entrega.
    public MovimentoPersonagem jogador;
    public GameObject iconeCartas;
    public float distanciaJogador = 0.7f;
    public float velocidadeJogador = 2f;

    private Vector3 posicaoInicial;
    private Vector3 destinoJogador;

    private AnimacaoMaxwell animacaoJogador;

    private bool chegou = false;
    private bool iniciouDialogo = false;
    private bool pegandoCartas = false;
    private bool saindo = false;

    void Start()
    {
        // Guarda a posição inicial do carteiro pra saber onde ele deve parar.
        posicaoInicial = transform.position;

        // Pega o script de animação do Maxwell.
        animacaoJogador = jogador.GetComponent<AnimacaoMaxwell>();

        // Começa escondendo os ícones.
        iconeAjuda.SetActive(false);
        iconeCartas.SetActive(false);

        // Já deixa o carteiro andando quando a cena começa.
        AtualizarAnimacao(true);
    }

    void Update()
    {
        // Enquanto ainda não chegou no ponto, ele entra andando na cena.
        if (!chegou)
        {
            AtualizarAnimacao(true);

            transform.position += Vector3.left * velocidade * Time.deltaTime;

            if (transform.position.x <= posicaoInicial.x - distancia)
            {
                chegou = true;

                // Quando ele chega, para de andar e mostra o ícone.
                AtualizarAnimacao(false);
                iconeAjuda.SetActive(true);
            }

            return;
        }

        // Depois que o diálogo terminar, Maxwell anda até pegar as cartas.
        if (iniciouDialogo && !sistemadialogo.caixaDialogo.activeSelf && !pegandoCartas && !saindo)
        {
            pegandoCartas = true;
            jogador.podeMover = false;

            // Maxwell olha pra direita e anda até o ponto da entrega.
            jogador.spritedopers.flipX = false;
            destinoJogador = jogador.transform.position + Vector3.right * distanciaJogador;

            if (animacaoJogador != null)
            {
                animacaoJogador.movimentoAutomatico = true;
            }
        }

        // Enquanto Maxwell vai pegar as cartas, o carteiro fica parado.
        if (pegandoCartas)
        {
            AtualizarAnimacao(false);

            jogador.transform.position = Vector3.MoveTowards(jogador.transform.position, destinoJogador, velocidadeJogador * Time.deltaTime);

            if (Vector3.Distance(jogador.transform.position, destinoJogador) < 0.01f)
            {
                pegandoCartas = false;

                // Maxwell volta pro idle.
                if (animacaoJogador != null)
                {
                    animacaoJogador.movimentoAutomatico = false;
                }

                // Mostra as cartas e a seta.
                if (ControleEntrega.instancia != null)
                {
                    ControleEntrega.instancia.MostrarCartasESeta();
                }

                // Vira o carteiro e manda ele embora.
                spriteRenderer.flipX = !spriteRenderer.flipX;
                saindo = true;
            }

            return;
        }

        // Quando ele sai da cena, volta a animação de andar.
        if (saindo)
        {
            AtualizarAnimacao(true);

            transform.position += Vector3.right * velocidade * Time.deltaTime;

            Vector3 posicaoTela = Camera.main.WorldToViewportPoint(transform.position);

            if (posicaoTela.x > 1.1f)
            {
                gameObject.SetActive(false);
            }

            return;
        }

        // Se não estiver entrando nem saindo, ele fica parado.
        AtualizarAnimacao(false);
    }

    public void ClicarIconeAjuda()
    {
        // Some com o ícone e inicia o diálogo.
        iconeAjuda.SetActive(false);
        iniciouDialogo = true;

        // Enquanto conversa, ele fica parado.
        AtualizarAnimacao(false);

        sistemadialogo.IniciarDialogo(dialogo);
    }

    void AtualizarAnimacao(bool andando)
    {
        // Troca entre parado e andando no Animator.
        if (animator != null)
        {
            animator.SetBool("andando", andando);
        }
    }
}