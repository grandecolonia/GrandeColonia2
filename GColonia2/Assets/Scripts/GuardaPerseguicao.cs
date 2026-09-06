using UnityEngine;
using System.Collections.Generic;

public class GuardaPerseguicao : MonoBehaviour
{
    [Header("Referências")]

    public Transform jogador;
    public ControleLabirinto controleLabirinto;
    public GradeNavegacao gradeNavegacao;

    [Header("Visual")]

    public Transform visual;
    public Animator animator;

    [Header("Movimento")]

    public float velocidade = 3f;
    public float distanciaChegarPonto = 0.1f;

    [Header("Navegação")]

    // Recalcula constantemente a rota até a posição atual de Maxwell.
    public float intervaloRecalculo = 0.15f;

    private Rigidbody2D rb;

    private List<Vector2> caminhoAtual = new List<Vector2>();
    private int indiceCaminho = 0;

    private float tempoRecalculo = 0f;
    private Vector2 direcao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (gradeNavegacao == null)
        {
            gradeNavegacao = FindFirstObjectByType<GradeNavegacao>();
        }
    }

    void FixedUpdate()
    {
        if (!controleLabirinto.alertaAtivo)
        {
            Parar();
            return;
        }

        tempoRecalculo -= Time.fixedDeltaTime;

        if (tempoRecalculo <= 0f)
        {
            RecalcularCaminho();
            tempoRecalculo = intervaloRecalculo;
        }

        SeguirCaminho();
    }

    void RecalcularCaminho()
    {
        if (gradeNavegacao == null)
        {
            Debug.LogError("Guarda: GradeNavegacao não configurada!");
            Parar();
            return;
        }

        caminhoAtual = gradeNavegacao.EncontrarCaminho(
            rb.position,
            jogador.position
        );

        indiceCaminho = 0;

        // Ignora pontos que já estão embaixo do guarda.
        while (
            indiceCaminho < caminhoAtual.Count &&
            Vector2.Distance(rb.position, caminhoAtual[indiceCaminho]) <= distanciaChegarPonto
        )
        {
            indiceCaminho++;
        }
    }

    void SeguirCaminho()
    {
        if (caminhoAtual == null || caminhoAtual.Count == 0)
        {
            Parar();
            return;
        }

        if (indiceCaminho >= caminhoAtual.Count)
        {
            Parar();
            return;
        }

        Vector2 destino = caminhoAtual[indiceCaminho];

        if (Vector2.Distance(rb.position, destino) <= distanciaChegarPonto)
        {
            indiceCaminho++;

            if (indiceCaminho >= caminhoAtual.Count)
            {
                Parar();
                return;
            }

            destino = caminhoAtual[indiceCaminho];
        }

        MoverPara(destino);
    }

    void MoverPara(Vector2 destino)
    {
        Vector2 diferenca = destino - rb.position;

        if (diferenca.sqrMagnitude < 0.001f)
        {
            return;
        }

        direcao = diferenca.normalized;

        Vector2 novaPosicao = Vector2.MoveTowards(
            rb.position,
            destino,
            velocidade * Time.fixedDeltaTime
        );

        rb.MovePosition(novaPosicao);

        if (animator != null)
        {
            animator.SetBool("andando", true);
        }

        AtualizarDirecaoVisual();
    }

    void Parar()
    {
        direcao = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetBool("andando", false);
        }
    }

    void AtualizarDirecaoVisual()
    {
        if (visual == null || direcao == Vector2.zero)
        {
            return;
        }

        if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
        {
            if (direcao.x > 0)
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
            if (direcao.y > 0)
            {
                visual.localRotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                visual.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}