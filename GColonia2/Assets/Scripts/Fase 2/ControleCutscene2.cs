using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[System.Serializable]
public class EtapaCutscene
{
    // Texto e imagem usados nesta etapa da cutscene.
    [TextArea(2, 5)]
    public string texto;

    public Sprite imagem;
    public AudioClip som;

    // Pequena pausa depois do som, caso precise.
    public float esperaDepoisSom = 0f;
}

public class ControleCutscene2 : MonoBehaviour
{
    [Header("Áudio")]

    // AudioSource usado para tocar os sons da cutscene.
    public AudioSource audioSource;

    [Header("Configuração")]

    // Controlam a velocidade do texto e o tempo de cada parte na tela.
    public float velocidadeTexto = 0.04f;
    public float tempoTexto = 3f;
    public float tempoImagem = 3f;

    [Header("Elementos da cutscene")]

    // Elementos usados para mostrar textos e imagens.
    public CanvasGroup texto;
    public CanvasGroup imagem;
    public TMP_Text textoInicial;
    public Image imagemInicial;

    [Header("Etapas")]

    // Todas as partes da cutscene na ordem em que vão aparecer.
    public EtapaCutscene[] etapas;

    [Header("Input")]

    // Action usada para avançar a cutscene.
    public InputActionReference avancarCutscene;

    [Header("Próxima cena")]

    public string cenaDestino;

    // Guarda quando o jogador pediu para avançar.
    private bool clicou = false;

    void OnEnable()
    {
        // Ativa a Action e começa a escutar o comando.
        if (avancarCutscene != null)
        {
            avancarCutscene.action.performed += Avancar;
            avancarCutscene.action.Enable();
        }
    }

    void OnDisable()
    {
        // Para de escutar o comando quando o objeto for desativado.
        if (avancarCutscene != null)
        {
            avancarCutscene.action.performed -= Avancar;
            avancarCutscene.action.Disable();
        }
    }

    IEnumerator Start()
    {
        // A cutscene começa com texto e imagem escondidos.
        texto.alpha = 0f;
        imagem.alpha = 0f;

        // Passa por todas as etapas configuradas no Inspector.
        foreach (EtapaCutscene etapa in etapas)
        {
            if (etapa.som != null)
            {
                audioSource.PlayOneShot(etapa.som);

                if (etapa.esperaDepoisSom > 0f)
                {
                    yield return new WaitForSecondsRealtime(etapa.esperaDepoisSom);
                }
            }

            if (!string.IsNullOrWhiteSpace(etapa.texto))
            {
                yield return StartCoroutine(MostrarTexto(etapa.texto));
            }

            if (etapa.imagem != null)
            {
                yield return StartCoroutine(MostrarImagem(etapa.imagem));
            }
        }

        // Vai para a próxima cena quando todas as etapas terminarem.
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaDestino);
    }

    void Avancar(InputAction.CallbackContext context)
    {
        Debug.Log("Avançou cutscene");

        // Registra que o jogador apertou o comando de avançar.
        clicou = true;
    }



IEnumerator MostrarTexto(string fala)
{
    clicou = false;
    textoInicial.text = "";

    yield return StartCoroutine(Fadein(texto));

    bool terminou = false;

    while (!terminou)
    {
        // Recalcula as páginas com o tamanho atual.
        float tamanhoAtual = textoInicial.fontSize;
        List<string> paginas = DividirTexto(fala);

        bool mudouTamanho = false;

        foreach (string pagina in paginas)
        {
            clicou = false;
            textoInicial.text = pagina;
            textoInicial.ForceMeshUpdate();

            int totalCaracteres = textoInicial.textInfo.characterCount;
            textoInicial.maxVisibleCharacters = 0;

            // Escreve o texto letra por letra.
            for (int i = 1; i <= totalCaracteres; i++)
            {
                if (textoInicial.fontSize != tamanhoAtual)
                {
                    mudouTamanho = true;
                    break;
                }

                if (clicou)
                {
                    textoInicial.maxVisibleCharacters = totalCaracteres;
                    clicou = false;
                    break;
                }

                textoInicial.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(velocidadeTexto);
            }

            if (mudouTamanho)
                break;

            // Espera o tempo acabar ou o jogador avançar.
            float tempo = 0f;
            clicou = false;

            while (tempo < tempoTexto && !clicou)
            {
                if (textoInicial.fontSize != tamanhoAtual)
                {
                    mudouTamanho = true;
                    break;
                }

                tempo += Time.unscaledDeltaTime;
                yield return null;
            }

            if (mudouTamanho)
                break;

            clicou = false;
        }

        if (!mudouTamanho)
            terminou = true;
        else
            clicou = false;
    }

    yield return StartCoroutine(FadeOut(texto));
}



List<string> DividirTexto(string fala)
{
    List<string> paginas = new List<string>();

    string[] palavras = fala.Split(new char[] { ' ', '\n', '\r', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
    string paginaAtual = "";

    float largura = textoInicial.rectTransform.rect.width - 30f;
    float altura = textoInicial.rectTransform.rect.height - 30f;

    foreach (string palavra in palavras)
    {
        string teste = string.IsNullOrEmpty(paginaAtual) ? palavra : paginaAtual + " " + palavra;

        Vector2 tamanho = textoInicial.GetPreferredValues(teste, largura, Mathf.Infinity);

        if (tamanho.y > altura && !string.IsNullOrEmpty(paginaAtual))
        {
            paginas.Add(paginaAtual);
            paginaAtual = palavra;
        }
        else
        {
            paginaAtual = teste;
        }
    }

    if (!string.IsNullOrEmpty(paginaAtual))
    {
        paginas.Add(paginaAtual);
    }

    return paginas;
}



    IEnumerator MostrarImagem(Sprite novaImagem)
    {
        // Troca a imagem antes de mostrar.
        clicou = false;
        imagemInicial.sprite = novaImagem;

        yield return StartCoroutine(Fadein(imagem));

        // Mantém a imagem até o tempo acabar ou o jogador avançar.
        yield return StartCoroutine(EsperarAvancoOuTempo(tempoImagem));
        yield return StartCoroutine(FadeOut(imagem));
    }

    IEnumerator EsperarAvancoOuTempo(float tempoMaximo)
    {
        float tempo = 0f;

        // Continua enquanto o tempo não acabar e ninguém avançar.
        while (tempo < tempoMaximo && !clicou)
        {
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // Reseta o comando para não pular duas etapas de uma vez.
        clicou = false;
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
        // Faz o elemento aparecer aos poucos.
        float tempo = 0f;

        while (tempo < 1f)
        {
            tempo += Time.unscaledDeltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        objeto.alpha = 1f;
    }

    
IEnumerator EscreverTexto()
{
    textoInicial.ForceMeshUpdate();
    int totalCaracteres = textoInicial.textInfo.characterCount;
    textoInicial.maxVisibleCharacters = 0;

    for (int i = 1; i <= totalCaracteres; i++)
    {
        if (clicou)
        {
            textoInicial.maxVisibleCharacters = totalCaracteres;
            clicou = false;
            yield break;
        }

        textoInicial.maxVisibleCharacters = i;
        yield return new WaitForSecondsRealtime(velocidadeTexto);
    }
}


    IEnumerator FadeOut(CanvasGroup objeto)
    {
        // Faz o elemento desaparecer aos poucos.
        float tempo = 1f;

        while (tempo > 0f)
        {
            tempo -= Time.unscaledDeltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        objeto.alpha = 0f;
    }
}