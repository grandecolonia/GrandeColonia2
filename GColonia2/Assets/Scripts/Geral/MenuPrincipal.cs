
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Interface")]

    // Menu principal que aparece quando o jogador abre o jogo.
    [SerializeField] private GameObject menuinicial;

    // Painel que aparece quando o jogador abre as opções.
    [SerializeField] private GameObject opcoes;

    [Header("Som")]

    // Fonte de áudio que terá o volume controlado pelo Slider.
    [SerializeField] private AudioSource som;

    public void AbrirOpcoes()
    {
        // Esconde o menu inicial para mostrar as configurações.
        menuinicial.SetActive(false);

        // Abre o painel de opções.
        opcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        // Mostra o menu principal novamente.
        menuinicial.SetActive(true);

        // Fecha o painel de opções.
        opcoes.SetActive(false);
    }

    public void ControlarSom(float value)
    {
        // Ajusta o volume da fonte de áudio conforme o valor do Slider.
        som.volume = value;
    }

    public void IniciarCutscene()
    {
        // Começa a história carregando a primeira cutscene.
        SceneManager.LoadScene("05_Cutscene01");
    }

    public void Tutorial()
    {
        // Abre a cena onde o jogador pode consultar o tutorial.
        SceneManager.LoadScene("04_Tutorial");
    }

    public void Creditos()
    {
        // Abre a tela de créditos do jogo.
        SceneManager.LoadScene("03_Creditos");
    }

    public void Fechar()
    {
        // Fecha o jogo quando o jogador escolhe a opção de sair.
        Application.Quit();
    }
}
