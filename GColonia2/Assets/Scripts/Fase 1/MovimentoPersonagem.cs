
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidade = 5f;
    public float forcaDoPulo = 8f;

    private Rigidbody2D rb;
    private float movimento;
    private bool estarnochao;

    public SpriteRenderer spritedopers;

    [Header("Troca de cena")]
    public string nomeDaCena;
    public InteracaoEscada escada;

    public bool EstaNoChao => estarnochao;
    public bool podeMover = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!podeMover)
        {
            movimento = 0;
            return;
        }

        Keyboard teclado = Keyboard.current;

        if (teclado == null)
        {
            movimento = 0;
            return;
        }

        movimento = 0;

        // Movimento horizontal.
        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed)
        {
            movimento -= 1;
        }

        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed)
        {
            movimento += 1;
        }

        // Vira o Maxwell conforme a direção.
        if (movimento > 0)
        {
            spritedopers.flipX = false;
        }
        else if (movimento < 0)
        {
            spritedopers.flipX = true;
        }

        // Pulo.
        if (teclado.spaceKey.wasPressedThisFrame && estarnochao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }

        // Troca de cena somente perto da escada.
        if (teclado.eKey.wasPressedThisFrame && escada != null && escada.jogadorPerto && !string.IsNullOrEmpty(nomeDaCena))
        {
            escada.EsconderEscada();
            SceneManager.LoadScene(nomeDaCena);
        }
    }

    void FixedUpdate()
    { 
        if (!podeMover)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

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
