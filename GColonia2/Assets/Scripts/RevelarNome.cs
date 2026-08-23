using System.Collections;
using TMPro;
using UnityEngine;

public class RevelarNome : MonoBehaviour
{
    // Elementos usados para controlar a revelação do nome.
    public TMP_Text texto;
    public float velocidade = 0.1f;

    // Impede que o mesmo nome seja revelado mais de uma vez.
    private bool jarevelado = false;

    void Start()
    {
        // Começa com todas as letras do nome escondidas.
        texto.maxVisibleCharacters = 0;
    }

    public void Revelar()
    {
        // Interrompe o método caso o nome já tenha sido revelado.
        if (jarevelado)
        {
            return;
        }

        jarevelado = true;
        StartCoroutine(EscreverNome());
    }

    IEnumerator EscreverNome()
    {
        // Mostra as letras uma por uma, respeitando a velocidade configurada.
        for (int i = 0; i <= texto.text.Length; i++)
        {
            texto.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidade);
        }
    }
}