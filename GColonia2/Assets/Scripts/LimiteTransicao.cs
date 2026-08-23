using UnityEngine;

public class LimiteTransicao : MonoBehaviour
{
    // Objeto que contém o vídeo e o script responsável pela transição.
    public GameObject transicaoVideo;

    // Impede que a transição seja iniciada mais de uma vez.
    private bool ativou = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Inicia a transição quando o jogador entra no limite.
        if (other.CompareTag("Player") && !ativou)
        {
            ativou = true;

            FadeTransicao fade = transicaoVideo.GetComponentInChildren<FadeTransicao>();

            if (fade != null)
            {
                fade.IniciarFade();
            }
        }
    }
}