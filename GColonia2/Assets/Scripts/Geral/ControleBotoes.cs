
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleBotoes : MonoBehaviour
{
    [Header("Cenas")]

    // Cena que será carregada quando o jogador quiser tentar a fase novamente.
    public string cenaInicialFase = "08_Fase1.2";

    // Cena do menu principal para onde o jogador pode voltar.
    public string cenaMenu = "02_Menu";

    // Cena que vem depois quando o jogador terminar a fase.
    public string proximaCena = "12_Cutscene02";

    public void TentarNovamente()
    {
        // Volta o tempo ao normal caso o jogo tenha sido pausado.
        Time.timeScale = 1f;

        // Recarrega a fase desde o começo.
        SceneManager.LoadScene(cenaInicialFase);
    }

    public void VoltarMenu()
    {
        // Garante que o jogo não continue pausado quando voltar ao menu.
        Time.timeScale = 1f;

        // Sai da fase e carrega o menu principal.
        SceneManager.LoadScene(cenaMenu);
    }

    public void Avancar()
    {
        // Deixa o tempo normal antes de carregar a próxima cena.
        Time.timeScale = 1f;

        // Carrega a cena que vem depois da fase concluída.
        SceneManager.LoadScene(proximaCena);
    }
}
