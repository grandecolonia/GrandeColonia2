
using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoBotaoEnergia : MonoBehaviour
{
    [Header("Labirinto")]

    // Sistema que guarda se a energia do labirinto já foi ligada.
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode apertar E para interagir.
    public GameObject iconeE;

    [Header("Input")]

    // Ação que identifica quando o jogador aperta o botão de interação.
    public InputActionReference interagir;

    // Guarda se Maxwell está perto o suficiente para apertar o botão.
    private bool jogadorNaArea = false;

    // Impede que a energia seja ativada várias vezes.
    private bool ativado = false;

    void Start()
    {
        // Começa com o ícone de interação escondido.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell está longe, a energia já foi ligada ou não existe ação configurada, não faz nada.
        if (!jogadorNaArea || ativado || interagir == null)
        {
            return;
        }

        // Se o jogador apertou o comando de interação, liga a energia.
        if (interagir.action.WasPressedThisFrame())
        {
            AtivarEnergia();
        }
    }

    void AtivarEnergia()
    {
        // Marca o botão como utilizado para impedir outra ativação.
        ativado = true;

        // Avisa ao controle do labirinto que a energia foi ligada.
        controleLabirinto.energiaLigada = true;

        // Esconde o ícone porque o botão não precisa mais ser utilizado.
        iconeE.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e o botão ainda não foi usado, libera a interação.
        if (collision.CompareTag("Player") && !ativado)
        {
            // Marca que Maxwell está perto do botão.
            jogadorNaArea = true;

            // Mostra o ícone para avisar que ele pode interagir.
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se Maxwell saiu da área, cancela a possibilidade de interação.
        if (collision.CompareTag("Player"))
        {
            // Marca que o jogador não está mais perto do botão.
            jogadorNaArea = false;

            // Esconde o ícone de interação.
            iconeE.SetActive(false);
        }
    }
}
