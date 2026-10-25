
using UnityEngine;

public class AnimacaoSetaVoltar : MonoBehaviour
{
    [Header("Animação")]

    // Define o quanto a seta vai aumentar de tamanho.
    public float aumento = 1.1f;

    // Distância que a seta vai andar para a esquerda.
    public float distanciaEsquerda = 20f;

    // Controla a velocidade da animação.
    public float velocidade = 2f;

    // Guardam o tamanho e a posição originais da seta.
    private Vector3 escalaInicial;
    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva o tamanho original para a seta não ficar crescendo sem parar.
        escalaInicial = transform.localScale;

        // Guarda a posição inicial para a seta sempre voltar ao mesmo lugar.
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Usa o seno para criar uma animação suave que vai de 0 até 1.
        float movimento = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;

        // Aumenta e diminui a seta sem perder seu tamanho original.
        transform.localScale = escalaInicial * Mathf.Lerp(1f, aumento, movimento);

        // Calcula o quanto a seta vai se deslocar para a esquerda.
        float deslocamento = Mathf.Lerp(0f, distanciaEsquerda, movimento);

        // Move a seta para a esquerda e depois traz de volta ao lugar.
        transform.localPosition = posicaoInicial + Vector3.left * deslocamento;
    }
}
