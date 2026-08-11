using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [Header("Elementos da tela")]
    public CanvasGroup logoFavoStudio;
    public CanvasGroup textoApresenta;
    public CanvasGroup LogodoJogo;

    [Header("Configuração")]
    public float velocidadeFade = 1f;
    public float tempoVisivel = 1.5f;

    void Start()
    {
        logoFavoStudio.alpha = 0;
        textoApresenta.alpha = 0;
        LogodoJogo.alpha = 0;
        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {

        yield return StartCoroutine(FadeIn(logoFavoStudio));

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(FadeIn(textoApresenta));

        // Espera os dois ficarem na tela
        yield return new WaitForSeconds(tempoVisivel);

        StartCoroutine(FadeOut(logoFavoStudio));
        StartCoroutine(FadeOut(textoApresenta));

        // Espera o fade terminar
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeIn(LogodoJogo));

        yield return new WaitForSeconds(tempoVisivel + 1f);

        yield return StartCoroutine(FadeOut(LogodoJogo));

        SceneManager.LoadScene("02_Menu");
    }

    IEnumerator FadeIn(CanvasGroup elemento)
    {
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