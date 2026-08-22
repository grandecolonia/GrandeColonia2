using UnityEngine;
using UnityEngine.UI;

public class MovimentoImagem : MonoBehaviour
{
    public float velocidadeDoZoom = 0.02f;
    public float zoommaximo = 1.08f;
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
        if (imagem.sprite != imagematual) // Se teve alteração na imagem, reseta o zoom;
        {
            imagematual = imagem.sprite;
            transform.localScale = escalainicial;
        }

        Vector3 escalamaxima = escalainicial * zoommaximo;
        transform.localScale = Vector3.MoveTowards(transform.localScale, escalamaxima, velocidadeDoZoom * Time.deltaTime);
    }
}
