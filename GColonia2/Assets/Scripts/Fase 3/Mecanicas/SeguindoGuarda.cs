
using UnityEngine;

public class SeguindoGuarda : MonoBehaviour
{
    [Header("Jogador")]

    // Maxwell, que será perseguido quando o guarda for ativado.
    public Transform jogador;

    [Header("Movimento")]

    // Velocidade com que o guarda se aproxima de Maxwell.
    public float velocidade = 3f;

    [Header("Visão")]

    // Distância máxima que o guarda consegue enxergar.
    public float distanciaVisao = 10f;

    // Define quais camadas o Raycast consegue detectar.
    public LayerMask layerMask;

    [Header("Controle")]

    // Guarda se a perseguição já foi ativada.
    private bool ativo = false;

    void Update()
    {
        // Se a perseguição ainda não começou, o guarda fica parado.
        if (!ativo)
        {
            return;
        }

        // Calcula a direção do guarda até a posição de Maxwell.
        Vector2 direcao = (jogador.position - transform.position).normalized;

        // Lança um raio para verificar se Maxwell está dentro do campo de visão.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaVisao, layerMask);

        // Mostra o raio vermelho na Scene durante os testes.
        Debug.DrawRay(transform.position, direcao * distanciaVisao, Color.red);

        // Se o raio encontrou Maxwell, começa a perseguição.
        if (hit.collider != null && hit.collider.gameObject == jogador.gameObject)
        {
            // Move o guarda suavemente em direção ao jogador.
            transform.position = Vector2.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);
        }
    }

    public void AtivarGuarda()
    {
        // Libera a perseguição quando o evento da fase acontece.
        ativo = true;
    }
}
