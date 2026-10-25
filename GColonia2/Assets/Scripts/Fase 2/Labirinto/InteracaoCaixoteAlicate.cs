
using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCaixoteAlicate : MonoBehaviour
{
    [Header("Caixote")]

    // Objeto do caixote que vai perder o visual e a colisão depois da coleta.
    public GameObject caixote;

    // Objeto do alicate que será escondido quando Maxwell pegar o item.
    public GameObject alicate;

    // Sistema que guarda quais itens Maxwell já conseguiu.
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode interagir com o caixote.
    public GameObject iconeE;

    [Header("Input")]

    // Ação usada para coletar o alicate.
    public InputActionReference interagir;

    // Guarda se Maxwell está dentro da área de interação.
    private bool jogadorNaArea = false;

    // Impede que o alicate seja coletado mais de uma vez.
    private bool coletado = false;

    void Start()
    {
        // Começa com o ícone de interação escondido.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell está perto, ainda não pegou o alicate e apertou o comando, faz a coleta.
        if (jogadorNaArea && !coletado && interagir.action.WasPressedThisFrame())
        {
            ColetarAlicate();
        }
    }

    void ColetarAlicate()
    {
        // Marca que o alicate já foi coletado para impedir outra interação.
        coletado = true;

        // Avisa ao sistema do labirinto que Maxwell agora possui o alicate.
        controleLabirinto.temAlicate = true;

        // Esconde o ícone porque não existe mais nada para pegar.
        iconeE.SetActive(false);

        // Faz o alicate desaparecer depois que Maxwell pega o item.
        alicate.SetActive(false);

        // Procura o SpriteRenderer do caixote para esconder sua imagem.
        SpriteRenderer spriteCaixote = caixote.GetComponent<SpriteRenderer>();

        // Se encontrou o sprite, desliga a parte visual do caixote.
        if (spriteCaixote != null)
        {
            spriteCaixote.enabled = false;
        }

        // Procura o Collider2D para remover a colisão do caixote.
        Collider2D colliderCaixote = caixote.GetComponent<Collider2D>();

        // Se encontrou o Collider, desativa para não bloquear Maxwell.
        if (colliderCaixote != null)
        {
            colliderCaixote.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e ainda não pegou o alicate, libera a interação.
        if (collision.CompareTag("Player") && !coletado)
        {
            // Marca que Maxwell está perto do caixote.
            jogadorNaArea = true;

            // Mostra o ícone para avisar que ele pode coletar o item.
            iconeE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Quando Maxwell se afasta do caixote, cancela a interação.
        if (collision.CompareTag("Player"))
        {
            // Marca que o jogador saiu da área.
            jogadorNaArea = false;

            // Esconde o ícone de interação.
            iconeE.SetActive(false);
        }
    }
}
