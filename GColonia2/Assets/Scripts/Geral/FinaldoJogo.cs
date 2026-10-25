
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinaldoJogo : MonoBehaviour
{
    [Header("Texto")]

    // Texto que vai mostrar a mensagem de encerramento.
    public TMP_Text textoContinua;

    [Header("Tempos")]

    // Tempo que a tela permanece preta antes da mensagem aparecer.
    public float tempoTelaPreta = 2f;

    // Tempo de espera entre cada letra durante a digitação.
    public float velocidadeDigitacao = 0.15f;

    // Tempo que a mensagem completa permanece parada antes do zoom.
    public float tempoAntesZoom = 1f;

    // Duração da animação que aumenta o tamanho do texto.
    public float duracaoZoom = 3f;

    [Header("Zoom")]

    // Multiplicador do tamanho final do texto.
    public float escalaFinal = 15f;

    [Header("Cena")]

    // Nome da cena do menu que será carregada no final.
    public string nomeCenaMenu = "02_Menu";

    // Guarda o tamanho original do texto antes de começar a animação.
    private Vector3 escalaInicial;

    void Start()
    {
        // Salva o tamanho original para utilizar na animação de zoom.
        escalaInicial = textoContinua.transform.localScale;

        // Começa com o texto vazio para mostrar somente a tela preta.
        textoContinua.text = "";

        // Inicia a sequência completa de encerramento.
        StartCoroutine(AnimacaoFinal());
    }

    IEnumerator AnimacaoFinal()
    {
        // Mantém a tela preta durante o tempo configurado.
        yield return new WaitForSecondsRealtime(tempoTelaPreta);

        // Mensagem que será mostrada no encerramento do jogo.
        string mensagem = "CONTINUA...";

        // Percorre cada letra da mensagem para criar o efeito de digitação.
        foreach (char letra in mensagem)
        {
            // Acrescenta uma letra ao texto que já está aparecendo.
            textoContinua.text += letra;

            // Espera um pouco antes de mostrar a próxima letra.
            yield return new WaitForSecondsRealtime(velocidadeDigitacao);
        }

        // Mantém a mensagem completa na tela antes do zoom começar.
        yield return new WaitForSecondsRealtime(tempoAntesZoom);

        // Começa a contar o tempo da animação de zoom.
        float tempo = 0f;

        // Continua aumentando o texto até completar a duração configurada.
        while (tempo < duracaoZoom)
        {
            // Atualiza o tempo sem depender da velocidade geral do jogo.
            tempo += Time.unscaledDeltaTime;

            // Calcula quanto da animação já aconteceu.
            float progresso = Mathf.Clamp01(tempo / duracaoZoom);

            // Suaviza o começo e o final do crescimento do texto.
            progresso = Mathf.SmoothStep(0f, 1f, progresso);

            // Aumenta o texto progressivamente até a escala desejada.
            textoContinua.transform.localScale = Vector3.Lerp(escalaInicial, escalaInicial * escalaFinal, progresso);

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Carrega o menu principal depois da animação.
        SceneManager.LoadScene(nomeCenaMenu);
    }
}
