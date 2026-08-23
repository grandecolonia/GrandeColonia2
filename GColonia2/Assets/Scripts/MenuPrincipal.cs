using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Grupos da interface e componente responsável pelo volume do jogo.
    [SerializeField] private GameObject menuinicial;
    [SerializeField] private GameObject opcoes;
    [SerializeField] private AudioSource som;

    public void AbrirOpcoes()
    {
        // Esconde o menu principal e mostra a tela de opções.
        menuinicial.SetActive(false);
        opcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        // Fecha as opções e retorna ao menu principal.
        menuinicial.SetActive(true);
        opcoes.SetActive(false);
    }

    public void ControlarSom(float value)
    {
        // Aplica ao volume o valor escolhido pelo jogador no Slider.
        som.volume = value;
    }

    public void IniciarCutscene()
    {
        SceneManager.LoadScene("05_Cutscene01");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("04_Tutorial");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("03_Creditos");
    }

    public void Fechar()
    {
        // Fecha o jogo quando ele estiver sendo executado fora do Editor.
        Application.Quit();
    }
}