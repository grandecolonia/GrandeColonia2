
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraVidaGeneralFase4 : MonoBehaviour
{
    public GameObject painelVida;
    public Image preenchimento;
    public TMP_Text textoVida;

    private GeneralFase4 general;

    void Start()
    {
        painelVida.SetActive(false);
    }

    public void DefinirGeneral(GeneralFase4 novoGeneral)
    {
        general = novoGeneral;
    }

    void Update()
    {
        if (general == null || general.EstaEscondido())
        {
            painelVida.SetActive(false);
            return;
        }

        painelVida.SetActive(true);

        int vida = general.ConsultarVida();

        preenchimento.fillAmount = (float)vida / general.vidaMaxima;
        textoVida.text = "GENERAL - " + vida + "/" + general.vidaMaxima;
    }
}
