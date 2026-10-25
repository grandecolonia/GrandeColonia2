
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscadaFinal : MonoBehaviour
{
    [Header("Personagem")]

    // Rigidbody de Maxwell que será movimentado pela escada.
    public Rigidbody2D rb;

    [Header("Escada")]

    // Ponto onde Maxwell deve chegar antes de trocar de cena.
    public Transform destino;

    // Velocidade com que Maxwell sobe a escada.
    public float velocidade = 2f;

    // Impede que a subida seja iniciada mais de uma vez.
    private bool subindo = false;

    void OnTriggerEnter2D(Collider2D colisao)
    {
        // Se Maxwell entrou na área e ainda não está subindo, começa a interação.
        if (colisao.CompareTag("Player") && !subindo)
        {
            // Pega o Rigidbody do personagem que entrou na área.
            rb = colisao.attachedRigidbody;

            // Se encontrou o Rigidbody, inicia a subida automática.
            if (rb != null)
            {
                StartCoroutine(SubirEscada());
            }
        }
    }

    IEnumerator SubirEscada()
    {
        // Marca que Maxwell começou a subir.
        subindo = true;

        // Procura os componentes de movimento e animação do personagem.
        MovimentacaoPersonagem movimento = rb.GetComponent<MovimentacaoPersonagem>();
        Animator animator = rb.GetComponent<Animator>();

        // Bloqueia os comandos enquanto Maxwell sobe a escada.
        if (movimento != null)
        {
            movimento.podeMover = false;
        }

        // Ativa a animação de subida.
        if (animator != null)
        {
            animator.SetBool("SubindoEscada", true);
        }

        // Para o movimento e desliga a gravidade durante a subida.
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.simulated = false;

        // Continua movimentando Maxwell até chegar perto do destino.
        while (Vector3.Distance(rb.transform.position, destino.position) > 0.05f)
        {
            // Move Maxwell suavemente em direção ao ponto final.
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, destino.position, velocidade * Time.deltaTime);

            // Espera o próximo frame para continuar a subida.
            yield return null;
        }

        // Coloca Maxwell exatamente no ponto final da escada.
        rb.transform.position = destino.position;

        // Carrega a próxima cena quando a subida termina.
        SceneManager.LoadScene("19_Portão");
    }
}
