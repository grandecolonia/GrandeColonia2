using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float velocidade = 0.01f;
    public Renderer quad;

    void Update()
    {
        quad.material.mainTextureOffset += new Vector2(velocidade * Time.deltaTime, 0);
        quad.material.mainTextureOffset = new Vector2(quad.material.mainTextureOffset.x, 0);
    }
}