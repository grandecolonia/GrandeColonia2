using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteracaoCaixoteCano : MonoBehaviour
{
    [Header("Caixote")]

    // Objeto completo do caixote.
    public GameObject caixote;

    // Controle geral dos itens do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Item")]

    // Visual do cano mostrado quando ele é coletado.
    public GameObject canoVisual;

    // Altura que o cano sobe durante a animação.
    public float distanciaSubida = 0.7f;

    // Velocidade da animação.
    public float velocidadeSubida = 2f;

    [Header("Interface")]

    public GameObject iconeE;
    public GameObject avisoItem;

    [Header("Input")]

    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool coletado = false;

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
        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    void Update()
    {
        if (jogadorNaArea && !coletado && interagir.action.WasPressedThisFrame())
        {
            StartCoroutine(ColetarCano());
        }
    }

    IEnumerator ColetarCano()
    {
        coletado = true;

        // Maxwell passa a possuir o cano.
        controleLabirinto.temCano = true;

        iconeE.SetActive(false);

        // Esconde apenas a parte visual do caixote.
        SpriteRenderer spriteCaixote = caixote.GetComponent<SpriteRenderer>();

        if (spriteCaixote != null)
        {
            spriteCaixote.enabled = false;
        }

        // Desativa o collider para Maxwell poder passar.
        BoxCollider2D colliderCaixote = caixote.GetComponent<BoxCollider2D>();

        if (colliderCaixote != null)
        {
            colliderCaixote.enabled = false;
        }

        // Mostra o cano.
        canoVisual.SetActive(true);
        avisoItem.SetActive(true);

        Vector3 posicaoInicial = canoVisual.transform.localPosition;
        Vector3 posicaoFinal = posicaoInicial + Vector3.up * distanciaSubida;

        // Faz o cano subir suavemente.
        while (Vector3.Distance(canoVisual.transform.localPosition, posicaoFinal) > 0.01f)
        {
            canoVisual.transform.localPosition = Vector3.MoveTowards(
                canoVisual.transform.localPosition,
                posicaoFinal,
                velocidadeSubida * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !coletado)
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
        }
    }
}