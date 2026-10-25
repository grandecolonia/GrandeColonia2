
using UnityEngine;

public class ParallaxFase2 : MonoBehaviour
{
    [Header("Partes")]

    // Primeira parte do cenário que ficará se movimentando.
    public Transform parte1;

    // Segunda parte que continua o cenário quando a primeira sai da tela.
    public Transform parte2;

    [Header("Movimento")]

    // Velocidade com que o cenário se movimenta para a esquerda.
    public float velocidade = 0.5f;

    // Pequena sobreposição para evitar espaços entre as duas imagens.
    public float sobreposicao = 0.05f;

    // Guarda a largura da primeira parte do cenário.
    private float largura;

    // Distância usada para posicionar uma parte depois da outra.
    private float distancia;

    // Câmera utilizada para verificar quando o cenário saiu da tela.
    private Camera cameraPrincipal;

    void Start()
    {
        // Encontra a câmera principal da cena.
        cameraPrincipal = Camera.main;

        // Descobre a largura real do sprite no cenário.
        largura = parte1.GetComponent<SpriteRenderer>().bounds.size.x;

        // Calcula a distância entre as partes, considerando a sobreposição.
        distancia = largura - sobreposicao;

        // Coloca a segunda parte logo depois da primeira.
        parte2.position = new Vector3(parte1.position.x + distancia, parte1.position.y, parte1.position.z);
    }

    void Update()
    {
        // Se não encontrou uma câmera principal, não movimenta o cenário.
        if (cameraPrincipal == null)
        {
            return;
        }

        // Calcula quanto o cenário deve se mover neste frame.
        float movimento = velocidade * Time.deltaTime;

        // Move as duas partes para a esquerda.
        parte1.position += Vector3.left * movimento;
        parte2.position += Vector3.left * movimento;

        // Descobre a posição da borda esquerda da câmera.
        float limiteEsquerdo = cameraPrincipal.transform.position.x - cameraPrincipal.orthographicSize * cameraPrincipal.aspect;

        // Se a primeira parte saiu completamente da tela, coloca ela depois da segunda.
        if (parte1.position.x + largura / 2f < limiteEsquerdo)
        {
            parte1.position = new Vector3(parte2.position.x + distancia, parte1.position.y, parte1.position.z);
        }

        // Se a segunda parte saiu completamente da tela, coloca ela depois da primeira.
        if (parte2.position.x + largura / 2f < limiteEsquerdo)
        {
            parte2.position = new Vector3(parte1.position.x + distancia, parte2.position.y, parte2.position.z);
        }
    }
}
