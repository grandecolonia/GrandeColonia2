using UnityEngine;
using UnityEngine.UI;

public class ConfiguracoesJogo : MonoBehaviour
{
    public static ConfiguracoesJogo instancia;

    [Header("Menu de opções")]

    // Elementos usados para abrir e fechar o painel de opções.
    public GameObject menuinicial;
    public GameObject seta;
    public GameObject fundoOpcoes;

    [Header("Som")]

    // Slider responsável pelo volume geral do jogo.
    public Slider sliderVolume;

    [Header("Cursores")]

    // Guarda todas as opções de cursor disponíveis.
    public Texture2D[] cursores;
    public Vector2 pontoClique = Vector2.zero;

    // Guarda a velocidade do jogo antes de abrir as opções.
    private float tempoAnterior = 1f;

    void Awake()
    {
        // Impede que existam dois menus de opções ao mesmo tempo.
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;

        // Mantém as opções durante as trocas de cena.
        DontDestroyOnLoad(gameObject);

        // Recupera o volume e o cursor escolhidos anteriormente.
        float volumeSalvo = PlayerPrefs.GetFloat("VolumeGeral", 1f);
        int cursorSalvo = PlayerPrefs.GetInt("CursorSelecionado", 0);

        AudioListener.volume = volumeSalvo;

        if (sliderVolume != null)
        {
            sliderVolume.SetValueWithoutNotify(volumeSalvo);
        }

        if (fundoOpcoes != null)
        {
            fundoOpcoes.SetActive(false);
        }

        if (seta != null)
        {
            seta.SetActive(true);
        }

        AplicarCursor(cursorSalvo);
    }

    public void AbrirOpcoes()
    {
        // Impede abrir novamente caso o painel já esteja aparecendo.
        if (fundoOpcoes == null || fundoOpcoes.activeSelf)
        {
            return;
        }

        tempoAnterior = Time.timeScale;
        Time.timeScale = 0f;

        if (menuinicial != null)
        {
            menuinicial.SetActive(false);
        }

        seta.SetActive(false);
        fundoOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        fundoOpcoes.SetActive(false);
        seta.SetActive(true);

        // O menu inicial só existe na cena principal.
        if (menuinicial != null)
        {
            menuinicial.SetActive(true);
        }

        Time.timeScale = tempoAnterior;
        PlayerPrefs.Save();
    }

    public void ControlarSom(float value)
    {
        // Controla o volume geral do jogo e salva a escolha.
        float volumeLimitado = Mathf.Clamp01(value);

        AudioListener.volume = volumeLimitado;
        PlayerPrefs.SetFloat("VolumeGeral", volumeLimitado);
    }

    public void EscolherCursor(int indice)
    {
        // Impede selecionar uma posição que não existe.
        if (cursores == null || indice < 0 || indice >= cursores.Length)
        {
            return;
        }

        AplicarCursor(indice);

        PlayerPrefs.SetInt("CursorSelecionado", indice);
        PlayerPrefs.Save();
    }

    void AplicarCursor(int indice)
    {
        if (cursores == null || cursores.Length == 0)
        {
            return;
        }

        if (indice < 0 || indice >= cursores.Length)
        {
            indice = 0;
        }

        Cursor.SetCursor(cursores[indice], pontoClique, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}