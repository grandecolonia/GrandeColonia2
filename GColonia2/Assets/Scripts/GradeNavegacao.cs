using UnityEngine;
using System.Collections.Generic;

public class GradeNavegacao : MonoBehaviour
{
    [Header("Área do Labirinto")]

    public Vector2 tamanhoMundo = new Vector2(18f, 10f);
    public float tamanhoCelula = 0.5f;

    [Header("Colisão")]

    // Layer usada pelas paredes do labirinto.
    public LayerMask camadaParede;

    // Quanto espaço deve ser considerado ao redor de cada célula.
    public float raioBloqueio = 0.2f;

    private Celula[,] grade;

    private int quantidadeX;
    private int quantidadeY;

    public class Celula
    {
        public bool podeAndar;

        public Vector2 posicaoMundo;

        public int x;
        public int y;

        public Celula(bool podeAndar, Vector2 posicaoMundo, int x, int y)
        {
            this.podeAndar = podeAndar;
            this.posicaoMundo = posicaoMundo;
            this.x = x;
            this.y = y;
        }
    }

    void Awake()
    {
        CriarGrade();
    }

    public void CriarGrade()
    {
        quantidadeX = Mathf.Max(1, Mathf.RoundToInt(tamanhoMundo.x / tamanhoCelula));
        quantidadeY = Mathf.Max(1, Mathf.RoundToInt(tamanhoMundo.y / tamanhoCelula));

        grade = new Celula[quantidadeX, quantidadeY];

        Vector2 cantoInferiorEsquerdo =
            (Vector2)transform.position - tamanhoMundo / 2f;

        for (int x = 0; x < quantidadeX; x++)
        {
            for (int y = 0; y < quantidadeY; y++)
            {
                Vector2 posicao = cantoInferiorEsquerdo + new Vector2(
                    x * tamanhoCelula + tamanhoCelula / 2f,
                    y * tamanhoCelula + tamanhoCelula / 2f
                );

                bool temParede = Physics2D.OverlapCircle(
                    posicao,
                    raioBloqueio,
                    camadaParede
                ) != null;

                grade[x, y] = new Celula(
                    !temParede,
                    posicao,
                    x,
                    y
                );
            }
        }
    }

    public List<Vector2> EncontrarCaminho(Vector2 origem, Vector2 destino)
    {
        Celula inicio = PegarCelulaLivreMaisProxima(origem);
        Celula fim = PegarCelulaLivreMaisProxima(destino);

        if (inicio == null || fim == null)
        {
            return new List<Vector2>();
        }

        List<Celula> abertos = new List<Celula>();
        HashSet<Celula> fechados = new HashSet<Celula>();

        Dictionary<Celula, Celula> veioDe = new Dictionary<Celula, Celula>();
        Dictionary<Celula, float> custo = new Dictionary<Celula, float>();

        abertos.Add(inicio);
        custo[inicio] = 0f;

        while (abertos.Count > 0)
        {
            Celula atual = abertos[0];

            float melhorCusto =
                custo[atual] + Heuristica(atual, fim);

            foreach (Celula celula in abertos)
            {
                float custoCelula =
                    custo[celula] + Heuristica(celula, fim);

                if (custoCelula < melhorCusto)
                {
                    atual = celula;
                    melhorCusto = custoCelula;
                }
            }

            if (atual == fim)
            {
                return ReconstruirCaminho(veioDe, atual);
            }

            abertos.Remove(atual);
            fechados.Add(atual);

            foreach (Celula vizinho in PegarVizinhos(atual))
            {
                if (!vizinho.podeAndar || fechados.Contains(vizinho))
                {
                    continue;
                }

                float novoCusto = custo[atual] + 1f;

                if (!custo.ContainsKey(vizinho) || novoCusto < custo[vizinho])
                {
                    veioDe[vizinho] = atual;
                    custo[vizinho] = novoCusto;

                    if (!abertos.Contains(vizinho))
                    {
                        abertos.Add(vizinho);
                    }
                }
            }
        }

        return new List<Vector2>();
    }

    List<Celula> PegarVizinhos(Celula celula)
    {
        List<Celula> vizinhos = new List<Celula>();

        // Somente cima, baixo, esquerda e direita.
        AdicionarVizinho(vizinhos, celula.x + 1, celula.y);
        AdicionarVizinho(vizinhos, celula.x - 1, celula.y);
        AdicionarVizinho(vizinhos, celula.x, celula.y + 1);
        AdicionarVizinho(vizinhos, celula.x, celula.y - 1);

        return vizinhos;
    }

    void AdicionarVizinho(List<Celula> lista, int x, int y)
    {
        if (
            x >= 0 &&
            x < quantidadeX &&
            y >= 0 &&
            y < quantidadeY
        )
        {
            lista.Add(grade[x, y]);
        }
    }

    Celula PegarCelula(Vector2 posicaoMundo)
    {
        Vector2 cantoInferiorEsquerdo =
            (Vector2)transform.position - tamanhoMundo / 2f;

        int x = Mathf.FloorToInt(
            (posicaoMundo.x - cantoInferiorEsquerdo.x) / tamanhoCelula
        );

        int y = Mathf.FloorToInt(
            (posicaoMundo.y - cantoInferiorEsquerdo.y) / tamanhoCelula
        );

        x = Mathf.Clamp(x, 0, quantidadeX - 1);
        y = Mathf.Clamp(y, 0, quantidadeY - 1);

        return grade[x, y];
    }

    Celula PegarCelulaLivreMaisProxima(Vector2 posicao)
    {
        Celula celulaInicial = PegarCelula(posicao);

        if (celulaInicial.podeAndar)
        {
            return celulaInicial;
        }

        // Procura uma célula livre ao redor caso esteja muito perto de uma parede.
        for (int raio = 1; raio <= 5; raio++)
        {
            Celula melhorCelula = null;
            float menorDistancia = Mathf.Infinity;

            for (int x = -raio; x <= raio; x++)
            {
                for (int y = -raio; y <= raio; y++)
                {
                    int verificarX = celulaInicial.x + x;
                    int verificarY = celulaInicial.y + y;

                    if (
                        verificarX < 0 ||
                        verificarX >= quantidadeX ||
                        verificarY < 0 ||
                        verificarY >= quantidadeY
                    )
                    {
                        continue;
                    }

                    Celula celula = grade[verificarX, verificarY];

                    if (!celula.podeAndar)
                    {
                        continue;
                    }

                    float distancia = Vector2.Distance(
                        posicao,
                        celula.posicaoMundo
                    );

                    if (distancia < menorDistancia)
                    {
                        menorDistancia = distancia;
                        melhorCelula = celula;
                    }
                }
            }

            if (melhorCelula != null)
            {
                return melhorCelula;
            }
        }

        return null;
    }

    float Heuristica(Celula a, Celula b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector2> ReconstruirCaminho(
        Dictionary<Celula, Celula> veioDe,
        Celula atual
    )
    {
        List<Celula> caminhoCelulas = new List<Celula>();

        caminhoCelulas.Add(atual);

        while (veioDe.ContainsKey(atual))
        {
            atual = veioDe[atual];
            caminhoCelulas.Add(atual);
        }

        caminhoCelulas.Reverse();

        return SimplificarCaminho(caminhoCelulas);
    }

    List<Vector2> SimplificarCaminho(List<Celula> caminho)
    {
        List<Vector2> resultado = new List<Vector2>();

        if (caminho.Count == 0)
        {
            return resultado;
        }

        if (caminho.Count == 1)
        {
            resultado.Add(caminho[0].posicaoMundo);
            return resultado;
        }

        resultado.Add(caminho[0].posicaoMundo);

        Vector2Int direcaoAnterior = new Vector2Int(
            caminho[1].x - caminho[0].x,
            caminho[1].y - caminho[0].y
        );

        for (int i = 2; i < caminho.Count; i++)
        {
            Vector2Int novaDirecao = new Vector2Int(
                caminho[i].x - caminho[i - 1].x,
                caminho[i].y - caminho[i - 1].y
            );

            // Só guarda os pontos onde o caminho muda de direção.
            if (novaDirecao != direcaoAnterior)
            {
                resultado.Add(caminho[i - 1].posicaoMundo);
            }

            direcaoAnterior = novaDirecao;
        }

        resultado.Add(caminho[caminho.Count - 1].posicaoMundo);

        return resultado;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, tamanhoMundo);

        if (grade == null)
        {
            return;
        }

        foreach (Celula celula in grade)
        {
            if (celula.podeAndar)
            {
                Gizmos.DrawWireCube(
                    celula.posicaoMundo,
                    Vector3.one * (tamanhoCelula * 0.85f)
                );
            }
        }
    }
}