
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalizacaoFase4 : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painelVitoria;
    public GameObject painelDerrota;
    public DisparoArmaFase4 arma;
    public RotacaoArmaFase4 rotacaoArma;
    public GerenciadorOndasFase4 gerenciadorOndas;

    [Header("Derrota")]
    public int limiteFugas = 3;

    [Header("Cenas")]
    public string nomeCenaMenu;

    private int fugas;
    private bool jogoTerminou;

public string nomeCenaProxima;

    void Awake()
    {
        Time.timeScale = 1f;

        painelVitoria.SetActive(false);
        painelDerrota.SetActive(false);
    }

    public void RegistrarFuga()
    {
        if (jogoTerminou) return;

        fugas++;

        Debug.Log("Inimigos que escaparam: " + fugas + "/" + limiteFugas);

        if (fugas >= limiteFugas)
            Perder();
    }

    public void Vencer()
    {
        FinalizarJogo(true);
    }

    public void Perder()
    {
        FinalizarJogo(false);
    }

    void FinalizarJogo(bool venceu)
    {
        if (jogoTerminou) return;

        jogoTerminou = true;

        if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarFinalizacao(venceu);

        // Interrompe o combate
        gerenciadorOndas.PararOndas();

        arma.enabled = false;
        rotacaoArma.enabled = false;

        if (venceu)
            painelVitoria.SetActive(true);
        else
            painelDerrota.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        if (string.IsNullOrEmpty(nomeCenaMenu))
        {
            Debug.LogWarning("Configure a cena do menu!");
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public int ConsultarFugas()
{
    return fugas;
}

public void AvancarCena()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(nomeCenaProxima);
}
}
