
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscadaFinal : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform destino;
    public float velocidade = 2f;
    private bool subindo = false;

    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player") && !subindo)
        {
            StartCoroutine(SubirEscada());
        }
    }

    IEnumerator SubirEscada()
    {
        subindo = true;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.simulated = false;

        while (Vector3.Distance(rb.transform.position, destino.position) > 0.05f)
        {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, destino.position, velocidade * Time.deltaTime);
            yield return null;
        }

        rb.transform.position = destino.position;

        SceneManager.LoadScene("19_Portão");
    }
}
