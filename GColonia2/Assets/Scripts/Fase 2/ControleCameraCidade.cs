
using UnityEngine;

public class ControleCameraCidade : MonoBehaviour
{
    [System.Serializable]
    public class ConfiguracaoVisao
    {
        public SpriteRenderer cenario;
        public Vector2 ajustePosicao;
        public float zoom = 1f;
    }

    [Header("Cenários")]
    public ConfiguracaoVisao[] visoes;

    [Header("Personagem")]
    public Transform personagem;

    [Header("Limites")]
    public Transform limite01;
    public Transform limite02;

    [Header("Configuração")]
    public int visaoInicial = 0;
    public float margem = 0.98f;

    private Camera cameraPrincipal;
    private int visaoAtual = -1;

    void Awake()
    {
        cameraPrincipal = GetComponent<Camera>();
        MudarVisao(visaoInicial);
    }

    void LateUpdate()
    {
        if (personagem == null || limite01 == null || limite02 == null)
        {
            return;
        }

        int novaVisao = 0;

        if (personagem.position.x >= limite02.position.x)
        {
            novaVisao = 2;
        }
        else if (personagem.position.x >= limite01.position.x)
        {
            novaVisao = 1;
        }

        if (novaVisao != visaoAtual)
        {
            MudarVisao(novaVisao);
        }
    }

    public void MudarVisao(int indice)
    {
        if (indice < 0 || indice >= visoes.Length)
        {
            return;
        }

        ConfiguracaoVisao visao = visoes[indice];

        if (visao.cenario == null)
        {
            return;
        }

        visaoAtual = indice;

        Bounds area = visao.cenario.bounds;

        // Centraliza e aplica o ajuste individual.
        Vector2 centro = (Vector2)area.center + visao.ajustePosicao;

        transform.position = new Vector3(centro.x, centro.y, transform.position.z);

        // Calcula o enquadramento e aplica o zoom.
        float altura = area.size.y / 2f;
        float largura = area.size.x / (2f * cameraPrincipal.aspect);

        float zoom = Mathf.Max(0.01f, visao.zoom);

        cameraPrincipal.orthographicSize = Mathf.Min(altura, largura) * margem / zoom;
    }
}
