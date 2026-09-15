using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NavMeshPlus.Components;

public class InteracaoGrade : MonoBehaviour
{
    [Header("Grade")]

    // Grade da cena e controle geral do labirinto.
    public GameObject grade;
    public ControleLabirinto controleLabirinto;

    [Header("NavMesh")]

    // Superfície usada para atualizar a navegação depois que a grade abrir.
    public NavMeshSurface navMeshSurface;

    [Header("Interface")]

    // Ícone de interação e aviso mostrado caso Maxwell ainda não tenha o alicate.
    public GameObject iconeE;
    public GameObject avisoSemAlicate;

    [Header("Input")]

    // Action usada para interagir com a grade.
    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool aberta = false;

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
        // Começa com os elementos da interface escondidos.
        iconeE.SetActive(false);
        avisoSemAlicate.SetActive(false);
    }

    void Update()
    {
        // Tenta abrir a grade quando Maxwell estiver na área e apertar o comando.
        if (jogadorNaArea && !aberta && interagir.action.WasPressedThisFrame())
        {
            InteragirGrade();
        }
    }

    void InteragirGrade()
    {
        // Sem o alicate, mostra o aviso e para a interação.
        if (!controleLabirinto.temAlicate)
        {
            StartCoroutine(MostrarAviso());
            return;
        }

        aberta = true;

        iconeE.SetActive(false);

        // Libera fisicamente a passagem.
        Collider2D colliderGrade = grade.GetComponent<Collider2D>();

        if (colliderGrade != null)
        {
            colliderGrade.enabled = false;
        }

        // Atualiza a física antes de refazer o NavMesh.
        Physics2D.SyncTransforms();

        // Refaz a navegação dos guardas com a passagem agora aberta.
        navMeshSurface.BuildNavMesh();

        // Remove a grade da cena.
        grade.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostra o ícone quando Maxwell entra na área e a grade ainda está fechada.
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
            avisoSemAlicate.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        // Mostra por alguns segundos que ainda falta o alicate.
        avisoSemAlicate.SetActive(true);

        yield return new WaitForSeconds(2f);

        avisoSemAlicate.SetActive(false);
    }
}