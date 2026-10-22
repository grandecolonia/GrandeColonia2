using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoBotaoEnergia : MonoBehaviour
{
    [Header("Labirinto")]

    // Controle geral do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que mostra quando o jogador pode interagir.
    public GameObject iconeE;

    private bool jogadorNaArea = false;
    private bool ativado = false;

    [Header("Input")]
public InputActionReference interagir;

    void Start()
    {
        // Começa com o ícone escondido.
        iconeE.SetActive(false);
    }

void Update()
{
    if (!jogadorNaArea || ativado || interagir == null)
    {
        return;
    }

    if (interagir.action.WasPressedThisFrame())
    {
        AtivarEnergia();
    }
}

    void AtivarEnergia()
    {
        // Marca o botão como usado e liga a energia do labirinto.
        ativado = true;
        controleLabirinto.energiaLigada = true;

        Debug.Log("ENERGIA LIGADA!");

        iconeE.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostra o ícone quando Maxwell entra na área do botão.
        if (collision.CompareTag("Player") && !ativado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);

            Debug.Log("MAXWELL ENTROU NA ÁREA DO BOTÃO");
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