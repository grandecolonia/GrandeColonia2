
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("Textos")]

    public TMP_Text textoExplicacao;
    public TMP_Text textoContinuar;

    [Header("Explicações das fases")]

    [TextArea(3, 6)]
    public string[] fase1;

    [TextArea(3, 6)]
    public string[] fase2;

    [TextArea(3, 6)]
    public string[] fase3;

    [TextArea(3, 6)]
    public string[] fase4;

    [Header("Configurações")]

    public string cenaMenu = "02_Menu";

    private string[] explicacoesAtuais;
    private int explicacaoAtual = 0;

    void Start()
    {
        textoExplicacao.text = "Selecione uma fase para ver as explicações.";

        if (textoContinuar != null)
        {
            textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
        }
    }

    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelecionarFase(fase1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelecionarFase(fase2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelecionarFase(fase3);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SelecionarFase(fase4);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ProximaExplicacao();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            VoltarMenu();
        }
    }

    void SelecionarFase(string[] fase)
    {
        // Guarda os textos da fase escolhida.
        explicacoesAtuais = fase;
        explicacaoAtual = 0;

        MostrarExplicacao();
    }

    void ProximaExplicacao()
    {
        if (explicacoesAtuais == null || explicacoesAtuais.Length == 0)
        {
            return;
        }

        if (explicacaoAtual < explicacoesAtuais.Length - 1)
        {
            explicacaoAtual++;
            MostrarExplicacao();
        }
    }

    void MostrarExplicacao()
    {
        if (explicacoesAtuais == null || explicacoesAtuais.Length == 0)
        {
            textoExplicacao.text = "Ainda não há explicações para esta fase.";

            if (textoContinuar != null)
            {
                textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
            }

            return;
        }

        textoExplicacao.text = explicacoesAtuais[explicacaoAtual];

        if (textoContinuar != null)
        {
            if (explicacaoAtual < explicacoesAtuais.Length - 1)
            {
                textoContinuar.text = "ESPAÇO - Continuar | E - Menu";
            }
            else
            {
                textoContinuar.text = "1, 2, 3, 4 - Fases | E - Menu";
            }
        }
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(cenaMenu);
    }
}
