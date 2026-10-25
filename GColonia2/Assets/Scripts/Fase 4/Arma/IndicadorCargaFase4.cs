
using UnityEngine;
using UnityEngine.UI;

public class IndicadorCargaFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Sistema responsável pelo tiro carregado da arma.
    public DisparoArmaFase4 arma;

    // Imagem que mostra o progresso do carregamento.
    public Image preenchimento;

    // Controla a visibilidade da barra durante o carregamento.
    public CanvasGroup grupo;

    [Header("Cores")]

    // Cor utilizada enquanto Maxwell está carregando o tiro.
    public Color corCarregando = Color.yellow;

    // Cor utilizada quando o disparo está completamente carregado.
    public Color corCompleta = Color.green;

    void Start()
    {
        // Começa com a barra de carregamento escondida.
        grupo.alpha = 0f;

        // Impede que o indicador receba interações da interface.
        grupo.interactable = false;

        // Impede que a barra bloqueie cliques em outros elementos.
        grupo.blocksRaycasts = false;
    }

    void Update()
    {
        // Se alguma referência não foi configurada, não atualiza o indicador.
        if (arma == null || preenchimento == null || grupo == null)
        {
            return;
        }

        // Se Maxwell está carregando um disparo, mostra a barra.
        if (arma.EstaCarregando())
        {
            grupo.alpha = 1f;
        }
        else
        {
            // Esconde a barra quando o jogador não está carregando.
            grupo.alpha = 0f;
        }

        // Consulta o progresso atual do tiro carregado.
        float progresso = arma.ProgressoCarga();

        // Atualiza o preenchimento da barra conforme o progresso.
        preenchimento.fillAmount = progresso;

        // Se o disparo está completamente carregado, muda a barra para verde.
        if (progresso >= 1f)
        {
            preenchimento.color = corCompleta;
        }
        else
        {
            // Enquanto o tiro está carregando, mantém a barra amarela.
            preenchimento.color = corCarregando;
        }
    }
}
