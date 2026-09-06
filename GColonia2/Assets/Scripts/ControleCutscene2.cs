using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[System.Serializable]
public class EtapaCutscene
{
    // Texto e imagem que pertencem a esta etapa da cutscene.
    [TextArea(2, 5)] public string texto;
    public Sprite imagem;

    public AudioClip som;

    // Pequena pausa depois do som, caso seja necessária.
    public float esperaDepoisSom = 0f;
}

public class ControleCutscene2 : MonoBehaviour
{
    [Header("Áudio")]

    // AudioSource usado para reproduzir sons durante a cutscene.
    public AudioSource audioSource;

    [Header("Configuração")]

    // Controlam a velocidade da escrita e o tempo de permanência na tela.
    public float velocidadeTexto = 0.04f;
    public float tempoTexto = 3f;
    public float tempoImagem = 3f;

    [Header("Elementos da cutscene")]

    // Elementos usados para mostrar os textos e as imagens.
    public CanvasGroup texto;
    public CanvasGroup imagem;
    public TMP_Text textoInicial;
    public Image imagemInicial;

    [Header("Etapas")]

    // Guarda todas as partes da cutscene na ordem em que serão exibidas.
    public EtapaCutscene[] etapas;

    [Header("Input")]

    // Action configurada no arquivo de Input Actions.
    public InputActionReference avancarCutscene;

    [Header("Próxima cena")]

    public string cenaDestino;

    // Guarda quando o jogador pediu para avançar.
    private bool clicou = false;

    void OnEnable()
    {
        // Ativa a Action e escuta quando o comando for realizado.
        if (avancarCutscene != null)
        {
            avancarCutscene.action.performed += Avancar;
            avancarCutscene.action.Enable();
        }
    }

    void OnDisable()
    {
        // Remove o evento quando o objeto for desativado.
        if (avancarCutscene != null)
        {
            avancarCutscene.action.performed -= Avancar;
            avancarCutscene.action.Disable();
        }
    }

    IEnumerator Start()
    {
        // A cutscene começa com texto e imagem invisíveis.
        texto.alpha = 0f;
        imagem.alpha = 0f;

        // Percorre todas as etapas configuradas no Inspector.
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

        // Carrega a próxima cena depois que todas as etapas terminarem.
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaDestino);
    }

    void Avancar(InputAction.CallbackContext context)
    {
        Debug.Log("Avançou cutscene");
        // Registra que o comando configurado no Action Map foi realizado.[]
        clicou = true;
    }

    IEnumerator MostrarTexto(string fala)
    {
        // Prepara o novo texto antes de mostrá-lo.
        clicou = false;
        textoInicial.text = fala;
        textoInicial.maxVisibleCharacters = 0;

        yield return StartCoroutine(Fadein(texto));

        // Caso o jogador avance durante o Fade In, mostra o texto inteiro.
        if (clicou)
        {
            textoInicial.maxVisibleCharacters = textoInicial.text.Length;
            clicou = false;
        }
        else
        {
            yield return StartCoroutine(EscreverTexto());
        }

        // Depois da escrita, espera o tempo terminar ou outro comando.
        yield return StartCoroutine(EsperarAvancoOuTempo(tempoTexto));
        yield return StartCoroutine(FadeOut(texto));
    }

    IEnumerator MostrarImagem(Sprite novaImagem)
    {
        // Coloca a nova imagem antes de iniciar sua animação.
        clicou = false;
        imagemInicial.sprite = novaImagem;

        yield return StartCoroutine(Fadein(imagem));

        // A imagem permanece até o tempo terminar ou o jogador avançar.
        yield return StartCoroutine(EsperarAvancoOuTempo(tempoImagem));
        yield return StartCoroutine(FadeOut(imagem));
    }

    IEnumerator EsperarAvancoOuTempo(float tempoMaximo)
    {
        float tempo = 0f;

        // Continua enquanto o tempo não terminar e o jogador não avançar.
        while (tempo < tempoMaximo && !clicou)
        {
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // Consome o comando para ele não avançar duas partes ao mesmo tempo.
        clicou = false;
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
        // Faz o elemento aparecer gradualmente.
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
        // Mostra as letras do texto uma por uma.
        textoInicial.maxVisibleCharacters = 0;

        for (int i = 0; i <= textoInicial.text.Length; i++)
        {
            // Um comando durante a escrita mostra imediatamente a fala inteira.
            if (clicou)
            {
                textoInicial.maxVisibleCharacters = textoInicial.text.Length;
                clicou = false;
                yield break;
            }

            textoInicial.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(velocidadeTexto);
        }
    }

    IEnumerator FadeOut(CanvasGroup objeto)
    {
        // Faz o elemento desaparecer gradualmente.
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