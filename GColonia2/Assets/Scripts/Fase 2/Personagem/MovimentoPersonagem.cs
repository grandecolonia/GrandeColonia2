
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovimentoPersonagem : MonoBehaviour
{
    [Header("Movimentação")]

    // Velocidade com que Maxwell anda para os lados.
    public float velocidade = 5f;

    // Força aplicada quando Maxwell pula.
    public float forcaDoPulo = 8f;

    // Rigidbody responsável pela movimentação física do personagem.
    private Rigidbody2D rb;

    // Guarda a direção horizontal que o jogador está apertando.
    private float movimento;

    // Verifica se Maxwell está encostando no chão e pode pular.
    private bool estarnochao;

    [Header("Visual")]

    // Sprite usado para virar Maxwell conforme a direção do movimento.
    public SpriteRenderer spritedopers;

    [Header("Troca de cena")]

    // Cena que será carregada quando Maxwell utilizar a escada.
    public string nomeDaCena;

    // Referência à escada que Maxwell pode utilizar.
    public InteracaoEscada escada;

    [Header("Controle")]

    // Permite que outros scripts saibam se Maxwell está no chão.
    public bool EstaNoChao => estarnochao;

    // Permite travar Maxwell durante diálogos, entregas e outros eventos.
    public bool podeMover = true;

    void Start()
    {
        // Pega o Rigidbody2D do Maxwell para controlar seu movimento.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Se outro sistema travou Maxwell, impede que ele receba comandos.
        if (!podeMover)
        {
            movimento = 0f;
            return;
        }

        // Pega o teclado usando o New Input System.
        Keyboard teclado = Keyboard.current;

        // Se não existe um teclado disponível, não tenta movimentar Maxwell.
        if (teclado == null)
        {
            movimento = 0f;
            return;
        }

        // Zera a direção para não continuar andando depois de soltar as teclas.
        movimento = 0f;

        // Se apertou A ou seta esquerda, manda Maxwell andar para a esquerda.
        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed)
        {
            movimento -= 1f;
        }

        // Se apertou D ou seta direita, manda Maxwell andar para a direita.
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed)
        {
            movimento += 1f;
        }

        // Se Maxwell está andando para a direita, vira o sprite para esse lado.
        if (movimento > 0f)
        {
            spritedopers.flipX = false;
        }
        // Se está andando para a esquerda, vira o sprite para o lado contrário.
        else if (movimento < 0f)
        {
            spritedopers.flipX = true;
        }

        // Só permite pular ao apertar Espaço enquanto Maxwell estiver no chão.
        if (teclado.spaceKey.wasPressedThisFrame && estarnochao)
        {
            // Aplica a velocidade do pulo sem alterar o movimento horizontal.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaDoPulo);
        }

        // Se apertou E perto da escada e existe uma cena configurada, libera a passagem.
        if (teclado.eKey.wasPressedThisFrame && escada != null && escada.jogadorPerto && !string.IsNullOrEmpty(nomeDaCena))
        {
            // Esconde o objeto de interação antes de sair da cena.
            escada.EsconderEscada();

            // Carrega a cena configurada para o destino da escada.
            SceneManager.LoadScene(nomeDaCena);
        }
    }

    void FixedUpdate()
    {
        // Se Maxwell estiver travado, interrompe apenas o movimento horizontal.
        if (!podeMover)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Move Maxwell para os lados sem mexer na velocidade vertical.
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se Maxwell encostou em um objeto com a tag Chao, permite o pulo.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Se Maxwell deixou de encostar no Chao, bloqueia o pulo no ar.
        if (collision.gameObject.CompareTag("Chao"))
        {
            estarnochao = false;
        }
    }
}
