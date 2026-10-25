
using UnityEngine;

public class AnimacaoFlutuar : MonoBehaviour
{
    [Header("Flutuação")]

    // Define o quanto o objeto vai subir e descer.
    public float altura = 0.15f;

    // Controla a velocidade dessa flutuação.
    public float velocidade = 2f;

    // Guarda a posição original para o objeto não sair flutuando por aí.
    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva a posição inicial do objeto antes de começar a flutuar.
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Usa o seno para fazer o objeto subir e descer suavemente.
        float movimento = Mathf.Sin(Time.time * velocidade) * altura;

        // Aplica a flutuação sem perder a posição original.
        transform.localPosition = posicaoInicial + Vector3.up * movimento;
    }
}
