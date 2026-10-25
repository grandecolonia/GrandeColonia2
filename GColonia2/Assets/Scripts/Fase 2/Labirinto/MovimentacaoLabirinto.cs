
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoLabirinto : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade com que Maxwell se movimenta pelo labirinto.
    public float velocidade = 5f;

    // Permite bloquear o movimento durante diálogos ou eventos.
    public bool podeMover = true;

    // Rigidbody usado para movimentar Maxwell.
    private Rigidbody2D rb;

    // Guarda a direção escolhida pelo jogador.
    private Vector2 movimento;

    [Header("Visual")]

    // Animator responsável pela animação de caminhada.
    public Animator animator;

    // Objeto visual que gira conforme a direção do movimento.
    public Transform visual;

    [Header("Input")]

    // Ação que permite movimentar Maxwell nas quatro direções.
    public InputActionReference mover;

    void OnEnable()
    {
        // Habilita a ação de movimento quando o personagem fica ativo.
        if (mover != null)
        {
            mover.action.Enable();
        }
    }

    void OnDisable()
    {
        // Desabilita a ação de movimento quando o personagem é desativado.
        if (mover != null)
        {
            mover.action.Disable();
        }
    }

    void Start()
    {
        // Pega o Rigidbody2D do próprio Maxwell.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Se Maxwell não pode se mover, interrompe o movimento e a animação.
        if (!podeMover)
        {
            movimento = Vector2.zero;
            animator.SetBool("andando", false);
            return;
        }

        // Lê a direção do jogador e normaliza para evitar movimento diagonal mais rápido.
        movimento = mover.action.ReadValue<Vector2>().normalized;

        // Ativa a animação de caminhada somente quando Maxwell está se movimentando.
        animator.SetBool("andando", movimento.sqrMagnitude > 0.01f);

        // Vira Maxwell para a direção em que ele está andando.
        AtualizarDirecao();
    }

    void FixedUpdate()
    {
        // Se Maxwell estiver bloqueado, para completamente o Rigidbody.
        if (!podeMover)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Aplica a velocidade na direção escolhida pelo jogador.
        rb.linearVelocity = movimento * velocidade;
    }

    void AtualizarDirecao()
    {
        // Se Maxwell está parado, mantém a última direção visual.
        if (movimento == Vector2.zero)
        {
            return;
        }

        // Verifica se Maxwell está andando mais na horizontal ou na vertical.
        if (Mathf.Abs(movimento.x) > Mathf.Abs(movimento.y))
        {
            // Se está indo para a direita, gira o visual para esse lado.
            if (movimento.x > 0)
            {
                visual.localRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                // Se está indo para a esquerda, gira para o lado contrário.
                visual.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
        else
        {
            // Se está subindo, faz Maxwell olhar para cima.
            if (movimento.y > 0)
            {
                visual.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                // Se está descendo, faz Maxwell olhar para baixo.
                visual.localRotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
    }
}
