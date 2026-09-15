using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    // Jogador que a câmera vai acompanhar.
    public Transform jogador;

    // Primeiro e último cenário usados como limite da câmera.
    public SpriteRenderer primeiroCenario;
    public SpriteRenderer ultimoCenario;

    // Controla a suavidade do movimento da câmera.
    public float suavidade = 5f;

    private float posicaoY;
    private float posicaoZ;
    private float limiteEsquerdo;
    private float limiteDireito;

    void Start()
    {
        posicaoY = transform.position.y;
        posicaoZ = transform.position.z;

        // Calcula metade da largura que a câmera consegue enxergar.
        float metadeLarguraCamera = Camera.main.orthographicSize * Camera.main.aspect;

        // Define os limites para a câmera não mostrar áreas fora do cenário.
        limiteEsquerdo = primeiroCenario.bounds.min.x + metadeLarguraCamera;
        limiteDireito = ultimoCenario.bounds.max.x - metadeLarguraCamera;
    }

    void LateUpdate()
    {
        if (jogador == null)
        {
            return;
        }

        // Limita a posição da câmera entre o começo e o fim do cenário.
        float posicaoX = Mathf.Clamp(jogador.position.x, limiteEsquerdo, limiteDireito);

        Vector3 posicaoDestino = new Vector3(posicaoX, posicaoY, posicaoZ);

        // Move a câmera suavemente até a posição do jogador.
        transform.position = Vector3.Lerp(transform.position, posicaoDestino, suavidade * Time.deltaTime);
    }
}