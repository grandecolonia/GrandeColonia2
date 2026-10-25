
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoFase2 : MonoBehaviour
{
    [Header("Pulo")]

    // Força que Maxwell recebe quando pula.
    public float forcaPulo = 10f;

    // Ponto usado para verificar se Maxwell está encostando no chão.
    public Transform pontoChao;

    // Tamanho da área que verifica se existe chão abaixo de Maxwell.
    public float raioChao = 0.15f;

    // Define quais camadas serão consideradas chão.
    public LayerMask camadaChao;

    [Header("Pulo Duplo")]

    // Permite que Maxwell pule mais uma vez enquanto estiver no ar.
    public bool permitirPuloDuplo = false;

    // Força aplicada quando Maxwell utiliza o segundo pulo.
    public float forcaSegundoPulo = 10f;

    [Header("Animação")]

    // Animator responsável por mostrar as animações de corrida e pulo.
    public Animator animator;

    // Rigidbody usado para controlar a velocidade vertical de Maxwell.
    private Rigidbody2D rb;

    // Guarda se Maxwell está encostando no chão.
    private bool estaNoChao;

    // Guarda o nome da animação atual para não reiniciá-la toda hora.
    private string animacaoAtual;

    // Controla quantos pulos Maxwell ainda pode realizar.
    private int pulosRestantes;

    // Guarda se Maxwell estava no chão no frame anterior.
    private bool estavaNoChao;

    void Start()
    {
        // Pega o Rigidbody2D do próprio Maxwell.
        rb = GetComponent<Rigidbody2D>();

        // Verifica se Maxwell já começa a fase encostando no chão.
        VerificarChao();

        // Se o pulo duplo estiver liberado, começa com dois pulos disponíveis.
        pulosRestantes = permitirPuloDuplo ? 2 : 1;

        // Guarda o estado inicial do chão para detectar quando Maxwell aterrissar.
        estavaNoChao = estaNoChao;

        // Começa a fase reproduzindo a animação de corrida.
        TrocarAnimacao("Maxwell_Correndo_Fase2");
    }

    void Update()
    {
        // Confere a cada frame se Maxwell está encostando no chão.
        VerificarChao();

        // Se Maxwell acabou de aterrissar, devolve os pulos disponíveis.
        if (estaNoChao && !estavaNoChao)
        {
            pulosRestantes = permitirPuloDuplo ? 2 : 1;
        }

        // Guarda o estado atual para comparar no próximo frame.
        estavaNoChao = estaNoChao;

        // Pega o teclado utilizando o New Input System.
        Keyboard teclado = Keyboard.current;

        // Se apertou Espaço e ainda existem pulos disponíveis, faz Maxwell pular.
        if (teclado != null && teclado.spaceKey.wasPressedThisFrame && pulosRestantes > 0)
        {
            // Começa utilizando a força normal do pulo.
            float forca = forcaPulo;

            // Se está no ar e só resta o segundo pulo, utiliza a força configurada para ele.
            if (permitirPuloDuplo && pulosRestantes == 1 && !estaNoChao)
            {
                forca = forcaSegundoPulo;
            }

            // Aplica a força vertical sem deixar Maxwell se movimentar horizontalmente.
            rb.linearVelocity = new Vector2(0f, forca);

            // Gasta um dos pulos disponíveis.
            pulosRestantes--;

            // Marca que Maxwell saiu do chão depois de pular.
            estaNoChao = false;
        }

        // Se Maxwell está no chão, mantém a animação de corrida.
        if (estaNoChao)
        {
            TrocarAnimacao("Maxwell_Correndo_Fase2");
        }
        else
        {
            // Se está no ar, mostra a animação de pulo.
            TrocarAnimacao("Maxwell_Pulando_Fase2");
        }
    }

    void FixedUpdate()
    {
        // Mantém Maxwell parado na horizontal, deixando somente o movimento vertical.
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void VerificarChao()
    {
        // Procura chão perto do ponto configurado, usando o raio e a camada escolhidos.
        estaNoChao = pontoChao != null && Physics2D.OverlapCircle(pontoChao.position, raioChao, camadaChao);
    }

    void TrocarAnimacao(string nome)
    {
        // Se a animação pedida já está tocando, não reinicia sem necessidade.
        if (animacaoAtual == nome)
        {
            return;
        }

        // Guarda qual animação Maxwell vai começar a reproduzir.
        animacaoAtual = nome;

        // Reproduz a animação escolhida desde o primeiro frame.
        animator.Play(nome, 0, 0f);
    }

    void OnDrawGizmosSelected()
    {
        // Se existe um ponto de verificação do chão, mostra sua área no Editor.
        if (pontoChao != null)
        {
            Gizmos.DrawWireSphere(pontoChao.position, raioChao);
        }
    }
}
