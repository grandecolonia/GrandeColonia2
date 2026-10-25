
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SistemaEscada : MonoBehaviour
{
    [Header("Pontos da Escada")]

    // Ponto onde Maxwell fica na parte de baixo da escada.
    public Transform pontoInferior;

    // Ponto onde Maxwell fica na parte de cima da escada.
    public Transform pontoSuperior;

    [Header("Configurações")]

    // Velocidade com que Maxwell sobe ou desce.
    public float velocidade = 2f;

    // Pequeno ajuste na altura final quando Maxwell sobe.
    public float alturaFinal = 0.2f;

    // Rigidbody usado para controlar o movimento durante a escada.
    private Rigidbody2D rb;

    // Script responsável pela movimentação normal de Maxwell.
    private MovimentacaoPersonagem movimento;

    // Animator usado para controlar a animação de subida.
    private Animator animator;

    // Informa se Maxwell está perto de uma das entradas da escada.
    private bool pertoDaEscada = false;

    // Impede que a escada seja utilizada mais de uma vez ao mesmo tempo.
    private bool subindo = false;

    // Guarda se Maxwell vai subir ou descer a escada.
    private bool indoParaCima = false;

    // Ponto de destino escolhido conforme o lado em que Maxwell entrou.
    private Transform destino;

    // Guarda a gravidade original para restaurar depois da animação.
    private float gravidadeOriginal;

    void Update()
    {
        // Se Maxwell está perto, não está subindo e apertou E, começa a usar a escada.
        if (pertoDaEscada && !subindo && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(UsarEscada());
        }
    }

    public void EntrarNaArea(Collider2D colisao)
    {
        // Se Maxwell já está utilizando a escada, não começa outra interação.
        if (subindo)
        {
            return;
        }

        // Procura o Rigidbody e o script de movimento de Maxwell.
        rb = colisao.GetComponent<Rigidbody2D>();
        movimento = colisao.GetComponent<MovimentacaoPersonagem>();

        // Se não encontrou os componentes necessários, cancela a interação.
        if (rb == null || movimento == null)
        {
            return;
        }

        // Procura o Animator para executar a animação de subida.
        animator = colisao.GetComponent<Animator>();

        // Marca que Maxwell pode utilizar a escada.
        pertoDaEscada = true;

        // Calcula a distância entre Maxwell e a parte inferior da escada.
        float distanciaInferior = Vector2.Distance(rb.position, pontoInferior.position);

        // Calcula a distância entre Maxwell e a parte superior da escada.
        float distanciaSuperior = Vector2.Distance(rb.position, pontoSuperior.position);

        // Se Maxwell está mais perto da parte inferior, significa que vai subir.
        indoParaCima = distanciaInferior < distanciaSuperior;

        // Escolhe o destino conforme o lado em que Maxwell começou.
        destino = indoParaCima ? pontoSuperior : pontoInferior;
    }

    public void SairDaArea()
    {
        // Se Maxwell não está utilizando a escada, cancela a possibilidade de interação.
        if (!subindo)
        {
            pertoDaEscada = false;
        }
    }

    IEnumerator UsarEscada()
    {
        // Marca que Maxwell começou a utilizar a escada.
        subindo = true;

        // Impede que outra interação seja iniciada durante a animação.
        pertoDaEscada = false;

        // Bloqueia os comandos de movimentação de Maxwell.
        movimento.podeMover = false;

        // Guarda a gravidade original antes de desligá-la.
        gravidadeOriginal = rb.gravityScale;

        // Interrompe a velocidade atual de Maxwell.
        rb.linearVelocity = Vector2.zero;

        // Desliga a gravidade para Maxwell não cair durante a subida.
        rb.gravityScale = 0f;

        // Desativa temporariamente a simulação física.
        rb.simulated = false;

        // Guarda a posição atual do personagem.
        Vector3 inicio = rb.transform.position;

        // Alinha Maxwell horizontalmente com a escada.
        inicio.x = pontoInferior.position.x;

        // Coloca Maxwell na posição alinhada.
        rb.transform.position = inicio;

        // Se existe um Animator, ativa a animação de subida.
        if (animator != null)
        {
            animator.SetBool("SubindoEscada", true);
        }

        // Escolhe a altura final conforme Maxwell esteja subindo ou descendo.
        float alturaDestino = indoParaCima ? pontoSuperior.position.y - alturaFinal : pontoInferior.position.y;

        // Monta a posição que Maxwell deve alcançar durante a animação.
        Vector3 posicaoEscada = new Vector3(inicio.x, alturaDestino, inicio.z);

        // Continua movimentando Maxwell até chegar perto da altura desejada.
        while (Vector3.Distance(rb.transform.position, posicaoEscada) > 0.05f)
        {
            // Move Maxwell suavemente pela escada.
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, posicaoEscada, velocidade * Time.deltaTime);

            // Espera o próximo frame para continuar o movimento.
            yield return null;
        }

        // Coloca Maxwell exatamente no ponto de saída da escada.
        rb.transform.position = new Vector3(destino.position.x, destino.position.y, rb.transform.position.z);

        // Desliga a animação de subida.
        if (animator != null)
        {
            animator.SetBool("SubindoEscada", false);
        }

        // Restaura a gravidade original do personagem.
        rb.gravityScale = gravidadeOriginal;

        // Ativa novamente a simulação física.
        rb.simulated = true;

        // Garante que Maxwell não saia da escada com velocidade acumulada.
        rb.linearVelocity = Vector2.zero;

        // Espera a próxima atualização da física antes de liberar os controles.
        yield return new WaitForFixedUpdate();

        // Permite que Maxwell volte a se movimentar normalmente.
        movimento.podeMover = true;

        // Marca que Maxwell terminou de utilizar a escada.
        subindo = false;
    }
}
