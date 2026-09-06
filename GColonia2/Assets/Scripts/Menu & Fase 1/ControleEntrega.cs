using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControleEntrega : MonoBehaviour
{
    // Permite que outros scripts encontrem o sistema de entrega que está ativo.
    public static ControleEntrega instancia;

    // Guarda a caixa de correio que o jogador está usando no momento.
    public CaixaCorreio caixaAtual;

    // Guarda as cores das caixas que já receberam suas cartas.
    private List<string> caixasConcluidas = new List<string>();

    [Header("Interface")]

    public GameObject iconeCartas;
    public GameObject setaVoltar;
    public TMP_Text textoTempo;

    [Header("Cronômetro")]

    // Tempo total disponível para completar as entregas.
    public float tempoInicial = 90f;

    [Header("Controle")]

    // Controlam o estado atual da gameplay.
    private float tempoRestante;
    private bool esperandoTeclaVoltar = false;
    private bool cronometroRodando = false;

    public GameObject limitevoltar;
    public string pontoEntrada = "Elevador";
    public GameObject menuCartas;
    public string corCaixaAtual;
    public bool entregaComecou = false;

    [Header("Painel de derrota")]

    public GameObject painelDerrota;
    public string cenaTentarNovamente;
    public string cenaMenu;

    private bool derrotaMostrada = false;

    [Header("Sons")]

    public AudioSource audioSource;
    public AudioClip somCartaCerta;
    public AudioClip somCartaErrada;
    public AudioClip somDerrotaImpacto;
    public AudioClip somDerrotaFinal;
    public AudioClip somVitoria;

    [Header("Vitória")]

    public PainelVitoria painelVitoria;

    private bool vitoriaMostrada = false;

    void Awake()
    {
        // Impede que dois sistemas de entrega existam ao mesmo tempo.
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;

        // Mantém o HUD, o cronômetro e o progresso ao trocar de cena.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Deixa os elementos escondidos até o momento em que forem necessários.
        painelDerrota.SetActive(false);
        iconeCartas.SetActive(false);
        setaVoltar.SetActive(false);
        textoTempo.gameObject.SetActive(false);
        limitevoltar.SetActive(false);
        menuCartas.SetActive(false);
        painelVitoria.gameObject.SetActive(false);
    }

    void Update()
    {
        // Espera o jogador apertar uma direção para iniciar a entrega.
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (esperandoTeclaVoltar && horizontal != 0)
        {
            IniciarCronometro();
        }

        // O restante do Update só acontece enquanto o cronômetro está rodando.
        if (!cronometroRodando)
        {
            return;
        }

        tempoRestante -= Time.deltaTime;

        // Mostra a derrota caso o tempo termine antes das quatro entregas.
        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            cronometroRodando = false;
            AtualizarTexto();

            if (caixasConcluidas.Count < 4)
            {
                MostrarDerrota();
            }

            return;
        }

        AtualizarTexto();
    }

    // Mostra as cartas e a seta depois que o jogador recebe as correspondências.
    public void MostrarCartasESeta()
    {
        iconeCartas.SetActive(true);
        setaVoltar.SetActive(true);
        limitevoltar.SetActive(true);

        esperandoTeclaVoltar = true;

        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }
    }

    void IniciarCronometro()
    {
        // Marca que a fase de entregas realmente começou.
        entregaComecou = true;
        esperandoTeclaVoltar = false;

        // A seta desaparece, mas as cartas continuam visíveis.
        setaVoltar.SetActive(false);
        iconeCartas.SetActive(true);

        // Mostra o tempo e inicia a contagem.
        textoTempo.gameObject.SetActive(true);
        tempoRestante = tempoInicial;
        cronometroRodando = true;

        AtualizarTexto();

        // Libera o jogador para começar a fase.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = true;
        }
    }

    void AtualizarTexto()
    {
        // Converte o tempo restante para o formato de minutos e segundos.
        int tempoInteiro = Mathf.CeilToInt(tempoRestante);
        int minutos = tempoInteiro / 60;
        int segundos = tempoInteiro % 60;

        textoTempo.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public void AbrirMenuCartas(string corDaCaixa, CaixaCorreio caixa)
    {
        // Guarda qual caixa está sendo usada antes de abrir o menu.
        corCaixaAtual = corDaCaixa;
        caixaAtual = caixa;

        menuCartas.SetActive(true);

        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }
    }

    public void MarcarCaixaConcluida(string cor)
    {
        // Adiciona a cor apenas se ela ainda não estiver na lista.
        if (!caixasConcluidas.Contains(cor))
        {
            caixasConcluidas.Add(cor);
        }
    }

    public bool CaixaJaConcluida(string cor)
    {
        // Retorna verdadeiro quando a caixa já recebeu sua carta.
        return caixasConcluidas.Contains(cor);
    }

    public void FecharMenuCartas()
    {
        menuCartas.SetActive(false);

        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = true;
        }
    }

    void MostrarDerrota()
    {
        // Impede que o painel de derrota apareça mais de uma vez.
        if (derrotaMostrada)
        {
            return;
        }

        derrotaMostrada = true;

        menuCartas.SetActive(false);
        painelDerrota.SetActive(true);

        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        StartCoroutine(TocarSonsDerrota());

        // Pausa o jogo enquanto o painel de derrota está aberto.
        Time.timeScale = 0f;
    }

    public void TentarNovamente()
    {
        // Volta o tempo ao normal e apaga o progresso da tentativa anterior.
        Time.timeScale = 1f;

        instancia = null;
        Destroy(gameObject);

        SceneManager.LoadScene(cenaTentarNovamente);
    }

    public void VoltarParaMenu()
    {
        // Volta o tempo ao normal antes de carregar o menu.
        Time.timeScale = 1f;

        instancia = null;
        Destroy(gameObject);

        SceneManager.LoadScene(cenaMenu);
    }

    public void TocarSomCartaCerta()
    {
        audioSource.PlayOneShot(somCartaCerta);
    }

    public void TocarSomCartaErrada()
    {
        audioSource.PlayOneShot(somCartaErrada);
    }

    IEnumerator TocarSonsDerrota()
    {
        // Toca o impacto e depois o som final da derrota.
        audioSource.PlayOneShot(somDerrotaImpacto);

        yield return new WaitForSecondsRealtime(0.45f);

        audioSource.PlayOneShot(somDerrotaFinal);
    }

    public void VerificarVitoria()
    {
        Debug.Log("Entregas concluídas: " + caixasConcluidas.Count);

        // A vitória acontece depois que as quatro caixas forem concluídas.
        if (caixasConcluidas.Count == 4)
        {
            MostrarVitoria();
        }
    }

    void MostrarVitoria()
    {
        // Impede que a animação de vitória seja iniciada mais de uma vez.
        if (vitoriaMostrada)
        {
            return;
        }

        vitoriaMostrada = true;
        cronometroRodando = false;

        // Esconde os elementos usados durante a gameplay.
        menuCartas.SetActive(false);
        iconeCartas.SetActive(false);
        textoTempo.gameObject.SetActive(false);

        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        audioSource.PlayOneShot(somVitoria);

        // Mostra e inicia a animação do painel de vitória.
        painelVitoria.gameObject.SetActive(true);
        painelVitoria.Iniciar();
    }
}