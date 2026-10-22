
using UnityEngine;

public class InteracaoEscada : MonoBehaviour
{
    public GameObject escada;

    public bool jogadorPerto = false;

    void Start()
    {
        escada.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            escada.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            escada.SetActive(false);
        }
    }

    public void EsconderEscada()
    {
        escada.SetActive(false);
    }
}
