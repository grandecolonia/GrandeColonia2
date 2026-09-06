using UnityEngine;
using UnityEngine.UI;

public class MovimentoImagem : MonoBehaviour
{
    // Configura a velocidade e o limite do efeito de zoom.
    public float velocidadeDoZoom = 0.02f;
    public float zoommaximo = 1.08f;

    // Guardam os dados necessários para controlar a imagem e sua escala.
    private Vector3 escalainicial;
    private Image imagem;
    private Sprite imagematual;

    void Start()
    {
        escalainicial = transform.localScale;
        imagem = GetComponent<Image>();
        imagematual = imagem.sprite;
    }

    void Update()
    {
        // Reinicia o zoom quando a imagem exibida é alterada.
        if (imagem.sprite != imagematual)
        {
            imagematual = imagem.sprite;
            transform.localScale = escalainicial;
        }

        // Aumenta a imagem suavemente até alcançar o zoom máximo.
        Vector3 escalamaxima = escalainicial * zoommaximo;
        transform.localScale = Vector3.MoveTowards(transform.localScale, escalamaxima, velocidadeDoZoom * Time.deltaTime);
    }
}