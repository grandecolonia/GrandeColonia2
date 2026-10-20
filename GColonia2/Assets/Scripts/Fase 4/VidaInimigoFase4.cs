
using UnityEngine;

public class VidaInimigoFase4 : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public int vidaMaxima = 40;
    public float tempoNaTela = 10f;
    public int pontosAoDerrotar = 100;

    private int vidaAtual;
    private bool derrotado;

    public System.Action AoMorrer;

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int dano)
    {
        if (derrotado) return;

        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        Debug.Log("Vida do inimigo: " + vidaAtual);

        if (vidaAtual <= 0)
            Morrer();
    }

    void Morrer()
    {
        derrotado = true;

        // Adiciona pontos somente quando morrer
        if (PontuacaoFase4.instancia != null)
            PontuacaoFase4.instancia.AdicionarPontos(pontosAoDerrotar);

        AoMorrer?.Invoke();

        Destroy(gameObject);
    }
}
