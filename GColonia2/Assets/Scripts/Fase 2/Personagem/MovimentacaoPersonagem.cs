
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade com que Maxwell anda para a esquerda ou direita.
    public float velocidade = 5f;

    // Rigidbody usado para controlar o movimento e o pulo.
    private Rigidbody2D rb;

    // Guarda a direção escolhida pelo jogador.
    private Vector2 movimento;

    [Header("Pulo")]

    // Força aplicada quando Maxwell pula.
    public float forcaDoPulo = 8f;

    // Guarda se Maxwell está encostando no chão.
    private bool estarNoChao;

    // Permite que outros scripts consultem se Maxwell está no chão.
    public bool EstaNoChao => estarNoChao;

    [Header("Visual")]

    // SpriteRenderer usado para virar Maxwell conforme a direção.
    public SpriteRenderer spritedopers;

    [Header("Controle")]

    // Permite bloquear o movimento durante diálogos ou eventos.
    public bool podeMover = true;

    [Header("Input")]

    // Ação usada para movimentar Maxwell.
    public InputActionReference mover;

    // Ação usada para fazer Maxwell pular.
    public InputActionReference pular;

    void OnEnable()
    {
        // Habilita a ação de movimento quando o personagem fica ativo.
        if (mover != null)
        {
            mover.action.Enable();
        }

        // Habilita a ação de pulo quando o personagem fica ativo.
        if (pular != null)
        {
            pular.action.Enable();
        }
    }

    void OnDisable()
    {
        // Desabilita a ação de movimento quando o personagem é desativado.
        if (mover != null)
        {
            mover.action.Disable();
        }

        // Desabilita a ação de pulo quando o personagem é desativado.
        if (pular != null)
        {
            pular.action.Disable();
        }
    }

    void Start()
    {
        // Pega o Rigidbody2D do próprio Maxwell.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Impede novos comandos enquanto Maxwell estiver bloqueado.
        if (!podeMover)
        {
            movimento = Vector2.zero;
            return;
        }

        // Lê a direção de movimento pelo New Input System.
        movimento = mover.action.ReadValue<Vector2>();

        // Se Maxwell está andando para a direita, mantém o sprite normal.
        if (movimento.x > 0)
        {
            spritedopers.flipX = false;
        }
        else if (movimento.x < 0)
        {
            // Se está andando para a esquerda, vira o sprite.
            spritedopers.flipX = true;
        }

        // Se o jogador apertou o comando de pulo e Maxwell está no chão, faz ele pular.
        if (pular.action.WasPressedThisFrame() && estarNoChao)
        {
            // Aplica a força vertical sem alterar a velocidade horizontal.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }
    }

    void FixedUpdate()
    {
        // Quando Maxwell está bloqueado, interrompe apenas o movimento horizontal.
        if (!podeMover)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Aplica o movimento horizontal sem interferir na velocidade do pulo.
        rb.linearVelocity = new Vector2(movimento.x * velocidade, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se Maxwell encostou em um objeto com tag Chao ou Agua, pode pular novamente.
        if (collision.gameObject.CompareTag("Chao") || collision.gameObject.CompareTag("Agua"))
        {
            estarNoChao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Se Maxwell saiu do contato com chão ou água, deixa de estar no chão.
        if (collision.gameObject.CompareTag("Chao") || collision.gameObject.CompareTag("Agua"))
        {
            estarNoChao = false;
        }
    }
}
