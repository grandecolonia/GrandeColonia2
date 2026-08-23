using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [Header("Elementos da tela")]

    // Elementos que aparecem durante a apresentação inicial.
    public CanvasGroup logoFavoStudio;
    public CanvasGroup textoApresenta;
    public CanvasGroup LogodoJogo;

    [Header("Configuração")]

    // Controlam a velocidade das transições e o tempo de exibição.
    public float velocidadeFade = 1f;
    public float tempoVisivel = 1.5f;

    void Start()
    {
        // Todos os elementos começam invisíveis.
        logoFavoStudio.alpha = 0;
        textoApresenta.alpha = 0;
        LogodoJogo.alpha = 0;

        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {
        // Mostra primeiro o logo do estúdio.
        yield return StartCoroutine(FadeIn(logoFavoStudio));

        yield return new WaitForSeconds(0.2f);

        // O texto aparece junto do logo.
        StartCoroutine(FadeIn(textoApresenta));

        yield return new WaitForSeconds(tempoVisivel);

        // Esconde o logo e o texto ao mesmo tempo.
        StartCoroutine(FadeOut(logoFavoStudio));
        StartCoroutine(FadeOut(textoApresenta));

        yield return new WaitForSeconds(1f);

        // Mostra o logo do jogo.
        yield return StartCoroutine(FadeIn(LogodoJogo));

        yield return new WaitForSeconds(tempoVisivel + 1f);

        yield return StartCoroutine(FadeOut(LogodoJogo));

        // Carrega o menu depois que a apresentação termina.
        SceneManager.LoadScene("02_Menu");
    }

    IEnumerator FadeIn(CanvasGroup elemento)
    {
        // Aumenta gradualmente a visibilidade do elemento.
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * velocidadeFade;
            elemento.alpha = alpha;

            yield return null;
        }

        elemento.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup elemento)
    {
        // Diminui gradualmente a visibilidade do elemento.
        float alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * velocidadeFade;
            elemento.alpha = alpha;

            yield return null;
        }

        elemento.alpha = 0;
    }
}