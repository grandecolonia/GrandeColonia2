using UnityEngine;
using UnityEngine.InputSystem;

public class AnimacaoMaxwellFase2 : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D rb;
    private MovimentacaoPersonagem movimento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movimento = GetComponent<MovimentacaoPersonagem>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (animator == null || rb == null || movimento == null)
        {
            return;
        }

        // Atualiza as animações conforme o movimento e o chão.
        bool noChao = movimento.EstaNoChao;
        bool andando = movimento.podeMover && Mathf.Abs(rb.linearVelocity.x) > 0.05f;

        animator.SetBool("NoChao", noChao);
        animator.SetBool("Andando", andando && noChao);
    }
}