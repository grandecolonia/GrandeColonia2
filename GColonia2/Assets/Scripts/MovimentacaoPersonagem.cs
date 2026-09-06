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

    // Força aplicada ao personagem ao pular.
    public float forcaDoPulo = 8f;

    private bool estarNoChao;

    [Header("Visual")]

    // SpriteRenderer usado para virar o personagem.
    public SpriteRenderer spritedopers;

    [Header("Controle")]

    // Permite bloquear o movimento durante diálogos ou eventos.
    public bool podeMover = true;

    [Header("Input")]

    // Actions responsáveis pelos comandos do personagem.
    public InputActionReference mover;
    public InputActionReference pular;

    void OnEnable()
    {
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
        if (!podeMover)
        {
            movimento = Vector2.zero;
            return;
        }

        // Lê continuamente o valor da Action de movimento.
        movimento = mover.action.ReadValue<Vector2>();

        // Vira o personagem de acordo com a direção.
        if (movimento.x > 0)
        {
            spritedopers.flipX = true;
        }
        else if (movimento.x < 0)
        {
            spritedopers.flipX = false;
        }

        // Executa o pulo quando a Action for pressionada.
        if (pular.action.WasPressedThisFrame() && estarNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }
    }

    void FixedUpdate()
    {
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
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarNoChao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarNoChao = false;
        }
    }
}