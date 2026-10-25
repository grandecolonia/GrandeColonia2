
using UnityEngine;
using UnityEngine.SceneManagement;

public class LimiteTransicao : MonoBehaviour
{
    [Header("Troca de cena")]

    // Nome da cena que será carregada quando o jogador chegar ao limite.
    public string nomeDaCena;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se foi o jogador que chegou ao limite e existe uma cena configurada, permite a troca.
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(nomeDaCena))
        {
            // Carrega a cena escolhida no Inspector.
            SceneManager.LoadScene(nomeDaCena);
        }
    }
}
