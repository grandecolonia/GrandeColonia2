
using UnityEngine;

public class AnimacaoMaxwellFase2 : MonoBehaviour
{
    [Header("Animação")]

    // Animator responsável por controlar as animações do Maxwell.
    public Animator animator;

    // Rigidbody usado para descobrir se Maxwell está se movimentando.
    private Rigidbody2D rb;

    // Script de movimentação que informa se Maxwell está no chão e pode andar.
    private MovimentacaoPersonagem movimento;

    void Start()
    {
        // Pega o Rigidbody2D do próprio Maxwell.
        rb = GetComponent<Rigidbody2D>();

        // Procura o script que controla a movimentação do personagem.
        movimento = GetComponent<MovimentacaoPersonagem>();

        // Se o Animator não foi colocado no Inspector, tenta encontrar no próprio objeto.
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Se algum componente necessário estiver faltando, não tenta atualizar as animações.
        if (animator == null || rb == null || movimento == null)
        {
            return;
        }

        // Verifica se Maxwell está encostando no chão.
        bool noChao = movimento.EstaNoChao;

        // Confere se Maxwell pode andar e está realmente se movimentando para os lados.
        bool andando = movimento.podeMover && Mathf.Abs(rb.linearVelocity.x) > 0.05f;

        // Avisa ao Animator se Maxwell está no chão ou no ar.
        animator.SetBool("NoChao", noChao);

        // Só ativa a animação de caminhada se Maxwell estiver andando no chão.
        animator.SetBool("Andando", andando && noChao);
    }
}
