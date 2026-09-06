using UnityEngine;
using UnityEngine.SceneManagement;

public class Limite : MonoBehaviour
{
    // Cena que será carregada quando o jogador atravessar este limite.
    public string cenaDestino;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora qualquer objeto que não seja o jogador.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // O limite só funciona depois que a fase de entregas começou.
        if (ControleEntrega.instancia != null && ControleEntrega.instancia.entregaComecou)
        {
            SceneManager.LoadScene(cenaDestino);
        }
    }
}