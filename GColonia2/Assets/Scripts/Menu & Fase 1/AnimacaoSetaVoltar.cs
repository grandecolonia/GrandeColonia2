using UnityEngine;

public class AnimacaoSetaVoltar : MonoBehaviour
{
    // Configura o tamanho, a distância e a velocidade da animação.
    public float aumento = 1.1f;
    public float distanciaEsquerda = 20f;
    public float velocidade = 2f;

    // Guardam a escala e a posição originais da seta.
    private Vector3 escalaInicial;
    private Vector3 posicaoInicial;

    void Start()
    {
        escalaInicial = transform.localScale;
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Cria um movimento suave que varia entre 0 e 1.
        float movimento = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;

        // Faz a seta aumentar e diminuir suavemente.
        transform.localScale = escalaInicial * Mathf.Lerp(1f, aumento, movimento);

        // Move a seta para a esquerda e depois retorna à posição inicial.
        float deslocamento = Mathf.Lerp(0f, distanciaEsquerda, movimento);
        transform.localPosition = posicaoInicial + Vector3.left * deslocamento;
    }
}