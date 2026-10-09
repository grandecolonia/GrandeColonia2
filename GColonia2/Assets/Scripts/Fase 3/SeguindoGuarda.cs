using UnityEngine;

public class SeguindoGuarda : MonoBehaviour
{
    public Transform jogador;
    public float velocidade = 3f;
    public float distanciaVisao = 10f;
    public LayerMask layerMask;
    bool ativo = false;
    
    void Update()
    {
        if (!ativo)
        return;

        Vector2 direcao = (jogador.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaVisao, layerMask);

        Debug.DrawRay(transform.position, direcao * distanciaVisao, Color.red);

        if (hit.collider.gameObject == jogador.gameObject)
        {
            transform.position = Vector2.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);
        }
    }

    public void AtivarGuarda()
    {
        ativo = true;
    }
}
