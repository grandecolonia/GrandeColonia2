
using UnityEngine;
using UnityEngine.UI;

public class MovimentoImagem : MonoBehaviour
{
    [Header("Animação de Zoom")]

    // Define a velocidade com que a imagem vai aumentando de tamanho.
    public float velocidadeDoZoom = 0.02f;

    // Limite de tamanho que a imagem pode alcançar durante o zoom.
    public float zoommaximo = 1.08f;

    // Guarda o tamanho original para a imagem não ficar crescendo sem parar.
    private Vector3 escalainicial;

    // Pega o componente Image que será usado na animação.
    private Image imagem;

    // Guarda qual sprite está aparecendo para perceber quando a imagem mudar.
    private Sprite imagematual;

    void Start()
    {
        // Salva o tamanho original da imagem antes de começar o zoom.
        escalainicial = transform.localScale;

        // Pega o componente Image do próprio objeto.
        imagem = GetComponent<Image>();

        // Guarda o sprite inicial para comparar com os próximos.
        imagematual = imagem.sprite;
    }

    void Update()
    {
        // Se o sprite mudou, significa que outra imagem começou a aparecer.
        if (imagem.sprite != imagematual)
        {
            // Guarda o novo sprite para não ficar reiniciando o zoom toda hora.
            imagematual = imagem.sprite;

            // Devolve a imagem ao tamanho original antes de aumentar novamente.
            transform.localScale = escalainicial;
        }

        // Calcula até que tamanho a imagem pode crescer.
        Vector3 escalamaxima = escalainicial * zoommaximo;

        // Aumenta a imagem suavemente até chegar ao limite, sem ultrapassá-lo.
        transform.localScale = Vector3.MoveTowards(transform.localScale, escalamaxima, velocidadeDoZoom * Time.deltaTime);
    }
}
