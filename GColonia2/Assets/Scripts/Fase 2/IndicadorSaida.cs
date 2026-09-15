using UnityEngine;

public class IndicadorSaida : MonoBehaviour
{
    [Header("Animação")]

    // Distância que o indicador sobe e desce.
    public float distancia = 0.5f;

    // Velocidade do movimento.
    public float velocidade = 4f;

    private Vector3 posicaoInicial;

    void OnEnable()
    {
        // Guarda a posição inicial sempre que o objeto for ativado.
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Cria o movimento suave de subida e descida.
        float movimentoY = Mathf.Sin(Time.time * velocidade) * distancia;

        transform.localPosition = new Vector3(posicaoInicial.x, posicaoInicial.y + movimentoY, posicaoInicial.z);
    }
}