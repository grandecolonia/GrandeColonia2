using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoBotaoEnergia : MonoBehaviour
{
    [Header("Labirinto")]

    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    public GameObject iconeE;

    private bool jogadorNaArea = false;
    private bool ativado = false;

    void Start()
    {
        iconeE.SetActive(false);
    }

    void Update()
    {
        if (!jogadorNaArea || ativado)
        {
            return;
        }

        // Lê diretamente a tecla E pelo New Input System.
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AtivarEnergia();
        }
    }

    void AtivarEnergia()
    {
        ativado = true;
        controleLabirinto.energiaLigada = true;

        Debug.Log("ENERGIA LIGADA!");

        iconeE.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !ativado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);

            Debug.Log("MAXWELL ENTROU NA ÁREA DO BOTÃO");
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