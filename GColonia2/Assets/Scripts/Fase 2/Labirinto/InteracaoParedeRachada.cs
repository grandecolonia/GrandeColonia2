
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NavMeshPlus.Components;

public class InteracaoParedeRachada : MonoBehaviour
{
    [Header("NavMesh")]

    // Superfície usada para atualizar os caminhos dos guardas depois de abrir a parede.
    public NavMeshSurface navMeshSurface;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode interagir com a parede.
    public GameObject iconeE;

    // Aviso que aparece quando Maxwell tenta abrir a passagem sem o cano.
    public GameObject avisoSemItem;

    [Header("Parede")]

    // Objeto da parede rachada que Maxwell precisa abrir.
    public GameObject paredeRachada;

    // Sistema que guarda os itens coletados durante o labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Input")]

    // Ação usada para interagir com a parede rachada.
    public InputActionReference interagir;

    // Guarda se Maxwell está perto o suficiente para interagir.
    private bool jogadorNaArea = false;

    // Marca se a passagem já foi aberta.
    private bool aberta = false;

    [Header("Cenário")]

    // Versão do cenário antes de Maxwell abrir a passagem.
    public GameObject cenarioFechado;

    // Versão do cenário depois que a parede é aberta.
    public GameObject cenarioAberto;

    // Collider que impede Maxwell de atravessar enquanto a parede está fechada.
    public Collider2D bloqueioParede;

    void OnEnable()
    {
        // Habilita a ação de interação quando este componente fica ativo.
        if (interagir != null)
        {
            interagir.action.Enable();
        }
    }

    void Start()
    {
        // Começa escondendo os elementos de interação.
        iconeE.SetActive(false);
        avisoSemItem.SetActive(false);

        // Deixa o cenário com a passagem fechada.
        cenarioFechado.SetActive(true);
        cenarioAberto.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell está perto e apertou o comando, tenta interagir com a parede.
        if (jogadorNaArea && interagir.action.WasPressedThisFrame())
        {
            InteragirParede();
        }
    }

    void InteragirParede()
    {
        // Se a parede já foi aberta, não precisa fazer nada novamente.
        if (aberta)
        {
            return;
        }

        // Se Maxwell ainda não possui o cano, mostra o aviso e cancela a interação.
        if (!controleLabirinto.temCano)
        {
            StartCoroutine(MostrarAviso());
            return;
        }

        // Marca que a passagem foi aberta.
        aberta = true;

        // Esconde os avisos e encerra a interação com a parede.
        iconeE.SetActive(false);
        avisoSemItem.SetActive(false);
        jogadorNaArea = false;

        // Esconde o cenário fechado e mostra a versão com a passagem aberta.
        cenarioFechado.SetActive(false);
        cenarioAberto.SetActive(true);

        // Desativa a colisão para Maxwell conseguir atravessar.
        bloqueioParede.enabled = false;

        // Atualiza as posições e colisões antes de recalcular a navegação.
        Physics2D.SyncTransforms();

        // Refaz o NavMesh para os guardas reconhecerem a nova passagem.
        navMeshSurface.BuildNavMesh();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell chegou perto e a parede ainda está fechada, libera a interação.
        if (collision.CompareTag("Player") && !aberta)
        {
            // Marca que Maxwell está dentro da área.
            jogadorNaArea = true;

            // Mostra o ícone para avisar que ele pode interagir.
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se Maxwell saiu da área, cancela a interação e esconde os avisos.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
            avisoSemItem.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        // Mostra que Maxwell ainda precisa encontrar o item para abrir a parede.
        avisoSemItem.SetActive(true);

        // Deixa o aviso aparecendo por dois segundos.
        yield return new WaitForSeconds(2f);

        // Esconde o aviso depois que o tempo termina.
        avisoSemItem.SetActive(false);
    }
}
