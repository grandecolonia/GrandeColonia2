
using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    [Header("Jogador")]

    // Maxwell, que será acompanhado pela câmera durante a fase.
    public Transform jogador;

    [Header("Limites do Cenário")]

    // Primeiro cenário, usado para definir até onde a câmera pode ir à esquerda.
    public SpriteRenderer primeiroCenario;

    // Último cenário, usado para definir até onde a câmera pode ir à direita.
    public SpriteRenderer ultimoCenario;

    [Header("Movimentação da Câmera")]

    // Controla a suavidade com que a câmera acompanha Maxwell.
    public float suavidade = 5f;

    // Guardam a altura e a profundidade originais para a câmera não sair do lugar nesses eixos.
    private float posicaoY, posicaoZ;

    // Guardam até onde a câmera pode andar sem mostrar áreas fora do cenário.
    private float limiteEsquerdo, limiteDireito;

    void Start()
    {
        // Salva a altura e a profundidade da câmera antes de começar a seguir Maxwell.
        posicaoY = transform.position.y;
        posicaoZ = transform.position.z;

        // Calcula metade da largura visível da câmera, considerando seu tamanho e proporção.
        float metadeLarguraCamera = Camera.main.orthographicSize * Camera.main.aspect;

        // Define o limite esquerdo para a câmera não mostrar o que existe antes do cenário.
        limiteEsquerdo = primeiroCenario.bounds.min.x + metadeLarguraCamera;

        // Define o limite direito para a câmera não mostrar o que existe depois do cenário.
        limiteDireito = ultimoCenario.bounds.max.x - metadeLarguraCamera;
    }

    void LateUpdate()
    {
        // Se Maxwell não está configurado, não tenta movimentar a câmera.
        if (jogador == null)
        {
            return;
        }

        // Pega a posição horizontal de Maxwell, respeitando os limites do cenário.
        float posicaoX = Mathf.Clamp(jogador.position.x, limiteEsquerdo, limiteDireito);

        // Monta a posição desejada sem alterar a altura e a profundidade da câmera.
        Vector3 posicaoDestino = new Vector3(posicaoX, posicaoY, posicaoZ);

        // Aproxima a câmera suavemente da posição desejada.
        transform.position = Vector3.Lerp(transform.position, posicaoDestino, suavidade * Time.deltaTime);
    }
}
