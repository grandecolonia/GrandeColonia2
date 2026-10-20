
using UnityEngine;

public class AliadoFase4 : MonoBehaviour
{
    [Header("Configurações do Aliado")]
    public float tempoNaTela = 10f;

    private bool foiAtingido;

    public void ReceberTiro()
    {
        if (foiAtingido) return;

        foiAtingido = true;

        if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarAliado();

        // Aplica a penalidade por acertar o aliado
        if (PontuacaoFase4.instancia != null)
            PontuacaoFase4.instancia.PenalizarAliado();

        Destroy(gameObject);
    }
}
