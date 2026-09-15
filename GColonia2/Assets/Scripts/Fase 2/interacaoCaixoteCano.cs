using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteracaoCaixoteCano : MonoBehaviour
{
    [Header("Caixote")]

    // Objeto completo do caixote e controle geral do labirinto.
    public GameObject caixote;
    public ControleLabirinto controleLabirinto;

    [Header("Item")]

    // Visual do cano que aparece quando ele é coletado.
    public GameObject canoVisual;

    // Controla a altura e a velocidade da animação do cano.
    public float distanciaSubida = 0.7f;
    public float velocidadeSubida = 2f;

    [Header("Interface")]

    // Ícone de interação e aviso mostrado quando o item é coletado.
    public GameObject iconeE;
    public GameObject avisoItem;

    [Header("Input")]

    // Action usada para interagir com o caixote.
    public InputActionReference interagir;

    private bool jogadorNaArea = false;
    private bool coletado = false;

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
        // Começa com os elementos da interação escondidos.
        iconeE.SetActive(false);
        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    void Update()
    {
        // Coleta o cano quando Maxwell estiver na área e apertar o comando.
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

        // Mostra o cano e o aviso do item.
        canoVisual.SetActive(true);
        avisoItem.SetActive(true);

        Vector3 posicaoInicial = canoVisual.transform.localPosition;
        Vector3 posicaoFinal = posicaoInicial + Vector3.up * distanciaSubida;

        // Faz o cano subir suavemente.
        while (Vector3.Distance(canoVisual.transform.localPosition, posicaoFinal) > 0.01f)
        {
            canoVisual.transform.localPosition = Vector3.MoveTowards(canoVisual.transform.localPosition, posicaoFinal, velocidadeSubida * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Esconde o item e o aviso depois da animação.
        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostra o ícone quando Maxwell entra na área e o item ainda não foi coletado.
        if (collision.CompareTag("Player") && !coletado)
        {
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Esconde o ícone quando Maxwell sai da área.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}