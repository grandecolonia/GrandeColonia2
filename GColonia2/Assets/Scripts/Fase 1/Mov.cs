using UnityEngine;

public class Mov : MonoBehaviour
{
    // Configura a velocidade horizontal e a força aplicada no pulo.
    public float velocidade = 5f;
    public float forcaPulo = 10f;

    // Componente usado para controlar o movimento físico do personagem.
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Aplica o movimento horizontal de acordo com a tecla pressionada.
        float movimento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);

        // Aplica a força vertical quando o jogador aperta Espaço.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
}