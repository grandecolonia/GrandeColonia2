
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentacaoFase2 : MonoBehaviour
{
    [Header("Pulo")]
    public float forcaPulo = 10f;
    public Transform pontoChao;
    public float raioChao = 0.15f;
    public LayerMask camadaChao;

    [Header("Animação")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool estaNoChao;
    private string animacaoAtual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        VerificarChao();
        TrocarAnimacao("Maxwell_Correndo_Fase2");
    }

    void Update()
    {
        VerificarChao();

        Keyboard teclado = Keyboard.current;

        // Maxwell só pode pular.
        if (teclado != null && teclado.spaceKey.wasPressedThisFrame && estaNoChao)
        {
            rb.linearVelocity = new Vector2(0, forcaPulo);
            estaNoChao = false;
        }

        // Alterna entre correr e pular.
        if (estaNoChao)
        {
            TrocarAnimacao("Maxwell_Correndo_Fase2");
        }
        else
        {
            TrocarAnimacao("Maxwell_Pulando_Fase2");
        }
    }

    void FixedUpdate()
    {
        // Mantém Maxwell na posição horizontal.
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void VerificarChao()
    {
        estaNoChao = pontoChao != null && Physics2D.OverlapCircle(pontoChao.position, raioChao, camadaChao);
    }

    void TrocarAnimacao(string nome)
    {
        if (animacaoAtual == nome) return;

        animacaoAtual = nome;
        animator.Play(nome, 0, 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (pontoChao != null)
        {
            Gizmos.DrawWireSphere(pontoChao.position, raioChao);
        }
    }
}
