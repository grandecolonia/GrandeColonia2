using UnityEngine;
using UnityEngine.UI;

public class SelecaoBotoes : MonoBehaviour
{
    public Image[] botoes;
    public Color corNormal = Color.white;
    public Color corSelecionada = Color.yellow;
    public int selecionado = 0;
    void Start()
    {
        Selecionar(selecionado); // seleciona o botão inicial ao iniciar o jogo.
    }

    public void Selecionar(int indice)
    {
        if (indice < 0 || indice >= botoes.Length)
            return;

        for (int i = 0; i < botoes.Length; i++)
        {
            if (i == indice) // verifica indice passado pôr parâmetro e muda a cor do botão selecionado.
            {
               botoes[i].color = corSelecionada;
            }
            else
            {
                botoes[i].color = corNormal;
            }
        }
        
    }
}
