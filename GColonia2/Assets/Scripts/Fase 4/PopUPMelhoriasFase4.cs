
using UnityEngine;
using TMPro;

public class PopUPMelhoriasFase4 : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painel;
    public TMP_Text textoNome;
    public TMP_Text textoDescricao;
    public DisparoArmaFase4 arma;
    public MelhoriasFase4 melhorias;

    void Start()
    {
        painel.SetActive(false);
    }

    public void Abrir(int tipo)
    {
        if (tipo == 0)
        {
            textoNome.text = "Gatilho Ágil";
            textoDescricao.text = "Diminui o intervalo entre os tiros.\n\n" +
                arma.intervaloEntreTiros.ToString("0.00") + "s → " +
                (arma.intervaloEntreTiros * melhorias.multiplicadorGatilho).ToString("0.00") + "s";
        }
        else if (tipo == 1)
        {
            textoNome.text = "Carregador Ampliado";
            textoDescricao.text = "Aumenta a capacidade do carregador.\n\n" +
                arma.capacidadeCarregador + " → " +
                (arma.capacidadeCarregador + melhorias.balasExtras) + " balas";
        }
        else if (tipo == 2)
        {
            textoNome.text = "Recarga Rápida";
            textoDescricao.text = "Diminui o tempo de recarga.\n\n" +
                arma.tempoRecarga.ToString("0.00") + "s → " +
                (arma.tempoRecarga * melhorias.multiplicadorRecarga).ToString("0.00") + "s";
        }
        else if (tipo == 3)
        {
            textoNome.text = "Ferrão Reforçado";
            textoDescricao.text = "Aumenta o dano dos disparos.\n\n" +
                arma.danoPorTiro + " → " +
                (arma.danoPorTiro * melhorias.multiplicadorDano) + " de dano";
        }
        else if (tipo == 4)
        {
            textoNome.text = "Tiro Carregado";
            textoDescricao.text = "Segure o disparo por " + arma.tempoParaCarregar.ToString("0.0") +
                " segundo(s) e solte para causar mais dano.\n\n" +
                arma.danoPorTiro + " → " +
                (arma.danoPorTiro * arma.multiplicadorDanoCarregado) + " de dano";
        }
        else
        {
            return;
        }

        painel.SetActive(true);
    }

    public void Fechar()
    {
        painel.SetActive(false);
    }
}
