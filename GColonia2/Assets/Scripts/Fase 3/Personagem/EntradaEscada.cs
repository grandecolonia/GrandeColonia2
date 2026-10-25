
using UnityEngine;

public class EntradaEscada : MonoBehaviour
{
    [Header("Escada")]

    // Sistema responsável por controlar a subida e a descida.
    public SistemaEscada escada;

    void OnTriggerEnter2D(Collider2D colisao)
    {
        // Se Maxwell entrou na área, informa ao sistema da escada.
        if (colisao.CompareTag("Player"))
        {
            escada.EntrarNaArea(colisao);
        }
    }

    void OnTriggerExit2D(Collider2D colisao)
    {
        // Se Maxwell saiu da área, cancela a possibilidade de usar a escada.
        if (colisao.CompareTag("Player"))
        {
            escada.SairDaArea();
        }
    }
}
