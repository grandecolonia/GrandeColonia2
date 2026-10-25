
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraVidaGeneralFase4 : MonoBehaviour
{
    [Header("Interface")]

    // Painel que contém a barra de vida do General.
    public GameObject painelVida;

    // Imagem que diminui conforme o General recebe dano.
    public Image preenchimento;

    // Texto que mostra a quantidade atual e máxima de vida.
    public TMP_Text textoVida;

    [Header("General")]

    // Guarda a referência do General que está enfrentando Maxwell.
    private GeneralFase4 general;

    void Start()
    {
        // Começa com a barra de vida escondida.
        painelVida.SetActive(false);
    }

    public void DefinirGeneral(GeneralFase4 novoGeneral)
    {
        // Guarda qual General terá a vida mostrada na interface.
        general = novoGeneral;
    }

    void Update()
    {
        // Se o General não existe ou ainda está escondido, oculta a barra.
        if (general == null || general.EstaEscondido())
        {
            painelVida.SetActive(false);
            return;
        }

        // Mostra a barra de vida quando o General está presente.
        painelVida.SetActive(true);

        // Consulta a quantidade de vida que ainda resta ao General.
        int vida = general.ConsultarVida();

        // Atualiza o preenchimento conforme a vida atual e a vida máxima.
        preenchimento.fillAmount = (float)vida / general.vidaMaxima;

        // Mostra os valores de vida na interface.
        textoVida.text = "GENERAL - " + vida + "/" + general.vidaMaxima;
    }
}
