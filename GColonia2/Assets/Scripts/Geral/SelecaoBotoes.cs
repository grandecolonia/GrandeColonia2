
using UnityEngine;
using UnityEngine.UI;

public class SelecaoBotoes : MonoBehaviour
{
    [Header("Botões")]

    // Guarda os botões que podem ser selecionados.
    public Image[] botoes;

    [Header("Cores")]

    // Cor dos botões que não estão selecionados.
    public Color corNormal = Color.white;

    // Cor usada para destacar o botão selecionado.
    public Color corSelecionada = Color.yellow;

    [Header("Seleção")]

    // Define qual botão começa selecionado.
    public int selecionado = 0;

    void Start()
    {
        // Já destaca o botão inicial assim que o jogo começa.
        Selecionar(selecionado);
    }

    public void Selecionar(int indice)
    {
        // Se o índice não existe na lista, não tenta selecionar nada.
        if (indice < 0 || indice >= botoes.Length)
        {
            return;
        }

        // Passa por todos os botões para atualizar suas cores.
        for (int i = 0; i < botoes.Length; i++)
        {
            // Se encontrou o botão escolhido, coloca a cor de destaque.
            if (i == indice)
            {
                botoes[i].color = corSelecionada;
            }
            else
            {
                // Os outros botões voltam para a cor normal.
                botoes[i].color = corNormal;
            }
        }
    }
}
