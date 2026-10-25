
using UnityEngine;
using TMPro;

public class AplicarTamanhoCutscene : MonoBehaviour
{
    // Guarda o texto que vai ter seu tamanho ajustado.
    private TMP_Text texto;

    // Guarda o último tamanho aplicado para não ficar atualizando à toa.
    private float tamanhoAtual;

    void Awake()
    {
        // Pega o componente de texto do próprio objeto.
        texto = GetComponent<TMP_Text>();

        // Já coloca o tamanho configurado assim que o objeto inicia.
        AtualizarTamanho();
    }

    void Update()
    {
        // Só mexe no texto quando o jogador realmente mudar o tamanho nas configurações.
        if (tamanhoAtual != ConfiguracoesJogo.ObterTamanhoTexto())
        {
            AtualizarTamanho();
        }
    }

    void AtualizarTamanho()
    {
        // Busca o tamanho que está salvo nas configurações do jogo.
        tamanhoAtual = ConfiguracoesJogo.ObterTamanhoTexto();

        // Aplica o tamanho escolhido ao texto da cutscene.
        texto.fontSize = tamanhoAtual;

        // Força o TextMeshPro a atualizar o texto com o novo tamanho.
        texto.ForceMeshUpdate();
    }
}
