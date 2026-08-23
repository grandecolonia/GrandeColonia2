using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PainelVitoria : MonoBehaviour
{
    // Elementos da interface usados na animação de vitória.
    public RectTransform textoMissao;
    public CanvasGroup telaEscura;

    [Header("Animação")]

    // Configura a entrada do texto e o escurecimento da tela.
    public float distanciaInicial = 350f;
    public float duracaoSubida = 1f;
    public float tempoParado = 1.2f;
    public float duracaoEscurecer = 1f;

    [Header("Próxima cena")]

    public string cenaDestino;

    // Guarda a posição em que o texto deve terminar a animação.
    private Vector2 posicaoFinal;

    void Awake()
    {
        posicaoFinal = textoMissao.anchoredPosition;
    }

    public void Iniciar()
    {
        // Interrompe uma possível animação anterior e inicia a sequência de vitória.
        StopAllCoroutines();
        StartCoroutine(AnimarVitoria());
    }

    IEnumerator AnimarVitoria()
    {
        // Coloca o texto abaixo da posição final antes de começar a animação.
        Vector2 posicaoInicial = posicaoFinal + Vector2.down * distanciaInicial;

        textoMissao.anchoredPosition = posicaoInicial;
        telaEscura.alpha = 0f;

        float tempo = 0f;

        // Move o texto suavemente até sua posição final.
        while (tempo < duracaoSubida)
        {
            tempo += Time.unscaledDeltaTime;

            float progresso = Mathf.Clamp01(tempo / duracaoSubida);
            textoMissao.anchoredPosition = Vector2.Lerp(posicaoInicial, posicaoFinal, progresso);

            yield return null;
        }

        textoMissao.anchoredPosition = posicaoFinal;

        // Mantém a mensagem visível por alguns segundos.
        yield return new WaitForSecondsRealtime(tempoParado);

        tempo = 0f;

        // Escurece a tela gradualmente antes da troca de cena.
        while (tempo < duracaoEscurecer)
        {
            tempo += Time.unscaledDeltaTime;
            telaEscura.alpha = Mathf.Clamp01(tempo / duracaoEscurecer);

            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaDestino);
    }
}