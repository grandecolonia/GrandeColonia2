using UnityEngine;
using System.Collections;

public class TremidaMenu : MonoBehaviour
{
    // Configura o tempo e a força do efeito de tremida.
    public float duracao = 0.25f;
    public float intensidade = 12f;

    // Guardam o componente do menu e sua posição original.
    private RectTransform rectTransform;
    private Vector2 posicaoInicial;

    void OnEnable()
    {
        // Atualiza a posição inicial sempre que o menu é ativado.
        rectTransform = GetComponent<RectTransform>();
        posicaoInicial = rectTransform.anchoredPosition;
    }

    public void Tremer()
    {
        // Interrompe uma tremida anterior antes de iniciar outra.
        StopAllCoroutines();
        StartCoroutine(TremerMenu());
    }

    IEnumerator TremerMenu()
    {
        float tempo = 0f;

        // Move o menu rapidamente apenas no eixo horizontal.
        while (tempo < duracao)
        {
            float movimentoX = Random.Range(-intensidade, intensidade);
            rectTransform.anchoredPosition = posicaoInicial + new Vector2(movimentoX, 0f);

            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // Devolve o menu exatamente para a posição original.
        rectTransform.anchoredPosition = posicaoInicial;
    }
}