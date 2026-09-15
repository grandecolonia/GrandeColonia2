using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCartao : MonoBehaviour
{
    [Header("Cartão")]

    // Cartão da cena e controle geral do labirinto.
    public GameObject cartao;
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode pegar o cartão.
    public GameObject iconeE;

    [Header("Input")]

    // Action usada para interagir com o cartão.
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
        // Tenta coletar o cartão quando Maxwell estiver na área e apertar o comando.
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

        // Guarda que o cartão foi coletado e começa o alerta.
        coletado = true;
        controleLabirinto.temCartao = true;
        controleLabirinto.alertaAtivo = true;

        iconeE.SetActive(false);

        // Remove o cartão da cena.
        cartao.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Guarda que Maxwell entrou na área do cartão.
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;

            // Só mostra o ícone se a energia já estiver ligada.
            if (controleLabirinto.energiaLigada)
            {
                iconeE.SetActive(true);
            }
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