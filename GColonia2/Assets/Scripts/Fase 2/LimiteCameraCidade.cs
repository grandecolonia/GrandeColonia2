using UnityEngine;

public class LimiteCameraCidade : MonoBehaviour
{
    [Header("Câmera")]
    public ControleCameraCidade controleCamera;

    [Header("Regiões")]
    public int visaoEsquerda;
    public int visaoDireita;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // Verifica para qual lado Maxwell saiu do limite.
        if (collision.bounds.center.x > transform.position.x)
        {
            controleCamera.MudarVisao(visaoDireita);
        }
        else
        {
            controleCamera.MudarVisao(visaoEsquerda);
        }
    }
}