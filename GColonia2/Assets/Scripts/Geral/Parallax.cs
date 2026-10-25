
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Parallax")]

    // Velocidade com que a textura do cenário vai se deslocar.
    public float velocidade = 0.01f;

    // Objeto que possui o material usado para movimentar a textura.
    public Renderer quad;

    void Update()
    {
        // Move a textura para o lado, criando a impressão de que o cenário está andando.
        quad.material.mainTextureOffset += new Vector2(velocidade * Time.deltaTime, 0f);

        // Mantém o deslocamento vertical zerado para o cenário não subir nem descer.
        quad.material.mainTextureOffset = new Vector2(quad.material.mainTextureOffset.x, 0f);
    }
}
