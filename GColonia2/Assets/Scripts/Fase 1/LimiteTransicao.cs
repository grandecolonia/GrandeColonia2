
using UnityEngine;
using UnityEngine.SceneManagement;

public class LimiteTransicao : MonoBehaviour
{
    [Header("Troca de cena")]
    public string nomeDaCena;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Troca de cena quando o Maxwell chega ao limite.
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(nomeDaCena))
        {
            SceneManager.LoadScene(nomeDaCena);
        }
    }
}
