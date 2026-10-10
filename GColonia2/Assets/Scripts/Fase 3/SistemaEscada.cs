using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SistemaEscada : MonoBehaviour
{
    [Header("Pontos da Escada")]
    public Transform pontoInferior;
    public Transform pontoSuperior;

    [Header("Configurações")]
    public float velocidade = 2f;

    // Referências do personagem que entrou na área.
    private Rigidbody2D rb;
    private MovimentacaoPersonagem movimento;

    // Controla o uso da escada.
    private bool pertoDaEscada = false;
    private bool subindo = false;
    private Transform destino;
    private float gravidadeOriginal;

    void Update()
    {
        // Começa a usar a escada ao apertar E perto dela.
        if (pertoDaEscada && !subindo && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(UsarEscada());
        }
    }

    public void EntrarNaArea(Collider2D colisao)
    {
        if (subindo) return;

        rb = colisao.GetComponent<Rigidbody2D>();
        movimento = colisao.GetComponent<MovimentacaoPersonagem>();

        if (rb == null || movimento == null) return;

        pertoDaEscada = true;

        // Vê qual ponta está mais perto e usa a outra como destino.
        float distanciaInferior = Vector2.Distance(rb.position, pontoInferior.position);
        float distanciaSuperior = Vector2.Distance(rb.position, pontoSuperior.position);

        if (distanciaInferior < distanciaSuperior)
        {
            destino = pontoSuperior;
        }
        else
        {
            destino = pontoInferior;
        }
    }

    public void SairDaArea()
    {
        // Só marca que saiu se o personagem não estiver usando a escada.
        if (!subindo)
        {
            pertoDaEscada = false;
        }
    }

    IEnumerator UsarEscada()
   {
        // Trava o controle do personagem enquanto ele usa a escada.
    subindo = true;
    pertoDaEscada = false;
    movimento.podeMover = false;

        // Guarda a gravidade e pausa a física para mover o personagem.
    gravidadeOriginal = rb.gravityScale;
    rb.linearVelocity = Vector2.zero;
    rb.gravityScale = 0;
    rb.simulated = false;
    
        // Primeiro sobe ou desce até a altura do destino.
        Vector3 posicaoVertical = new Vector3(
            rb.transform.position.x,
            destino.position.y,
            rb.transform.position.z
        );

    while (Vector3.Distance(rb.transform.position, posicaoVertical) > 0.05f)
    {
        rb.transform.position = Vector3.MoveTowards(rb.transform.position,posicaoVertical,velocidade * Time.deltaTime);
        yield return null;
    }

        // Depois vai até o ponto de chegada.
    while (Vector3.Distance(rb.transform.position, destino.position) > 0.05f)
    {
            rb.transform.position = Vector3.MoveTowards(rb.transform.position,destino.position,velocidade * Time.deltaTime);
        yield return null;
    }

    rb.transform.position = destino.position;

        // Volta com a física e a gravidade que estavam antes.
    rb.gravityScale = gravidadeOriginal;
    rb.simulated = true;
    rb.linearVelocity = Vector2.zero;

        // Espera a física atualizar antes de liberar o controle.
    yield return new WaitForFixedUpdate();

    movimento.podeMover = true;
    subindo = false;
   }
}
