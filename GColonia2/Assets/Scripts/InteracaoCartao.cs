using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCartao : MonoBehaviour
{
    [Header("Cartão")]

    public GameObject cartao;
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
            ColetarCartao();
        }
    }

    void ColetarCartao()
    {
        // O cartão só pode ser pego depois que a energia estiver ligada.
        if (!controleLabirinto.energiaLigada)
        {
            return;
        }

        coletado = true;
        controleLabirinto.temCartao = true;

        controleLabirinto.alertaAtivo = true;

        iconeE.SetActive(false);

        // Remove o cartão da cena.
        cartao.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;

            if (controleLabirinto.energiaLigada)
            {
                iconeE.SetActive(true);
            }
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