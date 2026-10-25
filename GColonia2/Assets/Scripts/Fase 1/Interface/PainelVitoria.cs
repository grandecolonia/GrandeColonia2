
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PainelVitoria : MonoBehaviour
{
    [Header("Interface")]

    // Texto da missão que vai subir na tela quando o jogador vencer.
    public RectTransform textoMissao;

    // Tela escura que aparece antes de carregar a próxima cena.
    public CanvasGroup telaEscura;

    [Header("Animação")]

    // Distância que o texto vai começar abaixo da posição original.
    public float distanciaInicial = 350f;

    // Tempo que o texto vai levar para subir até o lugar certo.
    public float duracaoSubida = 1f;

    // Tempo que a mensagem vai ficar parada na tela.
    public float tempoParado = 1.2f;

    // Tempo que a tela vai levar para escurecer completamente.
    public float duracaoEscurecer = 1f;

    [Header("Próxima cena")]

    // Cena que será carregada depois que a animação terminar.
    public string cenaDestino;

    // Guarda a posição original para o texto saber onde precisa parar.
    private Vector2 posicaoFinal;

    void Awake()
    {
        // Salva a posição final do texto antes de começar qualquer animação.
        posicaoFinal = textoMissao.anchoredPosition;
    }

    public void Iniciar()
    {
        // Para qualquer animação anterior para não ter duas rodando ao mesmo tempo.
        StopAllCoroutines();

        // Começa a sequência de vitória.
        StartCoroutine(AnimarVitoria());
    }

    IEnumerator AnimarVitoria()
    {
        // Calcula uma posição abaixo de onde o texto deve terminar.
        Vector2 posicaoInicial = posicaoFinal + Vector2.down * distanciaInicial;

        // Coloca o texto na posição inicial e deixa a tela escura invisível.
        textoMissao.anchoredPosition = posicaoInicial;
        telaEscura.alpha = 0f;

        // Começa a contar o tempo da subida.
        float tempo = 0f;

        // Continua subindo até completar o tempo definido.
        while (tempo < duracaoSubida)
        {
            // Conta o tempo mesmo se o jogo estiver pausado.
            tempo += Time.unscaledDeltaTime;

            // Calcula o progresso da subida, sem passar de 0 a 1.
            float progresso = Mathf.Clamp01(tempo / duracaoSubida);

            // Move o texto suavemente até a posição final.
            textoMissao.anchoredPosition = Vector2.Lerp(posicaoInicial, posicaoFinal, progresso);

            // Espera o próximo frame antes de continuar a animação.
            yield return null;
        }

        // Garante que o texto termine exatamente no lugar certo.
        textoMissao.anchoredPosition = posicaoFinal;

        // Deixa a mensagem aparecendo por alguns segundos antes de escurecer.
        yield return new WaitForSecondsRealtime(tempoParado);

        // Zera a contagem para começar o escurecimento da tela.
        tempo = 0f;

        // Vai escurecendo a tela aos poucos.
        while (tempo < duracaoEscurecer)
        {
            // Continua contando mesmo se o jogo estiver pausado.
            tempo += Time.unscaledDeltaTime;

            // Aumenta a opacidade da tela escura até ficar completamente preta.
            telaEscura.alpha = Mathf.Clamp01(tempo / duracaoEscurecer);

            // Espera o próximo frame para continuar o fade.
            yield return null;
        }

        // Devolve o tempo ao normal antes de sair da fase.
        Time.timeScale = 1f;

        // Carrega a cena configurada depois da vitória.
        SceneManager.LoadScene(cenaDestino);
    }
}
