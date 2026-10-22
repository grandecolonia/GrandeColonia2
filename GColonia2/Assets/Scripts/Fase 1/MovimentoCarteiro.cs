
using UnityEngine;

public class MovimentoCarteiro : MonoBehaviour
{
    public float velocidade = 2f;
    public float distancia = 2f;

    public SistemaDialogo sistemadialogo;
    public DialogoData dialogo;
    public SpriteRenderer spriteRenderer;
    public GameObject iconeAjuda;

    [Header("Entrega das cartas")]
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
        posicaoInicial = transform.position;

        // Pega o componente de animação do Maxwell.
        animacaoJogador = jogador.GetComponent<AnimacaoMaxwell>();

        iconeAjuda.SetActive(false);
        iconeCartas.SetActive(false);
    }

    void Update()
    {
        // Move o carteiro para dentro da cena.
        if (!chegou)
        {
            transform.position += Vector3.left * velocidade * Time.deltaTime;

            if (transform.position.x <= posicaoInicial.x - distancia)
            {
                chegou = true;
                iconeAjuda.SetActive(true);
            }
        }

        // Inicia a entrega quando o diálogo termina.
        if (iniciouDialogo && !sistemadialogo.caixaDialogo.activeSelf && !pegandoCartas && !saindo)
        {
            pegandoCartas = true;
            jogador.podeMover = false;

            // Maxwell olha para a direita e começa a andar.
            jogador.spritedopers.flipX = false;
            destinoJogador = jogador.transform.position + Vector3.right * distanciaJogador;

            if (animacaoJogador != null)
            {
                animacaoJogador.movimentoAutomatico = true;
            }
        }

        // Move Maxwell até o ponto da entrega.
        if (pegandoCartas)
        {
            jogador.transform.position = Vector3.MoveTowards(jogador.transform.position, destinoJogador, velocidadeJogador * Time.deltaTime);

            if (Vector3.Distance(jogador.transform.position, destinoJogador) < 0.01f)
            {
                pegandoCartas = false;

                // Maxwell para de andar e volta para Idle.
                if (animacaoJogador != null)
                {
                    animacaoJogador.movimentoAutomatico = false;
                }

                // Mostra as cartas e a seta.
                if (ControleEntrega.instancia != null)
                {
                    ControleEntrega.instancia.MostrarCartasESeta();
                }
                else
                {
                    Debug.LogError("Não existe um ControleEntrega ativo!");
                }

                // Carteiro vira e sai da cena.
                spriteRenderer.flipX = !spriteRenderer.flipX;
                saindo = true;
            }
        }

        // Move o carteiro para fora da tela.
        if (saindo)
        {
            transform.position += Vector3.right * velocidade * Time.deltaTime;

            Vector3 posicaoTela = Camera.main.WorldToViewportPoint(transform.position);

            if (posicaoTela.x > 1.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ClicarIconeAjuda()
    {
        iconeAjuda.SetActive(false);
        iniciouDialogo = true;

        sistemadialogo.IniciarDialogo(dialogo);
    }
}
