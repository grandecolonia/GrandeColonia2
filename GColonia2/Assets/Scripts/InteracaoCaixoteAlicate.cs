using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCaixoteAlicate : MonoBehaviour
{
    [Header("Caixote")]

    public GameObject caixote;
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    public GameObject iconeE;

    [Header("Input")]

    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool coletado = false;

    void OnEnable()
    {
        if (interagir != null)
        {
            interagir.action.Enable();
        }
    }

    void Start()
    {
        iconeE.SetActive(false);
    }

    void Update()
    {
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

        // Esconde o caixote.
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
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}