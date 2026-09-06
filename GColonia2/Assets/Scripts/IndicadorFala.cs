using UnityEngine;
using TMPro;
using System.Collections;

public class IndicadorFala : MonoBehaviour
{
    [Header("Elementos")]

    // Texto usado para mostrar os pontinhos da fala.
    public TMP_Text textoPontinhos;

    [Header("Configuração")]

    // Tempo entre cada mudança dos pontinhos.
    public float velocidadePontinhos = 0.35f;

    // Velocidade da animação de entrada e saída.
    public float velocidadeAnimacao = 5f;

    // Tempo que o balão fica escondido antes de aparecer novamente.
    public float tempoEscondido = 1f;

    // Permite atrasar o início para alternar entre NPCs.
    public float atrasoInicial = 0f;

    private Vector3 escalaInicial;

    void Start()
    {
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

            // Anima os três pontinhos.
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

            yield return new WaitForSeconds(tempoEscondido);
        }
    }
}