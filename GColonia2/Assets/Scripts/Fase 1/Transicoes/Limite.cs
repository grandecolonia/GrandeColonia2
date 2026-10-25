
using UnityEngine;
using UnityEngine.SceneManagement;

public class Limite : MonoBehaviour
{
    [Header("Transição")]

    // Cena que será carregada quando o jogador atravessar o limite.
    public string cenaDestino;

    [Header("Testes")]

    // Permite trocar de cena sem precisar começar as entregas.
    public bool ignorarEntrega = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se não foi o jogador que encostou no limite, não faz nada.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Só libera a passagem se estiver em modo de teste ou se as entregas já começaram.
        if (ignorarEntrega || (ControleEntrega.instancia != null && ControleEntrega.instancia.entregaComecou))
        {
            // Carrega a cena configurada para esse limite.
            SceneManager.LoadScene(cenaDestino);
        }
    }
}
