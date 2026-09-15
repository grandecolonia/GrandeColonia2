using UnityEngine;

public class BombaFase2 : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade que a bomba cai até o ponto marcado.
    public float velocidadeQueda = 7f;

    private Vector3 destino;
    private bool configurada = false;

    public void ConfigurarDestino(Vector3 novoDestino)
    {
        destino = novoDestino;
        configurada = true;
    }

    void Update()
    {
        if (!configurada)
        {
            return;
        }

        // Move a bomba até o ponto marcado no chão.
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadeQueda * Time.deltaTime);

        // Detecta quando a bomba chegou no chão.
        if (Vector3.Distance(transform.position, destino) < 0.05f)
        {
            AtingirChao();
        }
    }

    void AtingirChao()
    {
        Debug.Log("BOMBA ATINGIU O CHÃO!");

        Destroy(gameObject);
    }
}