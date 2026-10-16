
using UnityEngine;
using TMPro;

public class AplicarTamanhoCutscene : MonoBehaviour
{
    private TMP_Text texto;
    private float tamanhoAtual;

    void Awake()
    {
        texto = GetComponent<TMP_Text>();
        AtualizarTamanho();
    }

    void Update()
    {
        // Só atualiza quando o tamanho escolhido mudar.
        if (tamanhoAtual != ConfiguracoesJogo.ObterTamanhoTexto())
        {
            AtualizarTamanho();
        }
    }

    void AtualizarTamanho()
    {
        tamanhoAtual = ConfiguracoesJogo.ObterTamanhoTexto();
        texto.fontSize = tamanhoAtual;
        texto.ForceMeshUpdate();
    }
}
