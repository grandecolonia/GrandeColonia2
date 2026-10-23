
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleBotoesResultado : MonoBehaviour
{
    [Header("Cenas")]
    public string cenaInicialFase = "08_Fase1.2";
    public string cenaMenu = "02_Menu";
    public string proximaCena = "12_Cutscene02";

    // Reinicia a fase desde o começo.
    public void TentarNovamente()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaInicialFase);
    }

    // Volta para o menu principal.
    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaMenu);
    }

    // Continua após concluir a fase.
    public void Avancar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(proximaCena);
    }
}
