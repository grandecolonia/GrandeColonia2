using UnityEngine;
using TMPro;
using System.Collections;

public class IndicadorFala : MonoBehaviour
{
    [Header("Elementos")]

    // Texto que mostra os pontinhos dentro do balão.
    public TMP_Text textoPontinhos;

    [Header("Configuração")]

    // Tempo entre cada mudança dos pontinhos.
    public float velocidadePontinhos = 0.35f;

    // Velocidade que o balão aparece e desaparece.
    public float velocidadeAnimacao = 5f;

    // Tempo que o balão fica escondido antes de aparecer de novo.
    public float tempoEscondido = 1f;

    // Atraso antes de começar a animação, útil para alternar entre NPCs.
    public float atrasoInicial = 0f;

    private Vector3 escalaInicial;

    void Start()
    {
        // Guarda o tamanho original e começa com o balão escondido.
        escalaInicial = transform.localScale;
        transform.localScale = Vector3.zero;

        StartCoroutine(AnimarFala());
    }

    IEnumerator AnimarFala()
    {
        yield return new WaitForSeconds(atrasoInicial);

        while (true)
        {
            // Faz o balão aparecer.
            while (transform.localScale.x < escalaInicial.x * 0.99f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, escalaInicial, velocidadeAnimacao * Time.deltaTime);
                yield return null;
            }

            transform.localScale = escalaInicial;

            // Anima os pontinhos da fala.
            textoPontinhos.text = ".";
            yield return new WaitForSeconds(velocidadePontinhos);

            textoPontinhos.text = "..";
            yield return new WaitForSeconds(velocidadePontinhos);

            textoPontinhos.text = "...";
            yield return new WaitForSeconds(velocidadePontinhos);

            // Faz o balão desaparecer.
            while (transform.localScale.x > 0.02f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, velocidadeAnimacao * Time.deltaTime);
                yield return null;
            }

            transform.localScale = Vector3.zero;

            // Espera um pouco antes de repetir a animação.
            yield return new WaitForSeconds(tempoEscondido);
        }
    }
}