
using UnityEngine;
using TMPro;

public class PontuacaoFase4 : MonoBehaviour
{
    // Permite que outros scripts acessem o sistema de pontuação.
    public static PontuacaoFase4 instancia;

    [Header("Interface")]

    // Texto que mostra a pontuação atual durante a fase.
    public TMP_Text textoPontuacao;

    [Header("Penalidade")]

    // Porcentagem de pontos perdidos quando Maxwell acerta um aliado.
    [Range(0, 100)]
    public float penalidadeAliado = 10f;

    // Guarda a quantidade atual de pontos do jogador.
    private int pontos;

    void Awake()
    {
        // Guarda a referência deste sistema para outros scripts utilizarem.
        instancia = this;
    }

    void Start()
    {
        // Mostra a pontuação inicial quando a fase começa.
        AtualizarTexto();
    }

    public void AdicionarPontos(int quantidade)
    {
        // Adiciona a quantidade recebida à pontuação atual.
        pontos += quantidade;

        // Atualiza a pontuação mostrada na interface.
        AtualizarTexto();
    }

    public void PenalizarAliado()
    {
        // Calcula a porcentagem de pontos que Maxwell perderá.
        int perda = Mathf.CeilToInt(pontos * penalidadeAliado / 100f);

        // Desconta os pontos sem permitir que a pontuação fique negativa.
        pontos = Mathf.Max(0, pontos - perda);

        // Atualiza o texto depois de aplicar a penalidade.
        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        // Se o texto foi configurado, mostra a pontuação atual.
        if (textoPontuacao != null)
        {
            textoPontuacao.text = "Pontos: " + pontos;
        }
    }
}
