using UnityEngine;
using System.Collections.Generic;

public class PontosNavegacao : MonoBehaviour
{
    [Header("Conexões")]

    // Pontos que podem ser alcançados diretamente a partir deste ponto.
    public List<PontosNavegacao> vizinhos = new List<PontosNavegacao>();

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.12f);

        foreach (PontosNavegacao vizinho in vizinhos)
        {
            if (vizinho != null)
            {
                Gizmos.DrawLine(
                    transform.position,
                    vizinho.transform.position
                );
            }
        }
    }
}