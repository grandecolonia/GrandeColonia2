using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoPersonagem : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade horizontal do personagem.
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Vector2 movimento;

    [Header("Pulo")]

    // Força usada para fazer o personagem pular.
    public float forcaDoPulo = 8f;

    private bool estarNoChao;

    [Header("Visual")]

    // SpriteRenderer usado para virar o personagem.
    public SpriteRenderer spritedopers;

    [Header("Controle")]

    // Permite bloquear o movimento durante diálogos ou eventos.
    public bool podeMover = true;

    [Header("Input")]

    // Actions usadas para movimentar e pular.
    public InputActionReference mover;
    public InputActionReference pular;

    void OnEnable()
    {
        // Ativa as Actions usadas pelo personagem.
        if (mover != null)
        {
            mover.action.Enable();
        }

        if (pular != null)
        {
            pular.action.Enable();
        }
    }

    void OnDisable()
    {
        // Desativa as Actions quando o objeto for desativado.
        if (mover != null)
        {
            mover.action.Disable();
        }

        if (pular != null)
        {
            pular.action.Disable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Impede novos comandos enquanto o movimento estiver bloqueado.
        if (!podeMover)
        {
            movimento = Vector2.zero;
            return;
        }

        // Lê continuamente o valor da Action de movimento.
        movimento = mover.action.ReadValue<Vector2>();

        // Vira o personagem de acordo com a direção horizontal.
        if (movimento.x > 0)
        {
            spritedopers.flipX = true;
        }
        else if (movimento.x < 0)
        {
            spritedopers.flipX = false;
        }

        // Executa o pulo somente quando o personagem estiver no chão.
        if (pular.action.WasPressedThisFrame() && estarNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }
    }

    void FixedUpdate()
    {
        // Para somente o movimento horizontal quando o personagem estiver bloqueado.
        if (!podeMover)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Aplica somente o movimento horizontal.
        rb.linearVelocity = new Vector2(movimento.x * velocidade, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Guarda que o personagem encostou no chão.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarNoChao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Guarda que o personagem saiu do chão.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarNoChao = false;
        }
    }
}