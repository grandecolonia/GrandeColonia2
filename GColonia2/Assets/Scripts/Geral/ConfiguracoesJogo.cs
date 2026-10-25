
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracoesJogo : MonoBehaviour
{
    // Guarda a instância principal para outros scripts conseguirem acessar as configurações.
    public static ConfiguracoesJogo instancia;

    [Header("Menu de opções")]

    // Elementos que aparecem ou somem quando o jogador abre as opções.
    public GameObject menuinicial;
    public GameObject seta;
    public GameObject fundoOpcoes;

    [Header("Som")]

    // Slider usado para controlar o volume geral do jogo.
    public Slider sliderVolume;

    [Header("Cursores")]

    // Guarda todos os cursores que o jogador pode escolher.
    public Texture2D[] cursores;

    // Define o ponto exato do cursor que vai registrar o clique.
    public Vector2 pontoClique = Vector2.zero;

    // Guarda a velocidade anterior do jogo para restaurar quando fechar as opções.
    private float tempoAnterior = 1f;

    [Header("Tamanho dos textos")]

    // Tamanhos disponíveis para os textos do jogo.
    public float tamanhoPequeno = 30f;
    public float tamanhoNormal = 40f;
    public float tamanhoGrande = 50f;

    void Awake()
    {
        // Se já existe outro sistema de configurações, destrói este para não duplicar.
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Define este objeto como o sistema principal de configurações.
        instancia = this;

        // Mantém as configurações vivas mesmo quando o jogador troca de cena.
        DontDestroyOnLoad(gameObject);

        // Recupera o volume e o cursor que o jogador escolheu anteriormente.
        float volumeSalvo = PlayerPrefs.GetFloat("VolumeGeral", 1f);
        int cursorSalvo = PlayerPrefs.GetInt("CursorSelecionado", 0);

        // Aplica o volume salvo em todos os sons do jogo.
        AudioListener.volume = volumeSalvo;

        // Atualiza o slider sem disparar o evento de mudança de volume.
        if (sliderVolume != null)
        {
            sliderVolume.SetValueWithoutNotify(volumeSalvo);
        }

        // Começa com o painel de opções fechado.
        if (fundoOpcoes != null)
        {
            fundoOpcoes.SetActive(false);
        }

        // Deixa a seta inicial aparecendo.
        if (seta != null)
        {
            seta.SetActive(true);
        }

        // Coloca o cursor que estava selecionado na última vez.
        AplicarCursor(cursorSalvo);
    }

    public void AbrirOpcoes()
    {
        // Se o painel não existe ou já está aberto, não faz nada.
        if (fundoOpcoes == null || fundoOpcoes.activeSelf)
        {
            return;
        }

        // Guarda a velocidade atual e pausa o jogo enquanto as opções estão abertas.
        tempoAnterior = Time.timeScale;
        Time.timeScale = 0f;

        // Esconde o menu inicial enquanto mostra as configurações.
        if (menuinicial != null)
        {
            menuinicial.SetActive(false);
        }

        // Esconde a seta e abre o painel de opções.
        seta.SetActive(false);
        fundoOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        // Fecha o painel e faz a seta aparecer novamente.
        fundoOpcoes.SetActive(false);
        seta.SetActive(true);

        // Se o menu inicial existir nesta cena, mostra ele novamente.
        if (menuinicial != null)
        {
            menuinicial.SetActive(true);
        }

        // Devolve ao jogo a velocidade que ele tinha antes de abrir as opções.
        Time.timeScale = tempoAnterior;

        // Garante que as preferências alteradas sejam salvas.
        PlayerPrefs.Save();
    }

    public void ControlarSom(float value)
    {
        // Limita o volume entre 0 e 1 para não aceitar valores fora do permitido.
        float volumeLimitado = Mathf.Clamp01(value);

        // Aplica o volume escolhido e guarda para as próximas vezes.
        AudioListener.volume = volumeLimitado;
        PlayerPrefs.SetFloat("VolumeGeral", volumeLimitado);
    }

    public void EscolherTamanhoTexto(int opcao)
    {
        // Guarda a opção escolhida, impedindo valores menores que 0 ou maiores que 2.
        PlayerPrefs.SetInt("TamanhoTexto", Mathf.Clamp(opcao, 0, 2));

        // Salva a escolha para continuar valendo depois que o jogo fechar.
        PlayerPrefs.Save();
    }

    public static float ObterTamanhoTexto()
    {
        // Recupera a opção salva, usando o tamanho normal como padrão.
        int opcao = PlayerPrefs.GetInt("TamanhoTexto", 1);

        // Se não existe uma instância ativa, usa o tamanho padrão de 40.
        if (instancia == null)
        {
            return 40f;
        }

        // Se escolheu a opção 0, devolve o tamanho pequeno.
        if (opcao == 0)
        {
            return instancia.tamanhoPequeno;
        }

        // Se escolheu a opção 2, devolve o tamanho grande.
        if (opcao == 2)
        {
            return instancia.tamanhoGrande;
        }

        // Qualquer outra opção válida usa o tamanho normal.
        return instancia.tamanhoNormal;
    }

    public void EscolherCursor(int indice)
    {
        // Se o cursor escolhido não existe na lista, cancela a troca.
        if (cursores == null || indice < 0 || indice >= cursores.Length)
        {
            return;
        }

        // Aplica o cursor escolhido na tela.
        AplicarCursor(indice);

        // Guarda qual cursor foi escolhido para usar novamente depois.
        PlayerPrefs.SetInt("CursorSelecionado", indice);
        PlayerPrefs.Save();
    }

    void AplicarCursor(int indice)
    {
        // Se não tem nenhum cursor cadastrado, não tem o que aplicar.
        if (cursores == null || cursores.Length == 0)
        {
            return;
        }

        // Se o índice salvo não existe mais, usa o primeiro cursor da lista.
        if (indice < 0 || indice >= cursores.Length)
        {
            indice = 0;
        }

        // Troca o cursor do mouse pela imagem escolhida.
        Cursor.SetCursor(cursores[indice], pontoClique, CursorMode.Auto);

        // Garante que o cursor fique visível e livre para se movimentar.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
