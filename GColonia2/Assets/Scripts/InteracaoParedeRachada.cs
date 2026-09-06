using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteracaoParedeRachada : MonoBehaviour
{
    [Header("Interface")]

    // Ícone mostrado quando o jogador pode interagir.
    public GameObject iconeE;

    // Aviso mostrado quando o jogador tenta quebrar a parede sem o item.
    public GameObject avisoSemItem;

    [Header("Parede")]

    // Objeto completo da parede que será destruído.
    public GameObject paredeRachada;

    // Controle dos itens encontrados dentro do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Input")]

    // Action usada para interagir com a parede.
    public InputActionReference interagir;

    private bool jogadorNaArea = false;

    [Header("Cenário")]

   // SpriteRenderer da imagem completa do labirinto.
   public GameObject cenarioFechado;

   // Versão do cenário com a passagem aberta.
   public GameObject cenarioAberto;

   // Collider que bloqueia a passagem enquanto a parede existe.
   public Collider2D bloqueioParede;

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
        avisoSemItem.SetActive(false);
        cenarioFechado.SetActive(true);
        cenarioAberto.SetActive(false);
    }

    void Update()
    {
        if (jogadorNaArea && interagir.action.WasPressedThisFrame())
        {
            InteragirParede();
        }
    }

    void InteragirParede()
    {
        // Maxwell ainda não possui o cano.
    if (!controleLabirinto.temCano)
    {
        StartCoroutine(MostrarAviso());
        return;
    }

// Troca apenas a imagem do cenário.
cenarioFechado.SetActive(false);
cenarioAberto.SetActive(true);

// Remove o bloqueio real da passagem.
bloqueioParede.enabled = false;

// Atualiza a física.
Physics2D.SyncTransforms();

// Recria a navegação com a passagem agora livre.
GradeNavegacao grade = FindFirstObjectByType<GradeNavegacao>();

if (grade != null)
{
    grade.CriarGrade();
}

iconeE.SetActive(false);
jogadorNaArea = false;
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
            avisoSemItem.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        avisoSemItem.SetActive(true);

        yield return new WaitForSeconds(2f);

        avisoSemItem.SetActive(false);
    }
}