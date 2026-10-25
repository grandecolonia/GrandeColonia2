
using System.Collections;
using UnityEngine;

public class MelhoriasFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Painel que mostra as opções de melhorias entre as ondas.
    public GameObject painelMelhorias;

    // Sistema da arma que receberá as melhorias escolhidas.
    public DisparoArmaFase4 arma;

    [Header("Gatilho Ágil")]

    // Multiplicador que diminui o tempo de espera entre disparos.
    [Range(0.1f, 1f)]
    public float multiplicadorGatilho = 0.75f;

    [Header("Carregador Ampliado")]

    // Quantidade de balas adicionadas à capacidade do carregador.
    public int balasExtras = 4;

    [Header("Recarga Rápida")]

    // Multiplicador que diminui o tempo necessário para recarregar.
    [Range(0.1f, 1f)]
    public float multiplicadorRecarga = 0.65f;

    [Header("Ferrão Reforçado")]

    // Multiplicador aplicado ao dano dos disparos.
    public int multiplicadorDano = 2;

    // Impede que o jogador escolha mais de uma melhoria por vez.
    private bool escolhaFeita = false;

    void Awake()
    {
        // Começa com o painel de melhorias escondido.
        painelMelhorias.SetActive(false);
    }

    public IEnumerator AbrirMelhorias()
    {
        // Permite que Maxwell faça uma nova escolha.
        escolhaFeita = false;

        // Mostra o painel com as cinco opções disponíveis.
        painelMelhorias.SetActive(true);

        // Bloqueia os disparos enquanto o jogador escolhe uma melhoria.
        arma.enabled = false;

        // Espera até Maxwell selecionar uma das melhorias.
        yield return new WaitUntil(() => escolhaFeita);

        // Reproduz o som de melhoria escolhida.
        if (AudioFase4.instancia != null)
        {
            AudioFase4.instancia.TocarMelhoria();
        }

        // Esconde o painel depois que a escolha foi realizada.
        painelMelhorias.SetActive(false);

        // Libera novamente os disparos da arma.
        arma.enabled = true;
    }

    public void EscolherGatilhoAgil()
    {
        // Impede que outra melhoria seja escolhida na mesma rodada.
        if (escolhaFeita)
        {
            return;
        }

        // Diminui o intervalo necessário entre os tiros.
        arma.intervaloEntreTiros *= multiplicadorGatilho;

        // Confirma que Maxwell escolheu uma melhoria.
        escolhaFeita = true;
    }

    public void EscolherCarregadorAmpliado()
    {
        // Impede escolher mais de uma melhoria.
        if (escolhaFeita)
        {
            return;
        }

        // Aumenta a capacidade da arma e completa o carregador.
        arma.AumentarCarregador(balasExtras);

        // Confirma a escolha do jogador.
        escolhaFeita = true;
    }

    public void EscolherRecargaRapida()
    {
        // Impede que outra melhoria seja escolhida.
        if (escolhaFeita)
        {
            return;
        }

        // Reduz o tempo necessário para recarregar a arma.
        arma.tempoRecarga *= multiplicadorRecarga;

        // Confirma que Maxwell escolheu a melhoria.
        escolhaFeita = true;
    }

    public void EscolherFerraoReforcado()
    {
        // Impede selecionar outra melhoria na mesma rodada.
        if (escolhaFeita)
        {
            return;
        }

        // Aumenta o dano causado pelos disparos normais e carregados.
        arma.danoPorTiro *= multiplicadorDano;

        // Confirma a melhoria escolhida.
        escolhaFeita = true;
    }

    public void EscolherTiroCarregado()
    {
        // Impede que Maxwell escolha duas melhorias na mesma rodada.
        if (escolhaFeita)
        {
            return;
        }

        // Libera a mecânica de segurar e soltar para carregar um tiro.
        arma.tiroCarregadoAtivo = true;

        // Confirma que Maxwell escolheu a melhoria.
        escolhaFeita = true;
    }
}
