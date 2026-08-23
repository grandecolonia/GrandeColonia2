using UnityEngine;

public class AnimacaoFlutuar : MonoBehaviour
{
    // Configura a altura e a velocidade do movimento de flutuação.
    public float altura = 0.15f;
    public float velocidade = 2f;

    // Guarda a posição original para o objeto não se afastar do lugar.
    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Cria um movimento suave de subida e descida.
        float movimento = Mathf.Sin(Time.time * velocidade) * altura;
        transform.localPosition = posicaoInicial + Vector3.up * movimento;
    }
}