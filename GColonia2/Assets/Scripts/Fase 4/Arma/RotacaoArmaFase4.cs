
using UnityEngine;
using UnityEngine.InputSystem;

public class RotacaoArmaFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Câmera utilizada para descobrir a posição do mouse dentro do jogo.
    public Camera cameraPrincipal;

    // Objeto visual da arma que será espelhado conforme a direção.
    public Transform imagemArma;

    // Ponto da arma utilizado para alinhar corretamente a direção do disparo.
    public Transform pontaDoCano;

    [Header("Rotação")]

    // Ângulo máximo que a arma pode alcançar ao mirar para cima.
    [Range(0, 90)]
    public float anguloMaximo = 75f;

    // Guarda a posição original da imagem da arma.
    private Vector3 posicaoOriginal;

    // Guarda a escala original para espelhar a arma corretamente.
    private Vector3 escalaOriginal;

    void Start()
    {
        // Se nenhuma câmera foi configurada, utiliza a câmera principal.
        if (cameraPrincipal == null)
        {
            cameraPrincipal = Camera.main;
        }

        // Salva a posição original da imagem antes de começar a mirar.
        posicaoOriginal = imagemArma.localPosition;

        // Salva o tamanho original da arma para restaurar ao mudar de lado.
        escalaOriginal = imagemArma.localScale;
    }

    void Update()
    {
        // Se não existe mouse disponível, não tenta atualizar a mira.
        if (Mouse.current == null)
        {
            return;
        }

        // Lê a posição atual do mouse utilizando o New Input System.
        Vector2 posicaoMouse = Mouse.current.position.ReadValue();

        // Converte a posição do mouse da tela para o espaço do jogo.
        Vector3 mouseMundo = cameraPrincipal.ScreenToWorldPoint(new Vector3(posicaoMouse.x, posicaoMouse.y, Mathf.Abs(cameraPrincipal.transform.position.z - transform.position.z)));

        // Calcula a direção entre a arma e o cursor do jogador.
        Vector2 direcao = mouseMundo - transform.position;

        // Se a direção é muito pequena, mantém a rotação atual.
        if (direcao.sqrMagnitude < 0.01f)
        {
            return;
        }

        // Descobre se Maxwell está mirando para a direita.
        bool olhandoDireita = direcao.x > 0;

        // Espelha a imagem da arma quando a mira aponta para a direita.
        imagemArma.localScale = new Vector3(olhandoDireita ? -escalaOriginal.x : escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);

        // Ajusta a posição da imagem para acompanhar o espelhamento.
        imagemArma.localPosition = new Vector3(olhandoDireita ? -posicaoOriginal.x : posicaoOriginal.x, posicaoOriginal.y, posicaoOriginal.z);

        // Calcula o ângulo do mouse em relação à arma.
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        // Limita a rotação conforme o lado em que Maxwell está mirando.
        if (olhandoDireita)
        {
            // Ao mirar para a direita, permite apontar apenas para frente ou para cima.
            angulo = Mathf.Clamp(angulo, 0f, anguloMaximo);
        }
        else
        {
            // Ajusta os ângulos negativos para trabalhar com valores positivos.
            if (angulo < 0)
            {
                angulo += 360f;
            }

            // Ao mirar para a esquerda, impede que a arma aponte para baixo.
            angulo = Mathf.Clamp(angulo, 180f - anguloMaximo, 180f);
        }

        // Descobre a posição da ponta do cano em relação ao pivô da arma.
        Vector3 posicaoCano = transform.InverseTransformPoint(pontaDoCano.position);

        // Calcula o ângulo atual da ponta do cano.
        float anguloCano = Mathf.Atan2(posicaoCano.y, posicaoCano.x) * Mathf.Rad2Deg;

        // Gira a arma considerando o ângulo desejado e o alinhamento do cano.
        transform.rotation = Quaternion.Euler(0f, 0f, angulo - anguloCano);
    }
}
