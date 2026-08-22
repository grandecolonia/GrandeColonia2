using UnityEngine;

public class InteracaoEscada : MonoBehaviour
{
    public GameObject escada;

    void Start()
    {
       escada.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        escada.SetActive(true);
      }
    }
 
    private void OnTriggerExit2D(Collider2D other)
    {
       if (other.CompareTag("Player"))
        {
            escada.SetActive(false);
        }
    }

    public void EsconderEscada()
    {
        escada.SetActive(false);
    }
}
