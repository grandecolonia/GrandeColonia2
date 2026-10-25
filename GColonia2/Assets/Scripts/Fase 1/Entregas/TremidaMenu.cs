
using UnityEngine;
using System.Collections;

public class TremidaMenu : MonoBehaviour
{
    [Header("Efeito de Tremida")]

    // Tempo que o menu vai ficar tremendo depois de uma escolha errada.
    public float duracao = 0.25f;

    // Controla a força da tremida, ou seja, o quanto o menu vai balançar.
    public float intensidade = 12f;

    // Guarda o RectTransform para conseguir movimentar o menu.
    private RectTransform rectTransform;

    // Guarda a posição original para o menu voltar ao lugar depois da tremida.
    private Vector2 posicaoInicial;

    void OnEnable()
    {
        // Pega o RectTransform do próprio objeto quando o menu é ativado.
        rectTransform = GetComponent<RectTransform>();

        // Salva a posição atual para não deixar o menu deslocado depois da animação.
        posicaoInicial = rectTransform.anchoredPosition;
    }

    public void Tremer()
    {
        // Interrompe qualquer tremida anterior para não misturar duas animações.
        StopAllCoroutines();

        // Começa uma nova tremida no menu.
        StartCoroutine(TremerMenu());
    }

    IEnumerator TremerMenu()
    {
        // Começa a contar o tempo da animação.
        float tempo = 0f;

        // Continua tremendo até completar a duração configurada.
        while (tempo < duracao)
        {
            // Sorteia um deslocamento para fazer o menu balançar de um lado para o outro.
            float movimentoX = Random.Range(-intensidade, intensidade);

            // Move o menu somente na horizontal, sem alterar sua altura.
            rectTransform.anchoredPosition = posicaoInicial + new Vector2(movimentoX, 0f);

            // Conta o tempo mesmo se o jogo estiver pausado.
            tempo += Time.unscaledDeltaTime;

            // Espera o próximo frame para continuar a tremida.
            yield return null;
        }

        // Terminou a animação, então devolve o menu exatamente ao lugar original.
        rectTransform.anchoredPosition = posicaoInicial;
    }
}
