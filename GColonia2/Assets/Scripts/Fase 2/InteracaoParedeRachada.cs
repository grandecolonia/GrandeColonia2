using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NavMeshPlus.Components;

public class InteracaoParedeRachada : MonoBehaviour
{
    [Header("NavMesh")]

    // Superfície usada para atualizar a navegação depois que a parede abrir.
    public NavMeshSurface navMeshSurface;

    [Header("Interface")]

    // Ícone mostrado quando Maxwell pode interagir.
    public GameObject iconeE;

    // Aviso mostrado quando Maxwell tenta quebrar a parede sem o item.
    public GameObject avisoSemItem;

    [Header("Parede")]

    // Objeto completo da parede rachada.
    public GameObject paredeRachada;

    // Controle geral dos itens do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Input")]

    // Action usada para interagir com a parede.
    public InputActionReference interagir;

    private bool jogadorNaArea = false;

    private bool aberta = false;

    [Header("Cenário")]

    // Versão do cenário com a passagem fechada.
    public GameObject cenarioFechado;

    // Versão do cenário com a passagem aberta.
    public GameObject cenarioAberto;

    // Collider que bloqueia a passagem enquanto a parede existe.
    public Collider2D bloqueioParede;

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
        // Começa com a passagem fechada e os avisos escondidos.
        iconeE.SetActive(false);
        avisoSemItem.SetActive(false);
        cenarioFechado.SetActive(true);
        cenarioAberto.SetActive(false);
    }

    void Update()
    {
        // Tenta interagir com a parede quando Maxwell estiver na área.
        if (jogadorNaArea && interagir.action.WasPressedThisFrame())
        {
            InteragirParede();
        }
    }

    void InteragirParede()
{
    if (aberta)
    {
        return;
    }

    if (!controleLabirinto.temCano)
    {
        StartCoroutine(MostrarAviso());
        return;
    }

    aberta = true;

    iconeE.SetActive(false);
    avisoSemItem.SetActive(false);
    jogadorNaArea = false;

    // Abre a passagem e remove o bloqueio.
    cenarioFechado.SetActive(false);
    cenarioAberto.SetActive(true);
    bloqueioParede.enabled = false;

    // Atualiza a navegação dos guardas.
    Physics2D.SyncTransforms();
    navMeshSurface.BuildNavMesh();
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostra o ícone quando Maxwell entra na área da parede.
        if (collision.CompareTag("Player") && !aberta)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Esconde os avisos quando Maxwell sai da área.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
            avisoSemItem.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        // Mostra por alguns segundos que ainda falta o item.
        avisoSemItem.SetActive(true);

        yield return new WaitForSeconds(2f);

        avisoSemItem.SetActive(false);
    }
}