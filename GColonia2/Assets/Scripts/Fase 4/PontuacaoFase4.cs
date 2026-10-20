
using UnityEngine;
using TMPro;

public class PontuacaoFase4 : MonoBehaviour
{
    public static PontuacaoFase4 instancia;

    [Header("Interface")]
    public TMP_Text textoPontuacao;

    [Header("Penalidade")]
    [Range(0, 100)] public float penalidadeAliado = 10f;

    private int pontos;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        AtualizarTexto();
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;

        AtualizarTexto();

        Debug.Log("Pontuação atual: " + pontos);
    }

    public void PenalizarAliado()
    {
        // Desconta uma porcentagem dos pontos atuais
        int perda = Mathf.CeilToInt(pontos * penalidadeAliado / 100f);

        pontos = Mathf.Max(0, pontos - perda);

        AtualizarTexto();

        Debug.Log("Aliado atingido! -" + perda + " pontos. Total: " + pontos);
    }

    void AtualizarTexto()
    {
        if (textoPontuacao != null)
            textoPontuacao.text = "Pontos: " + pontos;
    }
}
