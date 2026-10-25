
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalizacaoFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Painel mostrado quando Maxwell vence a fase.
    public GameObject painelVitoria;

    // Painel mostrado quando Maxwell perde a fase.
    public GameObject painelDerrota;

    // Sistema de disparo que será desativado quando a fase terminar.
    public DisparoArmaFase4 arma;

    // Sistema de rotação da arma que será desativado no final.
    public RotacaoArmaFase4 rotacaoArma;

    // Sistema responsável pelas ondas de inimigos.
    public GerenciadorOndasFase4 gerenciadorOndas;

    [Header("Derrota")]

    // Quantidade máxima de inimigos que podem escapar antes da derrota.
    public int limiteFugas = 3;

    [Header("Cenas")]

    // Nome da cena do menu principal.
    public string nomeCenaMenu;

    // Nome da cena carregada quando Maxwell avança depois da vitória.
    public string nomeCenaProxima;

    // Guarda quantos inimigos já conseguiram escapar.
    private int fugas;

    // Impede que a fase seja finalizada mais de uma vez.
    private bool jogoTerminou;

    void Awake()
    {
        // Garante que o tempo do jogo esteja normal ao iniciar a fase.
        Time.timeScale = 1f;

        // Começa com o painel de vitória escondido.
        painelVitoria.SetActive(false);

        // Começa com o painel de derrota escondido.
        painelDerrota.SetActive(false);
    }

    public void RegistrarFuga()
    {
        // Se a fase já terminou, não registra novas fugas.
        if (jogoTerminou)
        {
            return;
        }

        // Acrescenta uma fuga à contagem.
        fugas++;

        // Se atingiu o limite de fugas, Maxwell perde a fase.
        if (fugas >= limiteFugas)
        {
            Perder();
        }
    }

    public void Vencer()
    {
        // Encerra a fase mostrando o resultado de vitória.
        FinalizarJogo(true);
    }

    public void Perder()
    {
        // Encerra a fase mostrando o resultado de derrota.
        FinalizarJogo(false);
    }

    void FinalizarJogo(bool venceu)
    {
        // Impede que a vitória ou derrota aconteça mais de uma vez.
        if (jogoTerminou)
        {
            return;
        }

        // Marca que a fase foi encerrada.
        jogoTerminou = true;

        // Reproduz o som correspondente ao resultado.
        if (AudioFase4.instancia != null)
        {
            AudioFase4.instancia.TocarFinalizacao(venceu);
        }

        // Interrompe o aparecimento de novos inimigos e os eventos das ondas.
        gerenciadorOndas.PararOndas();

        // Bloqueia os disparos da arma.
        arma.enabled = false;

        // Impede que a arma continue acompanhando o mouse.
        rotacaoArma.enabled = false;

        // Mostra o painel correspondente ao resultado da fase.
        if (venceu)
        {
            painelVitoria.SetActive(true);
        }
        else
        {
            painelDerrota.SetActive(true);
        }

        // Pausa o jogo enquanto o jogador escolhe o que fazer.
        Time.timeScale = 0f;
    }

    public void Reiniciar()
    {
        // Restaura a velocidade normal do jogo.
        Time.timeScale = 1f;

        // Recarrega a cena atual para começar novamente.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        // Se nenhuma cena de menu foi configurada, cancela a transição.
        if (string.IsNullOrEmpty(nomeCenaMenu))
        {
            return;
        }

        // Restaura o tempo antes de voltar ao menu.
        Time.timeScale = 1f;

        // Carrega a cena do menu principal.
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public int ConsultarFugas()
    {
        // Devolve quantos inimigos já escaparam.
        return fugas;
    }

    public void AvancarCena()
    {
        // Restaura a velocidade normal do jogo.
        Time.timeScale = 1f;

        // Carrega a próxima cena configurada no Inspector.
        SceneManager.LoadScene(nomeCenaProxima);
    }
}
