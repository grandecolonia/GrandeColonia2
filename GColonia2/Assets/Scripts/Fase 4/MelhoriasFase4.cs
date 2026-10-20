
using System.Collections;
using UnityEngine;

public class MelhoriasFase4 : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painelMelhorias;
    public DisparoArmaFase4 arma;

    [Header("Gatilho Ágil")]
    [Range(0.1f, 1f)] public float multiplicadorGatilho = 0.75f;

    [Header("Carregador Ampliado")]
    public int balasExtras = 4;

    [Header("Recarga Rápida")]
    [Range(0.1f, 1f)] public float multiplicadorRecarga = 0.65f;

    [Header("Ferrão Reforçado")]
    public int multiplicadorDano = 2;

    private bool escolhaFeita;

    void Awake()
    {
        painelMelhorias.SetActive(false);
    }

    public IEnumerator AbrirMelhorias()
    {
        escolhaFeita = false;

        painelMelhorias.SetActive(true);
        arma.enabled = false;

        // Espera escolher uma melhoria
        yield return new WaitUntil(() => escolhaFeita);

        if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarMelhoria();

        painelMelhorias.SetActive(false);
        arma.enabled = true;
    }

    public void EscolherGatilhoAgil()
    {
        if (escolhaFeita) return;

        arma.intervaloEntreTiros *= multiplicadorGatilho;

        Debug.Log("Gatilho Ágil ativado!");
        escolhaFeita = true;
    }

    public void EscolherCarregadorAmpliado()
    {
        if (escolhaFeita) return;

        arma.AumentarCarregador(balasExtras);

        Debug.Log("Carregador Ampliado ativado!");
        escolhaFeita = true;
    }

    public void EscolherRecargaRapida()
    {
        if (escolhaFeita) return;

        arma.tempoRecarga *= multiplicadorRecarga;

        Debug.Log("Recarga Rápida ativada! Novo tempo: " + arma.tempoRecarga);
        escolhaFeita = true;
    }

    public void EscolherFerraoReforcado()
{
    if (escolhaFeita) return;

    // Dobra o dano da arma
    arma.danoPorTiro *= multiplicadorDano;

    Debug.Log("Ferrão Reforçado ativado! Novo dano: " + arma.danoPorTiro);

    escolhaFeita = true;
}

public void EscolherTiroCarregado()
{
    if (escolhaFeita) return;

    // Ativa o tiro carregado
    arma.tiroCarregadoAtivo = true;

    Debug.Log("Tiro Carregado ativado!");

    escolhaFeita = true;
}
}
