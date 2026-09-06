using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoLabirinto : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade do personagem dentro do labirinto.
    public float velocidade = 5f;

    // Permite bloquear o movimento durante eventos ou interações.
    public bool podeMover = true;

    private Rigidbody2D rb;
    private Vector2 movimento;

    [Header("Visual")]

    // Elementos responsáveis pela animação e direção visual do personagem.
    public Animator animator;
    public Transform visual;

    [Header("Input")]

    // Action responsável pelo movimento em todas as direções.
    public InputActionReference mover;

    void OnEnable()
    {
        if (mover != null)
        {
            mover.action.Enable();
        }
    }

    void OnDisable()
    {
        if (mover != null)
        {
            mover.action.Disable();
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
            animator.SetBool("andando", false);
            return;
        }

        // Lê o movimento horizontal e vertical pelo New Input System.
        movimento = mover.action.ReadValue<Vector2>().normalized;

        // Ativa a animação enquanto o personagem estiver se movimentando.
        animator.SetBool("andando", movimento.sqrMagnitude > 0.01f);

        // Atualiza a direção visual do personagem.
        AtualizarDirecao();
    }

    void FixedUpdate()
    {
        if (!podeMover)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Movimenta o personagem em todas as direções.
        rb.linearVelocity = movimento * velocidade;
    }

    void AtualizarDirecao()
{
    if (movimento == Vector2.zero)
    {
        return;
    }

    if (Mathf.Abs(movimento.x) > Mathf.Abs(movimento.y))
    {
        if (movimento.x > 0)
        {
            visual.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            visual.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }
    else
    {
        if (movimento.y > 0)
        {
            visual.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            visual.localRotation = Quaternion.Euler(0, 0, 180);
        }
    }
}
}