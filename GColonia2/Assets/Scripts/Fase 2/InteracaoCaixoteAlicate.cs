using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCaixoteAlicate : MonoBehaviour
{
    [Header("Caixote")]

    // Caixote usado na interação e controle geral do labirinto.
    public GameObject caixote;
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode interagir.
    public GameObject iconeE;

    [Header("Input")]

    // Action usada para interagir com o caixote.
    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool coletado = false;

    void OnEnable()
    {
        // Ativa a Action usada na interação.
        if (interagir != null)
        {
            interagir.action.Enable();
        }
    }

    void Start()
    {
        // Começa com o ícone escondido.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Coleta o alicate quando Maxwell estiver na área e apertar o comando.
        if (jogadorNaArea && !coletado && interagir.action.WasPressedThisFrame())
        {
            ColetarAlicate();
        }
    }

    void ColetarAlicate()
    {
        coletado = true;

        // Maxwell passa a possuir o alicate.
        controleLabirinto.temAlicate = true;

        iconeE.SetActive(false);

        // Esconde o visual do caixote.
        SpriteRenderer spriteCaixote = caixote.GetComponent<SpriteRenderer>();

        if (spriteCaixote != null)
        {
            spriteCaixote.enabled = false;
        }

        // Remove o bloqueio físico do caixote.
        BoxCollider2D colliderCaixote = caixote.GetComponent<BoxCollider2D>();

        if (colliderCaixote != null)
        {
            colliderCaixote.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostra o ícone quando Maxwell entra na área e o item ainda não foi coletado.
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Esconde o ícone quando Maxwell sai da área.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}