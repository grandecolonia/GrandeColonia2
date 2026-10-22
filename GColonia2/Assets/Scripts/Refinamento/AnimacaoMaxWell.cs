
using UnityEngine;

public class AnimacaoMaxwell : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D rb;
    private MovimentoPersonagem movimento;

    // Indica quando outro script está movimentando Maxwell.
    public bool movimentoAutomatico = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movimento = GetComponent<MovimentoPersonagem>();

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

        bool noChao = movimento.EstaNoChao;
        bool andando = movimentoAutomatico || (movimento.podeMover && Mathf.Abs(rb.linearVelocity.x) > 0.05f);

        animator.SetBool("NoChao", noChao);
        animator.SetBool("Andando", andando && noChao);
    }
}
