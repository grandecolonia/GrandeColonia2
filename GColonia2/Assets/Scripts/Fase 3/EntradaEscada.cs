
using UnityEngine;

public class EntradaEscada : MonoBehaviour
{
    public SistemaEscada escada;

    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player"))
        {
            escada.EntrarNaArea(colisao);
        }
    }

    void OnTriggerExit2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player"))
        {
            escada.SairDaArea();
        }
    }
}
