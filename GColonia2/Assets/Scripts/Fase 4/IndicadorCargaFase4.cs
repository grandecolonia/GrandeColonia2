
using UnityEngine;
using UnityEngine.UI;

public class IndicadorCargaFase4 : MonoBehaviour
{
    [Header("Referências")]
    public DisparoArmaFase4 arma;
    public Image preenchimento;
    public CanvasGroup grupo;

    [Header("Cores")]
    public Color corCarregando = Color.yellow;
    public Color corCompleta = Color.green;

    void Start()
    {
        grupo.alpha = 0f;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
    }

    void Update()
    {
        if (arma == null || preenchimento == null || grupo == null) return;

        // Mostra a barra durante o carregamento
        if (arma.EstaCarregando())
        {
            grupo.alpha = 1f;
        }
        else
        {
            grupo.alpha = 0f;
        }

        float progresso = arma.ProgressoCarga();

        preenchimento.fillAmount = progresso;

        // Muda a cor quando terminar de carregar
        if (progresso >= 1f)
        {
            preenchimento.color = corCompleta;
        }
        else
        {
            preenchimento.color = corCarregando;
        }
    }
}
