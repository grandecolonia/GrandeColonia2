
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SistemaEscada : MonoBehaviour
{
    [Header("Pontos da Escada")]
    public Transform pontoInferior;
    public Transform pontoSuperior;

    [Header("Configurações")]
    public float velocidade = 2f;
    public float alturaFinal = 0.2f;

    private Rigidbody2D rb;
    private MovimentacaoPersonagem movimento;
    private Animator animator;

    private bool pertoDaEscada = false;
    private bool subindo = false;
    private bool indoParaCima = false;

    private Transform destino;
    private float gravidadeOriginal;

    void Update()
    {
        if (pertoDaEscada && !subindo && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(UsarEscada());
        }
    }

    public void EntrarNaArea(Collider2D colisao)
    {
        if (subindo) return;

        rb = colisao.GetComponent<Rigidbody2D>();
        movimento = colisao.GetComponent<MovimentacaoPersonagem>();

        if (rb == null || movimento == null) return;

        animator = colisao.GetComponent<Animator>();
        pertoDaEscada = true;

        float distanciaInferior = Vector2.Distance(rb.position, pontoInferior.position);
        float distanciaSuperior = Vector2.Distance(rb.position, pontoSuperior.position);

        indoParaCima = distanciaInferior < distanciaSuperior;
        destino = indoParaCima ? pontoSuperior : pontoInferior;
    }

    public void SairDaArea()
    {
        if (!subindo)
        {
            pertoDaEscada = false;
        }
    }

    IEnumerator UsarEscada()
    {
        subindo = true;
        pertoDaEscada = false;
        movimento.podeMover = false;

        gravidadeOriginal = rb.gravityScale;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.simulated = false;

        // Alinha Maxwell com a escada.
        Vector3 inicio = rb.transform.position;
        inicio.x = pontoInferior.position.x;
        rb.transform.position = inicio;

        if (animator != null)
        {
            animator.SetBool("SubindoEscada", true);
        }

        // Sobe ou desce somente na vertical.
        float alturaDestino = indoParaCima ? pontoSuperior.position.y - alturaFinal : pontoInferior.position.y;

        Vector3 posicaoEscada = new Vector3(inicio.x, alturaDestino, inicio.z);

        while (Vector3.Distance(rb.transform.position, posicaoEscada) > 0.05f)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, posicaoEscada, velocidade * Time.deltaTime);
            yield return null;
        }

        // Teletransporta diretamente para o local seguro.
        rb.transform.position = new Vector3(destino.position.x, destino.position.y, rb.transform.position.z);

        if (animator != null)
        {
            animator.SetBool("SubindoEscada", false);
        }

        rb.gravityScale = gravidadeOriginal;
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForFixedUpdate();

        movimento.podeMover = true;
        subindo = false;
    }
}
