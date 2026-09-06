using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    // Configura o movimento horizontal do personagem.
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private float movimento;

    // Componente visual usado para trocar a direção e o sprite do personagem.
    public SpriteRenderer spritedopers;

    // Configura o pulo e verifica se o personagem está no chão.
    public float forcaDoPulo = 8f;

    private bool estarnochao;

    // Referências usadas durante a interação com a escada.
    public FadeTransicao fade;
    public InteracaoEscada escada;

    // Permite que outros scripts bloqueiem ou liberem o movimento.
    public bool podeMover = true;

    // Sprites usados nas cenas em que o personagem começa olhando para a frente.
    public Sprite spriteFrente;
    public Sprite spriteLado;
    public bool comecaDeFrente = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Coloca o sprite frontal quando essa opção estiver ativada.
        if (comecaDeFrente)
        {
            spritedopers.sprite = spriteFrente;
        }
    }

    void Update()
    {
        // Interrompe a leitura dos comandos quando o movimento está bloqueado.
        if (!podeMover)
        {
            movimento = 0;
            return;
        }

        movimento = Input.GetAxisRaw("Horizontal");

        // Troca o sprite frontal pelo lateral quando o personagem começa a andar.
        if (comecaDeFrente && movimento != 0)
        {
            spritedopers.sprite = spriteLado;
            comecaDeFrente = false;
        }

        // Vira o sprite de acordo com a direção do movimento.
        if (movimento > 0)
        {
            spritedopers.flipX = true;
        }
        else if (movimento < 0)
        {
            spritedopers.flipX = false;
        }

        // Permite o pulo somente quando o personagem está no chão.
        if (Input.GetKeyDown(KeyCode.Space) && estarnochao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }

        // Inicia a transição relacionada à escada quando o jogador aperta E.
        if (Input.GetKeyDown(KeyCode.E))
        {
            escada.EsconderEscada();
            fade.IniciarFade();
        }
    }

    void FixedUpdate()
    {
        // Interrompe o movimento horizontal quando o personagem está bloqueado.
        if (!podeMover)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Marca que o personagem tocou o chão.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Marca que o personagem deixou de tocar o chão.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = false;
        }
    }
}