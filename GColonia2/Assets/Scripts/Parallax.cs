using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Configura a velocidade do movimento do cenário e o objeto que recebe o efeito.
    public float velocidade = 0.01f;
    public Renderer quad;

    void Update()
    {
        // Move a textura horizontalmente para criar o efeito de parallax.
        quad.material.mainTextureOffset += new Vector2(velocidade * Time.deltaTime, 0);

        // Mantém a posição vertical da textura sempre em zero.
        quad.material.mainTextureOffset = new Vector2(quad.material.mainTextureOffset.x, 0);
    }
}