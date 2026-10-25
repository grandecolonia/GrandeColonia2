
using UnityEngine;

public class ControleCameraCidade : MonoBehaviour
{
    [System.Serializable]
    public class ConfiguracaoVisao
    {
        // Cenário que a câmera precisa mostrar nesta visão.
        public SpriteRenderer cenario;

        // Permite ajustar a posição da câmera sem precisar mover o cenário.
        public Vector2 ajustePosicao;

        // Controla a aproximação da câmera nesta visão.
        public float zoom = 1f;
    }

    [Header("Cenários")]

    // Guarda as configurações das três visões da cidade.
    public ConfiguracaoVisao[] visoes;

    [Header("Personagem")]

    // Maxwell, que será usado para identificar quando a câmera deve mudar.
    public Transform personagem;

    [Header("Limites")]

    // Primeiro ponto onde a câmera deve trocar para a segunda visão.
    public Transform limite01;

    // Segundo ponto onde a câmera deve trocar para a terceira visão.
    public Transform limite02;

    [Header("Configuração")]

    // Define qual visão aparece quando a cena começa.
    public int visaoInicial = 0;

    // Controla a margem usada para calcular o enquadramento do cenário.
    public float margem = 0.98f;

    // Guarda a câmera responsável por mostrar as visões.
    private Camera cameraPrincipal;

    // Guarda a visão atual para não ficar atualizando a câmera sem necessidade.
    private int visaoAtual = -1;

    void Awake()
    {
        // Pega o componente Camera do próprio objeto.
        cameraPrincipal = GetComponent<Camera>();

        // Já coloca a câmera na visão inicial antes da cena começar.
        MudarVisao(visaoInicial);
    }

    void LateUpdate()
    {
        // Se Maxwell ou algum limite não existe, não tenta trocar de visão.
        if (personagem == null || limite01 == null || limite02 == null)
        {
            return;
        }

        // Começa considerando que Maxwell está na primeira visão.
        int novaVisao = 0;

        // Se Maxwell passou do segundo limite, precisa mostrar a terceira visão.
        if (personagem.position.x >= limite02.position.x)
        {
            novaVisao = 2;
        }
        // Se passou apenas do primeiro limite, mostra a segunda visão.
        else if (personagem.position.x >= limite01.position.x)
        {
            novaVisao = 1;
        }

        // Só muda a câmera quando Maxwell realmente entra em outra visão.
        if (novaVisao != visaoAtual)
        {
            MudarVisao(novaVisao);
        }
    }

    public void MudarVisao(int indice)
    {
        // Se a visão escolhida não existe no vetor, cancela a troca.
        if (indice < 0 || indice >= visoes.Length)
        {
            return;
        }

        // Pega as configurações da visão que precisa aparecer.
        ConfiguracaoVisao visao = visoes[indice];

        // Se essa visão não possui um cenário, não tenta posicionar a câmera.
        if (visao.cenario == null)
        {
            return;
        }

        // Guarda qual visão está ativa para não repetir a troca a cada frame.
        visaoAtual = indice;

        // Pega os limites reais do sprite para calcular o espaço disponível.
        Bounds area = visao.cenario.bounds;

        // Encontra o centro do cenário e aplica o ajuste de posição escolhido.
        Vector2 centro = (Vector2)area.center + visao.ajustePosicao;

        // Move a câmera imediatamente para o centro da visão, sem alterar sua profundidade.
        transform.position = new Vector3(centro.x, centro.y, transform.position.z);

        // Calcula metade da altura do cenário para ajustar o enquadramento vertical.
        float altura = area.size.y / 2f;

        // Calcula metade da largura do cenário considerando a proporção da câmera.
        float largura = area.size.x / (2f * cameraPrincipal.aspect);

        // Impede que o zoom fique zerado ou negativo.
        float zoom = Mathf.Max(0.01f, visao.zoom);

        // Ajusta o tamanho da câmera para enquadrar o cenário conforme a margem e o zoom.
        cameraPrincipal.orthographicSize = Mathf.Min(altura, largura) * margem / zoom;
    }
}
