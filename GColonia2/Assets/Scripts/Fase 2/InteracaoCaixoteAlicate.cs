using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCaixoteAlicate : MonoBehaviour
{
    [Header("Caixote")]
    public GameObject caixote;
    public GameObject alicate;
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]
    public GameObject iconeE;

    [Header("Input")]
    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool coletado = false;

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
        controleLabirinto.temAlicate = true;

        iconeE.SetActive(false);

        // Esconde o alicate depois da coleta.
        alicate.SetActive(false);

        // Remove o visual e a colisão do caixote.
        SpriteRenderer spriteCaixote = caixote.GetComponent<SpriteRenderer>();

        if (spriteCaixote != null)
        {
            spriteCaixote.enabled = false;
        }

        Collider2D colliderCaixote = caixote.GetComponent<Collider2D>();

        if (colliderCaixote != null)
        {
            colliderCaixote.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}