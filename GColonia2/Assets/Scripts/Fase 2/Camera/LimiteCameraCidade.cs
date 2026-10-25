
using UnityEngine;

public class LimiteCameraCidade : MonoBehaviour
{
    [Header("Câmera")]

    // Sistema responsável por trocar as visões da cidade.
    public ControleCameraCidade controleCamera;

    [Header("Regiões")]

    // Visão que será mostrada quando Maxwell atravessar o limite pela esquerda.
    public int visaoEsquerda;

    // Visão que será mostrada quando Maxwell atravessar o limite pela direita.
    public int visaoDireita;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se não foi Maxwell que saiu da área, não precisa trocar a câmera.
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // Descobre se Maxwell saiu pelo lado direito ou esquerdo do limite.
        if (collision.bounds.center.x > transform.position.x)
        {
            // Se saiu pela direita, muda para a visão configurada desse lado.
            controleCamera.MudarVisao(visaoDireita);
        }
        else
        {
            // Se saiu pela esquerda, volta para a visão correspondente.
            controleCamera.MudarVisao(visaoEsquerda);
        }
    }
}
