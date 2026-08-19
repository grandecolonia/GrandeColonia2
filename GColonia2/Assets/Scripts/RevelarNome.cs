using System.Collections;
using TMPro;
using UnityEngine;

public class RevelarNome : MonoBehaviour
{
    public TMP_Text texto; // variavel do tipo texto
    public float velocidade = 0.1f;
    private bool jarevelado = false;

    void Start()
    {
        texto.maxVisibleCharacters = 0; // propriedade que define a quantidade de letras a ser mostrada
    }

    public void Revelar() // metodo usado no script da abelha
    {
        if (jarevelado) // se ja foi revelado, interrompe o metodo
        {
            return;
        }
        jarevelado = true;
        StartCoroutine(EscreverNome()); // ativa a função
    }

    IEnumerator EscreverNome() // a função IEnumerator permite que você consiga dar um intervalo (tipo um timer) a qualquer momento
    {
        for (int i = 0; i <= texto.text.Length; i++) // enquanto o texto não tiver revelado todos as letras
        {
            texto.maxVisibleCharacters = i; // mostra de acordo com o indice do for
            yield return new WaitForSeconds(velocidade); // espera o tempo definido pela variavel pra revelar a proxima letra
        }
    }
}