using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidade = 5f;
    private Rigidbody2D rb;
    private float movimento;
    public SpriteRenderer spritedopers;

    public float forcaDoPulo = 8f;
    private bool estarnochao;
    public FadeTransicao fade;
    public InteracaoEscada escada;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimento = Input.GetAxisRaw("Horizontal");

         if (movimento > 0)
         {
            spritedopers.flipX = true;
         }
         else if (movimento < 0)
         {
            spritedopers.flipX = false;
         }

         if (Input.GetKeyDown(KeyCode.Space) && estarnochao)
         {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
         }

          if (Input.GetKeyDown(KeyCode.E))
        {
            escada.EsconderEscada();
            fade.IniciarFade();
        }
    }

     void FixedUpdate()
     {
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
     }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = false;
        }
    }
}
