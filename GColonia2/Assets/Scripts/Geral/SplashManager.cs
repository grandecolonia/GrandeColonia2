
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [Header("Elementos da tela")]

    // Logos que aparecem na tela de abertura do jogo, um de cada vez.
    public CanvasGroup logoFavoStudio, textoApresenta, LogodoJogo;

    [Header("Configuração")]

    // Controla a velocidade com que os elementos aparecem e desaparecem.
    public float velocidadeFade = 1f;

    // Tempo que os logos ficam visíveis antes de desaparecer.
    public float tempoVisivel = 1.5f;

    void Start()
    {
        // Começa com todos os elementos invisíveis para não mostrar nada antes da hora.
        logoFavoStudio.alpha = 0f;
        textoApresenta.alpha = 0f;
        LogodoJogo.alpha = 0f;

        // Inicia a sequência completa de apresentação.
        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {
        // Primeiro faz o logo do estúdio aparecer suavemente.
        yield return StartCoroutine(FadeIn(logoFavoStudio));

        // Dá uma pequena pausa antes de mostrar o texto.
        yield return new WaitForSeconds(0.2f);

        // Faz o texto aparecer enquanto o logo continua na tela.
        StartCoroutine(FadeIn(textoApresenta));

        // Mantém a apresentação aparecendo pelo tempo configurado.
        yield return new WaitForSeconds(tempoVisivel);

        // Começa a esconder o logo e o texto ao mesmo tempo.
        StartCoroutine(FadeOut(logoFavoStudio));
        StartCoroutine(FadeOut(textoApresenta));

        // Espera a apresentação anterior sair antes de mostrar o logo do jogo.
        yield return new WaitForSeconds(1f);

        // Faz o logo do jogo aparecer suavemente.
        yield return StartCoroutine(FadeIn(LogodoJogo));

        // Deixa o logo do jogo aparecendo um pouco mais.
        yield return new WaitForSeconds(tempoVisivel + 1f);

        // Esconde o logo do jogo antes de mudar de cena.
        yield return StartCoroutine(FadeOut(LogodoJogo));

        // Terminou a apresentação, então carrega o menu principal.
        SceneManager.LoadScene("02_Menu");
    }

    IEnumerator FadeIn(CanvasGroup elemento)
    {
        // Começa com o elemento completamente invisível.
        float alpha = 0f;

        // Vai aumentando a opacidade até o elemento aparecer por completo.
        while (alpha < 1f)
        {
            // Calcula o quanto a imagem deve aparecer neste frame.
            alpha += Time.deltaTime * velocidadeFade;

            // Aplica a nova transparência ao elemento.
            elemento.alpha = alpha;

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Garante que o elemento termine totalmente visível.
        elemento.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup elemento)
    {
        // Começa com o elemento completamente visível.
        float alpha = 1f;

        // Vai diminuindo a opacidade até o elemento desaparecer.
        while (alpha > 0f)
        {
            // Calcula o quanto a imagem deve desaparecer neste frame.
            alpha -= Time.deltaTime * velocidadeFade;

            // Aplica a nova transparência ao elemento.
            elemento.alpha = alpha;

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Garante que o elemento termine completamente invisível.
        elemento.alpha = 0f;
    }
}
