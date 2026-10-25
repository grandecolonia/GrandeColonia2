
using UnityEngine;

public class VidaInimigoFase4 : MonoBehaviour
{
    [Header("Configurações do Inimigo")]

    // Quantidade máxima de vida que o inimigo possui.
    public int vidaMaxima = 40;

    // Tempo máximo que o inimigo pode permanecer na tela.
    public float tempoNaTela = 10f;

    // Quantidade de pontos recebidos quando Maxwell derrota o inimigo.
    public int pontosAoDerrotar = 100;

    // Guarda a quantidade atual de vida do inimigo.
    private int vidaAtual;

    // Impede que o mesmo inimigo seja derrotado mais de uma vez.
    private bool derrotado = false;

    // Evento que avisa outros scripts quando o inimigo é derrotado.
    public System.Action AoMorrer;

    void Start()
    {
        // Começa com a vida completa.
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int dano)
    {
        // Se o inimigo já foi derrotado, não recebe mais dano.
        if (derrotado)
        {
            return;
        }

        // Diminui a vida sem permitir valores negativos.
        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        // Se a vida chegou a zero, derrota o inimigo.
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        // Marca que o inimigo foi derrotado.
        derrotado = true;

        // Adiciona os pontos da derrota, caso o sistema de pontuação exista.
        if (PontuacaoFase4.instancia != null)
        {
            PontuacaoFase4.instancia.AdicionarPontos(pontosAoDerrotar);
        }

        // Avisa aos sistemas de ondas que o inimigo foi eliminado.
        AoMorrer?.Invoke();

        // Remove o inimigo da cena depois da derrota.
        Destroy(gameObject);
    }
}
