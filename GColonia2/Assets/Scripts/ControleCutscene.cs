using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControleCutscene : MonoBehaviour
{
    // Controla a velocidade em que as letras aparecem na tela.
    public float velocidadeTexto = 0.04f;

    // Tempo máximo que cada imagem permanece na tela.
    public float tempoImagem = 3f;

    // Controlam a transparência do texto e das imagens.
    public CanvasGroup texto;
    public CanvasGroup imagem;

    // Elementos usados para mostrar o texto e as imagens da cutscene.
    public TMP_Text textoInicial;
    public Image imagemInicial;

    // Imagens exibidas durante cada parte da cutscene.
    public Sprite imagem1;
    public Sprite imagem2;
    public Sprite imagem3;
    public Sprite imagem4;
    public Sprite imagem5;
    public Sprite imagem6;
    public Sprite imagem7;
    public Sprite imagem8;
    public Sprite imagem9;
    public Sprite imagem10;

    [Header("Próxima cena")]

    public string cenaDestino;

    // Guarda se o jogador clicou com o botão esquerdo do mouse.
    private bool clicou = false;

    IEnumerator Start()
    {
        // Começa com o texto e a imagem completamente invisíveis.
        texto.alpha = 0;
        imagem.alpha = 0;

        // CUTSCENE 1
        yield return StartCoroutine(MostrarTexto(
            "É de conhecimento geral que as abelhas vivem em uma sociedade altamente organizada."
        ));

        yield return StartCoroutine(MostrarImagem(imagem1));

        // CUTSCENE 2
        yield return StartCoroutine(MostrarTexto(
            "Cada uma cumpre seu papel na colmeia e se orgulha de fazer parte dela."
        ));

        yield return StartCoroutine(MostrarImagem(imagem2));

        // CUTSCENE 3
        yield return StartCoroutine(MostrarTexto(
            "Tudo segue seu ciclo. A cada quatro anos, uma rainha deixa seu posto, dando lugar a uma nova soberana."
        ));

        yield return StartCoroutine(MostrarImagem(imagem3));

        // CUTSCENE 4
        yield return StartCoroutine(MostrarTexto(
            "No entanto, o que aconteceria se esse ciclo fosse interrompido?"
        ));

        yield return StartCoroutine(MostrarImagem(imagem4));

        // CUTSCENE 5
        yield return StartCoroutine(MostrarTexto(
            "A atual rainha, já velha, faleceu antes que uma nova pudesse assumir seu lugar."
        ));

        yield return StartCoroutine(MostrarImagem(imagem5));

        // CUTSCENE 6
        yield return StartCoroutine(MostrarTexto(
            "Sem uma regente, o caos tomou conta da colmeia..."
        ));

        yield return StartCoroutine(MostrarImagem(imagem6));

        // CUTSCENE 7
        yield return StartCoroutine(MostrarTexto(
            "Para apaziguar a situação, os generais da realeza decidiram assumir o controle enquanto a jovem princesa não estivesse pronta para governar."
        ));

        yield return StartCoroutine(MostrarImagem(imagem7));

        // CUTSCENE 8
        yield return StartCoroutine(MostrarTexto(
            "Mas o poder subiu à cabeça. Os generais se tornaram ditadores."
        ));

        yield return StartCoroutine(MostrarImagem(imagem8));

        // CUTSCENE 9
        yield return StartCoroutine(MostrarImagem(imagem9));

        // CUTSCENE 10
        yield return StartCoroutine(MostrarImagem(imagem10));

        SceneManager.LoadScene(cenaDestino);
    }

    void Update()
    {
        // Detecta um clique com o botão esquerdo do mouse.
        if (Input.GetMouseButtonDown(0))
        {
            clicou = true;
        }
    }

    IEnumerator MostrarTexto(string fala)
    {
        // Prepara o novo texto antes de mostrá-lo.
        clicou = false;
        textoInicial.text = fala;
        textoInicial.maxVisibleCharacters = 0;

        yield return StartCoroutine(Fadein(texto));

        // Caso o jogador tenha clicado durante o Fade In, mostra o texto inteiro.
        if (clicou)
        {
            textoInicial.maxVisibleCharacters = textoInicial.text.Length;
            clicou = false;
        }
        else
        {
            yield return StartCoroutine(EscreverTexto());
        }

        // Espera outro clique para passar para a imagem.
        yield return StartCoroutine(EsperarClique());
        yield return StartCoroutine(FadeOut(texto));
    }

IEnumerator MostrarImagem(Sprite novaImagem)
{
    // Troca a imagem e inicia seu aparecimento.
    clicou = false;
    imagemInicial.sprite = novaImagem;

    yield return StartCoroutine(Fadein(imagem));

    // Mantém a imagem na tela até o tempo terminar ou o jogador clicar.
    float tempo = 0f;

    while (tempo < tempoImagem && !clicou)
    {
        tempo += Time.unscaledDeltaTime;
        yield return null;
    }

    // Consome o clique antes de continuar para a próxima parte.
    clicou = false;

    yield return StartCoroutine(FadeOut(imagem));
}
    IEnumerator EsperarClique()
    {
        // Mantém a cutscene parada até o jogador clicar.
        while (!clicou)
        {
            yield return null;
        }

        // Consome o clique para ele não avançar duas partes de uma vez.
        clicou = false;
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
        // Aumenta a transparência aos poucos até o objeto ficar visível.
        float tempo = 0;

        while (tempo < 1)
        {
            tempo += Time.deltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        objeto.alpha = 1;
    }

    IEnumerator EscreverTexto()
    {
        // Mostra as letras do texto uma por uma.
        textoInicial.maxVisibleCharacters = 0;

        for (int i = 0; i <= textoInicial.text.Length; i++)
        {
            // Um clique durante a escrita mostra imediatamente todo o texto.
            if (clicou)
            {
                textoInicial.maxVisibleCharacters = textoInicial.text.Length;
                clicou = false;
                yield break;
            }

            textoInicial.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadeTexto);
        }
    }

    IEnumerator FadeOut(CanvasGroup objeto)
    {
        // Diminui a transparência aos poucos até o objeto desaparecer.
        float tempo = 1;

        while (tempo > 0)
        {
            tempo -= Time.deltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        objeto.alpha = 0;
    }
}