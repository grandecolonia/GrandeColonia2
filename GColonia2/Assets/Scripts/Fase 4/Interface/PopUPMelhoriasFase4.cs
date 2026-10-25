
using UnityEngine;
using TMPro;

public class PopUPMelhoriasFase4 : MonoBehaviour
{
    [Header("Interface")]

    // Painel que mostra os detalhes da melhoria selecionada.
    public GameObject painel;

    // Texto responsável por mostrar o nome do upgrade.
    public TMP_Text textoNome;

    // Texto que explica a melhoria e mostra seus novos valores.
    public TMP_Text textoDescricao;

    [Header("Referências")]

    // Sistema da arma que fornece os valores atuais dos disparos.
    public DisparoArmaFase4 arma;

    // Sistema que guarda os valores configurados para cada melhoria.
    public MelhoriasFase4 melhorias;

    void Start()
    {
        // Começa com o painel de informações escondido.
        painel.SetActive(false);
    }

    public void Abrir(int tipo)
    {
        // GATILHO ÁGIL: mostra a redução no intervalo entre os tiros.
        if (tipo == 0)
        {
            textoNome.text = "Gatilho Ágil";

            // Mostra o tempo atual e o novo intervalo depois da melhoria.
            textoDescricao.text = "Diminui o intervalo entre os tiros.\n\n" +
                arma.intervaloEntreTiros.ToString("0.00") + "s → " +
                (arma.intervaloEntreTiros * melhorias.multiplicadorGatilho).ToString("0.00") + "s";
        }

        // CARREGADOR AMPLIADO: mostra quantas balas serão adicionadas.
        else if (tipo == 1)
        {
            textoNome.text = "Carregador Ampliado";

            // Compara a capacidade atual com a capacidade depois do upgrade.
            textoDescricao.text = "Aumenta a capacidade do carregador.\n\n" +
                arma.capacidadeCarregador + " → " +
                (arma.capacidadeCarregador + melhorias.balasExtras) + " balas";
        }

        // RECARGA RÁPIDA: mostra a redução no tempo de recarga.
        else if (tipo == 2)
        {
            textoNome.text = "Recarga Rápida";

            // Mostra o tempo atual e o novo tempo depois da melhoria.
            textoDescricao.text = "Diminui o tempo de recarga.\n\n" +
                arma.tempoRecarga.ToString("0.00") + "s → " +
                (arma.tempoRecarga * melhorias.multiplicadorRecarga).ToString("0.00") + "s";
        }

        // FERRÃO REFORÇADO: mostra o aumento do dano dos disparos.
        else if (tipo == 3)
        {
            textoNome.text = "Ferrão Reforçado";

            // Compara o dano atual com o dano depois da melhoria.
            textoDescricao.text = "Aumenta o dano dos disparos.\n\n" +
                arma.danoPorTiro + " → " +
                (arma.danoPorTiro * melhorias.multiplicadorDano) + " de dano";
        }

        // TIRO CARREGADO: explica como utilizar o disparo especial.
        else if (tipo == 4)
        {
            textoNome.text = "Tiro Carregado";

            // Mostra o tempo necessário para carregar e o dano do disparo especial.
            textoDescricao.text = "Segure o disparo por " + arma.tempoParaCarregar.ToString("0.0") +
                " segundo(s) e solte para causar mais dano.\n\n" +
                arma.danoPorTiro + " → " +
                (arma.danoPorTiro * arma.multiplicadorDanoCarregado) + " de dano";
        }
        else
        {
            // Se recebeu um tipo inválido, não abre o painel.
            return;
        }

        // Mostra o painel depois de configurar os textos.
        painel.SetActive(true);
    }

    public void Fechar()
    {
        // Esconde o painel de informações da melhoria.
        painel.SetActive(false);
    }
}
