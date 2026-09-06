using UnityEngine;
using System.Collections;

public class AvisoObjetivo : MonoBehaviour
{
    [Header("Configuração")]

    // Tempo que o aviso permanece visível.
    public float tempoNaTela = 3f;

    // Controla a velocidade da entrada e da saída.
    public float velocidadeEntrada = 6f;
    public float velocidadeSaida = 8f;

    // Distância que o aviso começa fora da tela.
    public float distanciaFora = 450f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 posicaoFinal;
    private Vector2 posicaoInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        posicaoFinal = rectTransform.anchoredPosition;
        posicaoInicial = posicaoFinal + Vector2.left * distanciaFora;

        rectTransform.anchoredPosition = posicaoInicial;
        canvasGroup.alpha = 0f;

        StartCoroutine(MostrarAviso());
    }

    IEnumerator MostrarAviso()
    {
        // Faz o aviso entrar rapidamente pela esquerda.
        while (Vector2.Distance(rectTransform.anchoredPosition, posicaoFinal) > 1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicaoFinal, velocidadeEntrada * Time.unscaledDeltaTime);
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.unscaledDeltaTime);
            yield return null;
        }

        rectTransform.anchoredPosition = posicaoFinal;
        canvasGroup.alpha = 1f;

        // Mantém o aviso parado por alguns segundos.
        yield return new WaitForSecondsRealtime(tempoNaTela);

        // Faz o aviso sair rapidamente.
        while (Vector2.Distance(rectTransform.anchoredPosition, posicaoInicial) > 1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicaoInicial, velocidadeSaida * Time.unscaledDeltaTime);
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, 6f * Time.unscaledDeltaTime);
            yield return null;
        }

        rectTransform.anchoredPosition = posicaoInicial;
        canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }
}