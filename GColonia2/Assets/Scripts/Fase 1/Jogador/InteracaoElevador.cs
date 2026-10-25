
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteracaoElevador : MonoBehaviour
{
    [Header("Elevador")]

    // Ícone que aparece para avisar que o jogador pode apertar E.
    public GameObject iconeE;

    // Cena para onde o elevador vai levar o jogador.
    public string cenaDestino = "09_VisaodeCima";

    // Guarda se o jogador está perto o suficiente para usar o elevador.
    private bool jogadorPerto = false;

    void Start()
    {
        // Começa com o ícone escondido, já que o jogador ainda não chegou perto.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Se o jogador está perto e apertou E, entra no elevador.
        if (jogadorPerto && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Esconde o ícone antes de sair da cena.
            iconeE.SetActive(false);

            // Se o sistema de entregas existe, avisa que o jogador veio pelo elevador.
            if (ControleEntrega.instancia != null)
            {
                ControleEntrega.instancia.pontoEntrada = "Elevador";
            }

            // Carrega a cena configurada para o destino do elevador.
            SceneManager.LoadScene(cenaDestino);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se foi o jogador que entrou na área, libera a interação.
        if (other.CompareTag("Player"))
        {
            // Marca que o jogador está perto e mostra o ícone E.
            jogadorPerto = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se o jogador saiu da área, não pode mais usar o elevador.
        if (other.CompareTag("Player"))
        {
            // Cancela a interação e esconde o ícone.
            jogadorPerto = false;
            iconeE.SetActive(false);
        }
    }
}
