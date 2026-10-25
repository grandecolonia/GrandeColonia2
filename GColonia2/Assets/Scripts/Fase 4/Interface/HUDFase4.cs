
using UnityEngine;
using TMPro;

public class HUDFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Sistema responsável pelos disparos e pela munição da arma.
    public DisparoArmaFase4 arma;

    // Sistema que controla as fugas, a vitória e a derrota.
    public FinalizacaoFase4 finalizacao;

    // Sistema responsável pelas ondas de inimigos.
    public GerenciadorOndasFase4 gerenciadorOndas;

    [Header("Textos")]

    // Texto que mostra a quantidade atual de balas no carregador.
    public TMP_Text textoMunicao;

    // Texto que avisa quando a arma está recarregando.
    public TMP_Text textoRecarga;

    // Texto que mostra quantos inimigos já escaparam.
    public TMP_Text textoFugas;

    // Texto que mostra o nome da onda atual.
    public TMP_Text textoOndaAtual;

    void Update()
    {
        // Se algum dos sistemas principais não foi configurado, não atualiza o HUD.
        if (arma == null || finalizacao == null || gerenciadorOndas == null)
        {
            return;
        }

        // Atualiza a quantidade atual de munição e a capacidade do carregador.
        textoMunicao.text = "MUNIÇÃO: " + arma.ConsultarMunicao() + "/" + arma.capacidadeCarregador;

        // Verifica se a arma está recarregando.
        if (arma.EstaRecarregando())
        {
            // Mostra o aviso enquanto a recarga acontece.
            textoRecarga.text = "RECARREGANDO...";
        }
        else
        {
            // Esconde o aviso quando a arma não está recarregando.
            textoRecarga.text = "";
        }

        // Atualiza a quantidade de fugas e o limite permitido.
        textoFugas.text = "FUGAS: " + finalizacao.ConsultarFugas() + "/" + finalizacao.limiteFugas;

        // Verifica se o gerenciador possui um texto de identificação da onda.
        if (gerenciadorOndas.textoOnda != null)
        {
            // Copia o nome da onda para o texto do HUD.
            textoOndaAtual.text = gerenciadorOndas.textoOnda.text;
        }
    }
}
