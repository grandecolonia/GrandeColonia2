using UnityEngine;

public class Mov : MonoBehaviour
{
    public float velocidade = 5f;
    public float forcaPulo = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimento horizontal
        float movimento = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
}