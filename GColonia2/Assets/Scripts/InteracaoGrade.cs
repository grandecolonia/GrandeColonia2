using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteracaoGrade : MonoBehaviour
{
    [Header("Grade")]

    public GameObject grade;
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    public GameObject iconeE;
    public GameObject avisoSemAlicate;

    [Header("Input")]

    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool aberta = false;

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
        avisoSemAlicate.SetActive(false);
    }

    void Update()
    {
        if (jogadorNaArea && !aberta && interagir.action.WasPressedThisFrame())
        {
            InteragirGrade();
        }
    }

    void InteragirGrade()
    {
        if (!controleLabirinto.temAlicate)
        {
            StartCoroutine(MostrarAviso());
            return;
        }

        aberta = true;

        iconeE.SetActive(false);
        grade.SetActive(false);
        FindFirstObjectByType<GradeNavegacao>()?.CriarGrade();
        PontosNavegacao.AtualizarTodos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !aberta)
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
            avisoSemAlicate.SetActive(false);
        }
    }

    IEnumerator MostrarAviso()
    {
        avisoSemAlicate.SetActive(true);

        yield return new WaitForSeconds(2f);

        avisoSemAlicate.SetActive(false);
    }
}