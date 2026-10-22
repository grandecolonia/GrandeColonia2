using UnityEngine;
using UnityEngine.SceneManagement;

public class Limite : MonoBehaviour
{
    public string cenaDestino;

    [Header("Testes")]
    public bool ignorarEntrega = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (ignorarEntrega || (ControleEntrega.instancia != null && ControleEntrega.instancia.entregaComecou))
        {
            SceneManager.LoadScene(cenaDestino);
        }
    }
}