
using UnityEngine;
using System.Collections;

public class AvisoObjetivo : MonoBehaviour
{
    [Header("Configuração")]

    // Tempo que o aviso vai ficar parado na tela antes de desaparecer.
    public float tempoNaTela = 3f;

    // Velocidade com que o aviso entra pela esquerda.
    public float velocidadeEntrada = 6f;

    // Velocidade com que o aviso sai da tela.
    public float velocidadeSaida = 8f;

    // Distância que o aviso fica fora da tela antes de começar a animação.
    public float distanciaFora = 450f;

    // Guarda o RectTransform para conseguir movimentar o painel.
    private RectTransform rectTransform;

    // Controla a transparência do aviso durante a entrada e saída.
    private CanvasGroup canvasGroup;

    // Posição onde o aviso deve ficar enquanto estiver aparecendo.
    private Vector2 posicaoFinal;

    // Posição onde o aviso começa e termina, fora da tela.
    private Vector2 posicaoInicial;

    void Start()
    {
        // Pega os componentes usados para movimentar e esconder o aviso.
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Guarda a posição original do painel, onde ele deve ficar visível.
        posicaoFinal = rectTransform.anchoredPosition;

        // Calcula uma posição à esquerda para começar com o aviso fora da tela.
        posicaoInicial = posicaoFinal + Vector2.left * distanciaFora;

        // Coloca o aviso fora da tela e deixa ele completamente invisível.
        rectTransform.anchoredPosition = posicaoInicial;
        canvasGroup.alpha = 0f;

        // Começa a animação completa de entrada, espera e saída.
        StartCoroutine(MostrarAviso());
    }

    IEnumerator MostrarAviso()
    {
        // Continua movendo o aviso até ele chegar perto da posição final.
        while (Vector2.Distance(rectTransform.anchoredPosition, posicaoFinal) > 1f)
        {
            // Aproxima o painel suavemente da posição onde ele deve aparecer.
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicaoFinal, velocidadeEntrada * Time.unscaledDeltaTime);

            // Vai deixando o aviso visível enquanto ele entra na tela.
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.unscaledDeltaTime);

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Garante que o aviso termine exatamente na posição correta.
        rectTransform.anchoredPosition = posicaoFinal;

        // Deixa o painel completamente visível.
        canvasGroup.alpha = 1f;

        // Mantém o aviso na tela pelo tempo configurado, mesmo se o jogo estiver pausado.
        yield return new WaitForSecondsRealtime(tempoNaTela);

        // Continua movendo o aviso até ele voltar para fora da tela.
        while (Vector2.Distance(rectTransform.anchoredPosition, posicaoInicial) > 1f)
        {
            // Move o painel suavemente para a esquerda.
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicaoInicial, velocidadeSaida * Time.unscaledDeltaTime);

            // Vai deixando o aviso invisível enquanto ele sai.
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, 6f * Time.unscaledDeltaTime);

            // Espera o próximo frame para continuar a saída.
            yield return null;
        }

        // Devolve o painel exatamente para a posição fora da tela.
        rectTransform.anchoredPosition = posicaoInicial;

        // Garante que o aviso termine completamente invisível.
        canvasGroup.alpha = 0f;

        // Desativa o objeto depois que toda a animação termina.
        gameObject.SetActive(false);
    }
}
