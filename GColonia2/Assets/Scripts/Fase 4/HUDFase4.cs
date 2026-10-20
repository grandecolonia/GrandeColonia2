
using UnityEngine;
using TMPro;

public class HUDFase4 : MonoBehaviour
{
    [Header("Referências")]
    public DisparoArmaFase4 arma;
    public FinalizacaoFase4 finalizacao;
    public GerenciadorOndasFase4 gerenciadorOndas;

    [Header("Textos")]
    public TMP_Text textoMunicao, textoRecarga, textoFugas, textoOndaAtual;

    void Update()
    {
        if (arma == null || finalizacao == null || gerenciadorOndas == null) return;

        // Atualiza a munição
        textoMunicao.text = "MUNIÇÃO: " + arma.ConsultarMunicao() + "/" + arma.capacidadeCarregador;

        // Mostra quando a arma está recarregando
        if (arma.EstaRecarregando())
        {
            textoRecarga.text = "RECARREGANDO...";
        }
        else
        {
            textoRecarga.text = "";
        }

        // Atualiza as fugas
        textoFugas.text = "FUGAS: " + finalizacao.ConsultarFugas() + "/" + finalizacao.limiteFugas;

        // Mantém o nome da onda visível
        if (gerenciadorOndas.textoOnda != null)
        {
            textoOndaAtual.text = gerenciadorOndas.textoOnda.text;
        }
    }
}
