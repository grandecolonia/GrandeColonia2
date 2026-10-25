
using UnityEngine;
using TMPro;
using System.Collections;

public class IndicadorFala : MonoBehaviour
{
    [Header("Elementos")]

    // Texto responsável por mostrar os pontinhos dentro do balão.
    public TMP_Text textoPontinhos;

    [Header("Configuração")]

    // Tempo de espera entre cada mudança dos pontinhos.
    public float velocidadePontinhos = 0.35f;

    // Controla a velocidade com que o balão aumenta e diminui.
    public float velocidadeAnimacao = 5f;

    // Tempo que o balão fica escondido antes de aparecer novamente.
    public float tempoEscondido = 1f;

    // Espera inicial para não deixar todos os NPCs animando ao mesmo tempo.
    public float atrasoInicial = 0f;

    // Guarda o tamanho original para o balão saber até onde precisa crescer.
    private Vector3 escalaInicial;

    void Start()
    {
        // Salva o tamanho original do balão antes de começar a animação.
        escalaInicial = transform.localScale;

        // Começa com o balão completamente escondido.
        transform.localScale = Vector3.zero;

        // Inicia a sequência de aparecimento, pontinhos e desaparecimento.
        StartCoroutine(AnimarFala());
    }

    IEnumerator AnimarFala()
    {
        // Espera o tempo inicial antes de começar a primeira animação.
        yield return new WaitForSeconds(atrasoInicial);

        // Mantém a animação se repetindo enquanto o objeto estiver ativo.
        while (true)
        {
            // Faz o balão crescer suavemente até chegar perto do tamanho original.
            while (transform.localScale.x < escalaInicial.x * 0.99f)
            {
                // Aproxima o tamanho atual do tamanho original do balão.
                transform.localScale = Vector3.Lerp(transform.localScale, escalaInicial, velocidadeAnimacao * Time.deltaTime);

                // Espera o próximo frame para continuar aumentando.
                yield return null;
            }

            // Garante que o balão termine exatamente no tamanho original.
            transform.localScale = escalaInicial;

            // Mostra o primeiro pontinho da fala.
            textoPontinhos.text = ".";
            yield return new WaitForSeconds(velocidadePontinhos);

            // Mostra dois pontinhos para continuar o efeito.
            textoPontinhos.text = "..";
            yield return new WaitForSeconds(velocidadePontinhos);

            // Mostra os três pontinhos antes de esconder o balão.
            textoPontinhos.text = "...";
            yield return new WaitForSeconds(velocidadePontinhos);

            // Faz o balão diminuir suavemente até ficar praticamente invisível.
            while (transform.localScale.x > 0.02f)
            {
                // Aproxima o tamanho do balão de zero para criar o efeito de desaparecimento.
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, velocidadeAnimacao * Time.deltaTime);

                // Espera o próximo frame antes de continuar diminuindo.
                yield return null;
            }

            // Garante que o balão fique completamente escondido.
            transform.localScale = Vector3.zero;

            // Espera um pouco antes de repetir toda a sequência.
            yield return new WaitForSeconds(tempoEscondido);
        }
    }
}
