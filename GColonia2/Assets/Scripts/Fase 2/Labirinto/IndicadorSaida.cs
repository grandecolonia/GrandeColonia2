
using UnityEngine;

public class IndicadorSaida : MonoBehaviour
{
    [Header("Animação")]

    // Distância que o indicador vai subir e descer.
    public float distancia = 0.5f;

    // Controla a velocidade com que o indicador fica flutuando.
    public float velocidade = 4f;

    // Guarda a posição original para o indicador não sair do lugar.
    private Vector3 posicaoInicial;

    void OnEnable()
    {
        // Salva a posição do indicador sempre que ele for ativado.
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Usa o seno para fazer o indicador subir e descer suavemente.
        float movimentoY = Mathf.Sin(Time.time * velocidade) * distancia;

        // Aplica o movimento vertical sem alterar a posição horizontal ou a profundidade.
        transform.localPosition = new Vector3(posicaoInicial.x, posicaoInicial.y + movimentoY, posicaoInicial.z);
    }
}
