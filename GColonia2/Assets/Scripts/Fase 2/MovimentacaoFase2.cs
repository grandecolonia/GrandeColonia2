using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovimentacaoFase2 : MonoBehaviour
{
    [Header("Pulo")]

    // Força usada para fazer Maxwell pular.
    public float forcaPulo = 10f;

    // Ponto usado para verificar se Maxwell está encostando no chão.
    public Transform pontoChao;

    // Tamanho da área usada para detectar o chão.
    public float raioChao = 0.15f;

    // Layer usada para identificar o chão.
    public LayerMask camadaChao;

    [Header("Animação")]

    // Animator usado para trocar as animações do Maxwell.
    public Animator animator;

    private Rigidbody2D rb;
    private bool estaNoChao;
    private bool estavaNoChao;
    private string animacaoAtual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Começa a fase com Maxwell correndo.
        TrocarAnimacao("Maxwell_Correndo_Fase2");
    }

    void Update()
    {
        VerificarChao();

        // Faz Maxwell pular com Espaço somente quando estiver no chão.
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaNoChao)
        {
            Pular();
        }

        AtualizarAnimacao();

        // Guarda o estado anterior para saber quando Maxwell acabou de tocar o chão.
        estavaNoChao = estaNoChao;
    }

    void Pular()
    {
        // Aplica a velocidade vertical do pulo.
        rb.linearVelocity = new Vector2(0, forcaPulo);
    }

    void VerificarChao()
    {
        // Verifica se existe algum objeto da camada de chão abaixo de Maxwell.
        estaNoChao = Physics2D.OverlapCircle(pontoChao.position, raioChao, camadaChao);
    }

    void AtualizarAnimacao()
    {
        // Enquanto estiver no ar, escolhe entre subida e queda.
        if (!estaNoChao)
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                TrocarAnimacao("Maxwell_PuloSubindo_Fase2");
            }
            else
            {
                TrocarAnimacao("Maxwell_PuloCaindo_Fase2");
            }

            return;
        }

        // Se acabou de tocar o chão, toca a animação de aterrissagem.
        if (!estavaNoChao)
        {
            TrocarAnimacao("Maxwell_Aterrissando_Fase2");
            StartCoroutine(VoltarParaCorrida());

            return;
        }

        // Se estiver normalmente no chão, continua correndo.
        TrocarAnimacao("Maxwell_Correndo_Fase2");
    }

    void TrocarAnimacao(string nome)
    {
        // Evita reiniciar a mesma animação a cada frame.
        if (animacaoAtual == nome)
        {
            return;
        }

        animacaoAtual = nome;
        animator.Play(nome);
    }

    IEnumerator VoltarParaCorrida()
    {
        // Dá um pequeno tempo para a animação de aterrissagem aparecer.
        yield return new WaitForSeconds(0.08f);

        if (estaNoChao)
        {
            TrocarAnimacao("Maxwell_Correndo_Fase2");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Mostra no Scene View a área usada para detectar o chão.
        if (pontoChao != null)
        {
            Gizmos.DrawWireSphere(pontoChao.position, raioChao);
        }
    }
}