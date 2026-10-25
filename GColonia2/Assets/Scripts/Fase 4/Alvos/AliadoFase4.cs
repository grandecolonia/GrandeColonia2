
using UnityEngine;

public class AliadoFase4 : MonoBehaviour
{
    [Header("Configurações do Aliado")]

    // Tempo configurado para o aliado permanecer na tela.
    public float tempoNaTela = 10f;

    // Impede que o mesmo aliado receba mais de um tiro.
    private bool foiAtingido = false;

    public void ReceberTiro()
    {
        // Se o aliado já foi atingido, não aplica outra penalidade.
        if (foiAtingido)
        {
            return;
        }

        // Marca que o aliado recebeu um tiro.
        foiAtingido = true;

        // Reproduz o som de aliado atingido, caso o sistema de áudio exista.
        if (AudioFase4.instancia != null)
        {
            AudioFase4.instancia.TocarAliado();
        }

        // Aplica a penalidade na pontuação por acertar um aliado.
        if (PontuacaoFase4.instancia != null)
        {
            PontuacaoFase4.instancia.PenalizarAliado();
        }

        // Remove o aliado da cena depois de receber o tiro.
        Destroy(gameObject);
    }
}
