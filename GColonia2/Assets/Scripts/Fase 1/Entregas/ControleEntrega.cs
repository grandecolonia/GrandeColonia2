
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ControleEntrega : MonoBehaviour
{
    // Guarda a instância principal para outros scripts acessarem o sistema de entregas.
    public static ControleEntrega instancia;

    // Guarda a caixa de correio com a qual o jogador está interagindo.
    public CaixaCorreio caixaAtual;

    // Lista das cores que já receberam suas cartas corretamente.
    private List<string> caixasConcluidas = new List<string>();

    [Header("Interface")]

    // Elementos que aparecem durante a fase de entregas.
    public GameObject iconeCartas;
    public GameObject setaVoltar;
    public TMP_Text textoTempo;

    [Header("Cronômetro")]

    // Tempo total que o jogador tem para entregar todas as cartas.
    public float tempoInicial = 90f;

    [Header("Controle")]

    // Guarda quanto tempo ainda falta para terminar a fase.
    private float tempoRestante;

    // Indica que o jogador precisa apertar uma direção para começar.
    private bool esperandoTeclaVoltar = false;

    // Controla se o cronômetro está contando ou não.
    private bool cronometroRodando = false;

    // Limite que será liberado quando as entregas começarem.
    public GameObject limitevoltar;

    // Guarda de onde o jogador veio para posicioná-lo corretamente entre as cenas.
    public string pontoEntrada = "Elevador";

    // Menu onde o jogador escolhe qual carta vai entregar.
    public GameObject menuCartas;

    // Guarda a cor da caixa que está sendo utilizada.
    public string corCaixaAtual;

    // Informa aos outros scripts se a fase de entregas já começou.
    public bool entregaComecou = false;

    [Header("Input")]

    // Usa a ação de movimento do jogador para identificar quando ele quer começar.
    public InputActionReference mover;

    [Header("Painel de derrota")]

    // Painel que aparece quando o jogador fica sem tempo.
    public GameObject painelDerrota;

    // Cenas usadas para tentar novamente ou voltar ao menu.
    public string cenaTentarNovamente;
    public string cenaMenu;

    // Impede que a derrota seja ativada várias vezes.
    private bool derrotaMostrada = false;

    [Header("Sons")]

    // Fonte responsável por reproduzir os efeitos sonoros da fase.
    public AudioSource audioSource;

    // Sons usados quando o jogador acerta, erra, perde ou vence.
    public AudioClip somCartaCerta, somCartaErrada, somDerrotaImpacto, somDerrotaFinal, somVitoria;

    [Header("Vitória")]

    // Painel que aparece quando todas as cartas são entregues.
    public GameObject painelVitoria;

    // Impede que a vitória seja ativada mais de uma vez.
    private bool vitoriaMostrada = false;

    void Awake()
    {
        // Se já existe outro sistema de entrega, destrói este para não duplicar.
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Define este objeto como o sistema principal de entregas.
        instancia = this;

        // Mantém as cartas, o tempo e o progresso mesmo quando troca de cena.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Começa com os painéis e elementos da gameplay escondidos.
        painelDerrota.SetActive(false);
        iconeCartas.SetActive(false);
        setaVoltar.SetActive(false);
        textoTempo.gameObject.SetActive(false);
        limitevoltar.SetActive(false);
        menuCartas.SetActive(false);
        painelVitoria.SetActive(false);
    }

    void Update()
    {
        // Lê o movimento horizontal usando o New Input System.
        float horizontal = 0f;

        // Só tenta ler o movimento se a ação estiver configurada e habilitada.
        if (mover != null && mover.action != null && mover.action.enabled)
        {
            horizontal = mover.action.ReadValue<Vector2>().x;
        }

        // Se está esperando o jogador apertar uma direção, inicia as entregas.
        if (esperandoTeclaVoltar && Mathf.Abs(horizontal) > 0.01f)
        {
            IniciarCronometro();
        }

        // Se o cronômetro não está rodando, não precisa contar nada.
        if (!cronometroRodando)
        {
            return;
        }

        // Vai diminuindo o tempo disponível a cada frame.
        tempoRestante -= Time.deltaTime;

        // Se o tempo acabou, interrompe a contagem.
        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            cronometroRodando = false;

            // Atualiza o HUD para mostrar que não sobrou tempo.
            AtualizarTexto();

            // Se não entregou as quatro cartas, o jogador perdeu.
            if (caixasConcluidas.Count < 4)
            {
                MostrarDerrota();
            }

            return;
        }

        // Mantém o cronômetro atualizado na tela.
        AtualizarTexto();
    }

    public void MostrarCartasESeta()
    {
        // Mostra as cartas, a seta e o limite de retorno.
        iconeCartas.SetActive(true);
        setaVoltar.SetActive(true);
        limitevoltar.SetActive(true);

        // Agora espera o jogador apertar uma direção para iniciar a fase.
        esperandoTeclaVoltar = true;

        // Procura o Maxwell que está na cena atual.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        // Trava a movimentação até o jogador começar as entregas.
        if (jogador != null)
        {
            jogador.podeMover = false;
        }
    }

    void IniciarCronometro()
    {
        // Marca que a fase começou e para de esperar o comando inicial.
        entregaComecou = true;
        esperandoTeclaVoltar = false;

        // Esconde a seta, mas mantém as cartas aparecendo.
        setaVoltar.SetActive(false);
        iconeCartas.SetActive(true);

        // Mostra o tempo e começa a contagem.
        textoTempo.gameObject.SetActive(true);
        tempoRestante = tempoInicial;
        cronometroRodando = true;

        // Já coloca o tempo inicial corretamente no HUD.
        AtualizarTexto();

        // Procura o jogador para liberar a movimentação.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = true;
        }
    }

    void AtualizarTexto()
    {
        // Arredonda o tempo para cima para não mostrar zero antes da hora.
        int tempoInteiro = Mathf.CeilToInt(tempoRestante);

        // Separa o tempo em minutos e segundos.
        int minutos = tempoInteiro / 60;
        int segundos = tempoInteiro % 60;

        // Mostra o cronômetro no formato 01:30, por exemplo.
        textoTempo.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public void AbrirMenuCartas(string corDaCaixa, CaixaCorreio caixa)
    {
        // Guarda a cor e a caixa com a qual o jogador está interagindo.
        corCaixaAtual = corDaCaixa;
        caixaAtual = caixa;

        // Abre o menu para escolher a carta.
        menuCartas.SetActive(true);

        // Procura o jogador da cena atual.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        // Impede o jogador de andar enquanto escolhe a carta.
        if (jogador != null)
        {
            jogador.podeMover = false;
        }
    }

    public void MarcarCaixaConcluida(string cor)
    {
        // Se essa cor ainda não foi entregue, adiciona à lista de concluídas.
        if (!caixasConcluidas.Contains(cor))
        {
            caixasConcluidas.Add(cor);
        }
    }

    public bool CaixaJaConcluida(string cor)
    {
        // Verifica se a caixa dessa cor já recebeu sua carta.
        return caixasConcluidas.Contains(cor);
    }

    public void FecharMenuCartas()
    {
        // Fecha o menu de escolha das cartas.
        menuCartas.SetActive(false);

        // Procura o jogador para devolver o controle.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = true;
        }
    }

    void MostrarDerrota()
    {
        // Se a derrota já foi mostrada, não faz tudo novamente.
        if (derrotaMostrada)
        {
            return;
        }

        // Marca que o jogador perdeu.
        derrotaMostrada = true;

        // Fecha o menu de cartas e mostra o painel de derrota.
        menuCartas.SetActive(false);
        painelDerrota.SetActive(true);

        // Procura o jogador e bloqueia sua movimentação.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        // Começa a tocar os sons de derrota na sequência.
        StartCoroutine(TocarSonsDerrota());

        // Pausa o jogo enquanto o painel de derrota está aberto.
        Time.timeScale = 0f;
    }

    public void TentarNovamente()
    {
        // Volta o tempo ao normal antes de reiniciar a fase.
        Time.timeScale = 1f;

        // Limpa a instância para a próxima tentativa começar do zero.
        instancia = null;

        // Destrói o sistema atual para não carregar o progresso antigo.
        Destroy(gameObject);

        // Recarrega a cena inicial da fase.
        SceneManager.LoadScene(cenaTentarNovamente);
    }

    public void VoltarParaMenu()
    {
        // Garante que o menu não fique pausado.
        Time.timeScale = 1f;

        // Limpa a instância e destrói os dados da tentativa atual.
        instancia = null;
        Destroy(gameObject);

        // Volta para o menu principal.
        SceneManager.LoadScene(cenaMenu);
    }

    public void TocarSomCartaCerta()
    {
        // Toca o efeito quando o jogador entrega a carta correta.
        audioSource.PlayOneShot(somCartaCerta);
    }

    public void TocarSomCartaErrada()
    {
        // Toca o efeito quando o jogador escolhe uma carta errada.
        audioSource.PlayOneShot(somCartaErrada);
    }

    IEnumerator TocarSonsDerrota()
    {
        // Primeiro toca o impacto da derrota.
        audioSource.PlayOneShot(somDerrotaImpacto);

        // Espera um pouquinho, mesmo com o jogo pausado.
        yield return new WaitForSecondsRealtime(0.45f);

        // Depois toca o som final da derrota.
        audioSource.PlayOneShot(somDerrotaFinal);
    }

    public void VerificarVitoria()
    {
        // Se as quatro caixas foram concluídas, o jogador venceu.
        if (caixasConcluidas.Count == 4)
        {
            MostrarVitoria();
        }
    }

    void MostrarVitoria()
    {
        // Impede que o painel de vitória apareça mais de uma vez.
        if (vitoriaMostrada)
        {
            return;
        }

        // Marca a vitória e interrompe o cronômetro.
        vitoriaMostrada = true;
        cronometroRodando = false;

        // Esconde os elementos da gameplay que não serão mais necessários.
        menuCartas.SetActive(false);
        iconeCartas.SetActive(false);
        textoTempo.gameObject.SetActive(false);

        // Procura o jogador e bloqueia sua movimentação.
        MovimentoPersonagem jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        // Toca o som de vitória.
        audioSource.PlayOneShot(somVitoria);

        // Mostra o painel de vitória na tela.
        painelVitoria.SetActive(true);
    }

    public void AvancarFase()
    {
        // Devolve o tempo ao normal antes de sair da fase.
        Time.timeScale = 1f;

        // Limpa a instância para não levar os dados de entrega para a próxima fase.
        instancia = null;
        Destroy(gameObject);

        // Carrega a cutscene seguinte.
        SceneManager.LoadScene("12_Cutscene02");
    }
}
