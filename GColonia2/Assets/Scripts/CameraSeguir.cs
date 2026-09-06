using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    // Jogador acompanhado pela câmera.
    public Transform jogador;

    // Primeiro e último cenário da fase.
    public SpriteRenderer primeiroCenario;
    public SpriteRenderer ultimoCenario;

    // Controla a suavidade do movimento.
    public float suavidade = 5f;

    private float posicaoY;
    private float posicaoZ;
    private float limiteEsquerdo;
    private float limiteDireito;

    void Start()
    {
        posicaoY = transform.position.y;
        posicaoZ = transform.position.z;

        // Calcula metade da largura visível da câmera.
        float metadeLarguraCamera = Camera.main.orthographicSize * Camera.main.aspect;

        // Impede que a câmera mostre qualquer área fora dos cenários.
        limiteEsquerdo = primeiroCenario.bounds.min.x + metadeLarguraCamera;
        limiteDireito = ultimoCenario.bounds.max.x - metadeLarguraCamera;
    }

    void LateUpdate()
    {
        if (jogador == null)
        {
            return;
        }

        float posicaoX = Mathf.Clamp(jogador.position.x, limiteEsquerdo, limiteDireito);
        Vector3 posicaoDestino = new Vector3(posicaoX, posicaoY, posicaoZ);

        transform.position = Vector3.Lerp(transform.position, posicaoDestino, suavidade * Time.deltaTime);
    }
}