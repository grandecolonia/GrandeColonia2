
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NavMeshPlus.Components;

public class InteracaoGrade : MonoBehaviour
{
    [Header("Grade")]

    // Grade que será aberta quando Maxwell utilizar o alicate.
    public GameObject grade;

    // Sistema que guarda os itens coletados durante o labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("NavMesh")]

    // Superfície que será atualizada para os guardas reconhecerem a passagem aberta.
    public NavMeshSurface navMeshSurface;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell está perto da grade.
    public GameObject iconeE;

    // Aviso que aparece quando Maxwell tenta abrir a grade sem o alicate.
    public GameObject avisoSemAlicate;

    [Header("Input")]

    // Ação usada para interagir com a grade.
    public InputActionReference interagir;

    // Guarda se Maxwell está perto o suficiente para interagir.
    private bool jogadorNaArea = false;

    // Impede que a grade seja aberta várias vezes.
    private bool aberta = false;

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
        // Começa com o ícone e o aviso escondidos.
        iconeE.SetActive(false);
        avisoSemAlicate.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell está perto, a grade está fechada e apertou o comando, tenta abrir.
        if (jogadorNaArea && !aberta && interagir.action.WasPressedThisFrame())
        {
            InteragirGrade();
        }
    }

    void InteragirGrade()
    {
        // Se Maxwell ainda não possui o alicate, mostra um aviso e cancela a abertura.
        if (!controleLabirinto.temAlicate)
        {
            StartCoroutine(MostrarAviso());
            return;
        }

        // Marca que a grade foi aberta para impedir outra interação.
        aberta = true;

        // Esconde o ícone porque a grade não precisa mais ser utilizada.
        iconeE.SetActive(false);

        // Procura a colisão que impede Maxwell de atravessar a grade.
        Collider2D colliderGrade = grade.GetComponent<Collider2D>();

        // Se encontrou o Collider, desliga a colisão para liberar a passagem.
        if (colliderGrade != null)
        {
            colliderGrade.enabled = false;
        }

        // Atualiza as posições dos objetos no sistema de física.
        Physics2D.SyncTransforms();

        // Refaz o NavMesh para os guardas reconhecerem que a passagem foi aberta.
        navMeshSurface.BuildNavMesh();

        // Remove a grade do cenário depois da abertura.
        grade.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e a grade ainda está fechada, libera a interação.
        if (collision.CompareTag("Player") && !aberta)
        {
            // Marca que Maxwell está perto da grade.
            jogadorNaArea = true;

            // Mostra o ícone para avisar que ele pode interagir.
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Quando Maxwell sai da área, esconde os elementos da interação.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
            avisoSemAlicate.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        // Mostra que Maxwell ainda precisa encontrar o alicate.
        avisoSemAlicate.SetActive(true);

        // Mantém o aviso aparecendo por dois segundos.
        yield return new WaitForSeconds(2f);

        // Depois esconde o aviso novamente.
        avisoSemAlicate.SetActive(false);
    }
}
