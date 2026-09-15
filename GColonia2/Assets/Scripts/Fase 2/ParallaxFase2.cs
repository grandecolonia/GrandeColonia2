using UnityEngine;

public class ParallaxFase2 : MonoBehaviour
{
    [Header("Partes")]

    // Duas partes iguais do cenário usadas para repetir o parallax.
    public Transform parte1;
    public Transform parte2;

    [Header("Movimento")]

    // Velocidade que as partes andam para a esquerda.
    public float velocidade = 0.5f;

    // Pequena sobreposição para evitar frestas entre as imagens.
    public float sobreposicao = 0.05f;

    private float largura;

    void Start()
    {
        // Pega a largura da primeira parte para posicionar a segunda ao lado.
        largura = parte1.GetComponent<SpriteRenderer>().bounds.size.x;

        parte2.position = new Vector3(parte1.position.x + largura - sobreposicao, parte1.position.y, parte1.position.z);
    }

    void Update()
    {
        float movimento = velocidade * Time.deltaTime;

        // Move as duas partes juntas para a esquerda.
        parte1.position += Vector3.left * movimento;
        parte2.position += Vector3.left * movimento;

        // Quando a primeira parte sair, ela volta para depois da segunda.
        if (parte1.position.x <= parte2.position.x - largura)
        {
            parte1.position = new Vector3(parte2.position.x + largura - sobreposicao, parte1.position.y, parte1.position.z);
        }

        // Faz o mesmo com a segunda parte.
        if (parte2.position.x <= parte1.position.x - largura)
        {
            parte2.position = new Vector3(parte1.position.x + largura - sobreposicao, parte2.position.y, parte2.position.z);
        }
    }
}