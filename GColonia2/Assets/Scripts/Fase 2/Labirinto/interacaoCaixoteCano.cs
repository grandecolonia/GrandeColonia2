
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteracaoCaixoteCano : MonoBehaviour
{
    [Header("Caixote")]

    // Caixote que vai desaparecer depois que Maxwell pegar o cano.
    public GameObject caixote;

    // Sistema que guarda os itens coletados durante o labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Item")]

    // Objeto visual do cano que aparece durante a coleta.
    public GameObject canoVisual;

    // Distância que o cano vai subir durante a animação.
    public float distanciaSubida = 0.7f;

    // Velocidade com que o cano sobe depois de ser coletado.
    public float velocidadeSubida = 2f;

    [Header("Interface")]

    // Ícone que avisa quando Maxwell pode interagir com o caixote.
    public GameObject iconeE;

    // Aviso que aparece na tela depois que o cano é coletado.
    public GameObject avisoItem;

    [Header("Input")]

    // Ação que identifica quando o jogador aperta o comando de interação.
    public InputActionReference interagir;

    // Guarda se Maxwell está perto o suficiente para coletar o cano.
    private bool jogadorNaArea = false;

    // Impede que o mesmo cano seja coletado várias vezes.
    private bool coletado = false;

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
        // Começa escondendo os elementos que só aparecem durante a coleta.
        iconeE.SetActive(false);
        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell está perto, ainda não pegou o cano e apertou o comando, começa a coleta.
        if (jogadorNaArea && !coletado && interagir.action.WasPressedThisFrame())
        {
            StartCoroutine(ColetarCano());
        }
    }

    IEnumerator ColetarCano()
    {
        // Marca que o cano já foi coletado para impedir outra interação.
        coletado = true;

        // Avisa ao controle do labirinto que Maxwell agora possui o cano.
        controleLabirinto.temCano = true;

        // Esconde o ícone porque o item já foi coletado.
        iconeE.SetActive(false);

        // Procura o SpriteRenderer do caixote para esconder sua imagem.
        SpriteRenderer spriteCaixote = caixote.GetComponent<SpriteRenderer>();

        // Se encontrou o sprite, desliga somente a parte visual do caixote.
        if (spriteCaixote != null)
        {
            spriteCaixote.enabled = false;
        }

        // Procura o BoxCollider2D do caixote para liberar a passagem.
        BoxCollider2D colliderCaixote = caixote.GetComponent<BoxCollider2D>();

        // Se encontrou a colisão, desativa para não atrapalhar Maxwell.
        if (colliderCaixote != null)
        {
            colliderCaixote.enabled = false;
        }

        // Mostra o cano e o aviso de que Maxwell conseguiu o item.
        canoVisual.SetActive(true);
        avisoItem.SetActive(true);

        // Guarda onde o cano começou e calcula até onde ele precisa subir.
        Vector3 posicaoInicial = canoVisual.transform.localPosition;
        Vector3 posicaoFinal = posicaoInicial + Vector3.up * distanciaSubida;

        // Continua subindo até o cano chegar perto do ponto final.
        while (Vector3.Distance(canoVisual.transform.localPosition, posicaoFinal) > 0.01f)
        {
            // Move o cano suavemente sem ultrapassar o destino.
            canoVisual.transform.localPosition = Vector3.MoveTowards(canoVisual.transform.localPosition, posicaoFinal, velocidadeSubida * Time.deltaTime);

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Mantém o cano e o aviso aparecendo por mais um segundo.
        yield return new WaitForSeconds(1f);

        // Terminou a animação, então esconde o item e o aviso.
        canoVisual.SetActive(false);
        avisoItem.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e ainda não pegou o cano, libera a interação.
        if (collision.CompareTag("Player") && !coletado)
        {
            // Marca que Maxwell está perto e mostra o ícone de interação.
            jogadorNaArea = true;
            iconeE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se Maxwell saiu da área, esconde o ícone e cancela a interação.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}
