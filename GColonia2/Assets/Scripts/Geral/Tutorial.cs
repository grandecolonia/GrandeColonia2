
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("Textos")]

    // Texto principal onde as explicações de cada fase vão aparecer.
    public TMP_Text textoExplicacao;

    // Texto que mostra quais teclas o jogador pode utilizar.
    public TMP_Text textoContinuar;

    [Header("Explicações das fases")]

    // Guarda as explicações da primeira fase, podendo ter várias páginas.
    [TextArea(3, 6)]
    public string[] fase1;

    // Guarda as explicações da segunda fase.
    [TextArea(3, 6)]
    public string[] fase2;

    // Guarda as explicações da terceira fase.
    [TextArea(3, 6)]
    public string[] fase3;

    // Guarda as explicações da quarta fase.
    [TextArea(3, 6)]
    public string[] fase4;

    [Header("Configurações")]

    // Cena que será carregada quando o jogador sair do tutorial.
    public string cenaMenu = "02_Menu";

    // Guarda os textos da fase que o jogador escolheu.
    private string[] explicacoesAtuais;

    // Controla qual página da explicação está aparecendo.
    private int explicacaoAtual = 0;

    void Start()
    {
        // Começa pedindo para o jogador escolher qual fase deseja consultar.
        textoExplicacao.text = "Selecione uma fase para ver as explicações.";

        // Mostra os comandos disponíveis na tela inicial.
        if (textoContinuar != null)
        {
            textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
        }
    }

    void Update()
    {
        // Se não existe um teclado conectado, não tenta ler nenhum comando.
        if (Keyboard.current == null)
        {
            return;
        }

        // Se apertou 1, mostra as explicações da primeira fase.
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelecionarFase(fase1);
        }

        // Se apertou 2, mostra as explicações da segunda fase.
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelecionarFase(fase2);
        }

        // Se apertou 3, mostra as explicações da terceira fase.
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelecionarFase(fase3);
        }

        // Se apertou 4, mostra as explicações da quarta fase.
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SelecionarFase(fase4);
        }

        // Se apertou Espaço, tenta avançar para a próxima página.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ProximaExplicacao();
        }

        // Se apertou E, sai do tutorial e volta para o menu principal.
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            VoltarMenu();
        }
    }

    void SelecionarFase(string[] fase)
    {
        // Guarda as explicações da fase que o jogador acabou de escolher.
        explicacoesAtuais = fase;

        // Volta para a primeira página sempre que selecionar uma fase.
        explicacaoAtual = 0;

        // Mostra a primeira explicação da fase escolhida.
        MostrarExplicacao();
    }

    void ProximaExplicacao()
    {
        // Se nenhuma fase foi selecionada ou não existem textos, não faz nada.
        if (explicacoesAtuais == null || explicacoesAtuais.Length == 0)
        {
            return;
        }

        // Só permite avançar se ainda existir outra página para mostrar.
        if (explicacaoAtual < explicacoesAtuais.Length - 1)
        {
            // Passa para a próxima explicação.
            explicacaoAtual++;

            // Atualiza o texto que aparece na tela.
            MostrarExplicacao();
        }
    }

    void MostrarExplicacao()
    {
        // Se a fase não tem explicações cadastradas, mostra um aviso.
        if (explicacoesAtuais == null || explicacoesAtuais.Length == 0)
        {
            textoExplicacao.text = "Ainda não há explicações para esta fase.";

            // Mantém os comandos de seleção disponíveis.
            if (textoContinuar != null)
            {
                textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
            }

            return;
        }

        // Mostra o texto da página atual da fase escolhida.
        textoExplicacao.text = explicacoesAtuais[explicacaoAtual];

        // Atualiza as instruções que aparecem abaixo da explicação.
        if (textoContinuar != null)
        {
            // Se ainda existem páginas, avisa que Espaço passa para a próxima.
            if (explicacaoAtual < explicacoesAtuais.Length - 1)
            {
                textoContinuar.text = "ESPAÇO - Continuar | E - Menu";
            }
            else
            {
                // Se chegou à última página, mostra os comandos para escolher outra fase.
                textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
            }
        }
    }

    public void VoltarMenu()
    {
        // Sai do tutorial e carrega o menu principal.
        SceneManager.LoadScene(cenaMenu);
    }
}
