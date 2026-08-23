using UnityEngine;

public class MovimentoCarteiro : MonoBehaviour
{
    // Configura o movimento de entrada e saída do carteiro.
    public float velocidade = 2f;
    public float distancia = 2f;

    // Referências usadas para iniciar o diálogo.
    public SistemaDialogo sistemadialogo;
    public DialogoData dialogo;
    public SpriteRenderer spriteRenderer;
    public GameObject iconeAjuda;

    [Header("Entrega das cartas")]

    // Elementos usados durante a entrega das cartas ao jogador.
    public MovimentoPersonagem jogador;
    public GameObject iconeCartas;
    public float distanciaJogador = 0.7f;
    public float velocidadeJogador = 2f;

    // Guardam as posições usadas durante a movimentação.
    private Vector3 posicaoInicial;
    private Vector3 destinoJogador;

    // Controlam cada etapa da ação do carteiro.
    private bool chegou = false;
    private bool iniciouDialogo = false;
    private bool pegandoCartas = false;
    private bool saindo = false;

    void Start()
    {
        posicaoInicial = transform.position;

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

            // Faz o jogador olhar e caminhar em direção às cartas.
            jogador.spritedopers.flipX = true;
            destinoJogador = jogador.transform.position + Vector3.right * distanciaJogador;
        }

        // Move o jogador até o ponto em que ele recebe as cartas.
        if (pegandoCartas)
        {
            jogador.transform.position = Vector3.MoveTowards(
                jogador.transform.position,
                destinoJogador,
                velocidadeJogador * Time.deltaTime
            );

            if (Vector3.Distance(jogador.transform.position, destinoJogador) < 0.01f)
            {
                pegandoCartas = false;

                // Mostra as cartas e a seta através do sistema de entregas.
                if (ControleEntrega.instancia != null)
                {
                    ControleEntrega.instancia.MostrarCartasESeta();
                }
                else
                {
                    Debug.LogError("Não existe um ControleEntrega ativo!");
                }

                // Vira o carteiro e inicia sua saída da cena.
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
        // Esconde o ícone e inicia o diálogo configurado.
        iconeAjuda.SetActive(false);
        iniciouDialogo = true;

        sistemadialogo.IniciarDialogo(dialogo);
    }
}