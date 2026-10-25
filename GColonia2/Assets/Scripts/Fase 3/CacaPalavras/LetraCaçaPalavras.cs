
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LetraCaçaPalavras : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    [Header("Referências")]

    // Imagem de fundo que muda de cor durante a seleção.
    public Image fundo;

    // Texto que mostra a letra dentro de cada célula.
    public TMP_Text textoLetra;

    [Header("Posição")]

    // Linha em que a letra está localizada no caça-palavras.
    public int linha;

    // Coluna em que a letra está localizada no caça-palavras.
    public int coluna;

    [Header("Estado")]

    // Impede que a letra seja utilizada novamente depois de encontrada.
    public bool bloqueada = false;

    // Sistema responsável por controlar as palavras e a seleção das letras.
    private GerenciadorCaçaPalavras gerenciador;

    public void Configurar(char letra, int novaLinha, int novaColuna, GerenciadorCaçaPalavras novoGerenciador)
    {
        // Mostra a letra recebida dentro da célula.
        textoLetra.text = letra.ToString();

        // Guarda a posição da letra na grade.
        linha = novaLinha;
        coluna = novaColuna;

        // Guarda a referência do gerenciador do caça-palavras.
        gerenciador = novoGerenciador;

        // Começa com a cor normal configurada no gerenciador.
        fundo.color = gerenciador.corNormal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Se a letra já foi encontrada, impede uma nova seleção.
        if (bloqueada)
        {
            return;
        }

        // Avisa ao gerenciador que o jogador começou a selecionar por esta letra.
        gerenciador.ComecarSelecao(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Se a letra está bloqueada, não permite incluí-la na seleção.
        if (bloqueada)
        {
            return;
        }

        // Avisa ao gerenciador que o mouse passou por esta letra.
        gerenciador.PassarPorLetra(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Avisa ao gerenciador que o jogador soltou o botão do mouse.
        gerenciador.TerminarSelecao();
    }

    public void MudarCor(Color novaCor)
    {
        // Altera a cor do fundo conforme o resultado da seleção.
        fundo.color = novaCor;
    }

    public void Bloquear()
    {
        // Marca que esta letra já pertence a uma palavra encontrada.
        bloqueada = true;

        // Mantém a letra com a cor correta para mostrar que foi encontrada.
        fundo.color = gerenciador.corCorreta;
    }
}
