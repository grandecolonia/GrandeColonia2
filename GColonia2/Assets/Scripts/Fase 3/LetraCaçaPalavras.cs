using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LetraCaçaPalavras : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    [Header("Referências")]

    // Fundo usado para mostrar vermelho ou verde durante a seleção.
    public Image fundo;

    // Texto que mostra a letra da célula.
    public TMP_Text textoLetra;

    [Header("Posição")]

    public int linha;
    public int coluna;

    [Header("Estado")]

    public bool bloqueada = false;

    private GerenciadorCaçaPalavras gerenciador;

    public void Configurar(char letra, int novaLinha, int novaColuna, GerenciadorCaçaPalavras novoGerenciador)
    {
        textoLetra.text = letra.ToString();

        linha = novaLinha;
        coluna = novaColuna;
        gerenciador = novoGerenciador;

        fundo.color = gerenciador.corNormal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bloqueada)
        {
            return;
        }

        gerenciador.ComecarSelecao(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bloqueada)
        {
            return;
        }

        gerenciador.PassarPorLetra(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gerenciador.TerminarSelecao();
    }

    public void MudarCor(Color novaCor)
    {
        fundo.color = novaCor;
    }

    public void Bloquear()
    {
        bloqueada = true;
        fundo.color = gerenciador.corCorreta;
    }
}