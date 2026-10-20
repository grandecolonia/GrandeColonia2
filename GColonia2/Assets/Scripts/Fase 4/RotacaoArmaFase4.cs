
using UnityEngine;
using UnityEngine.InputSystem;

public class RotacaoArmaFase4 : MonoBehaviour
{
    [Header("Referências")]
    public Camera cameraPrincipal;
    public Transform imagemArma;
    public Transform pontaDoCano;

    [Header("Rotação")]
    [Range(0, 90)] public float anguloMaximo = 75f;

    private Vector3 posicaoOriginal;
    private Vector3 escalaOriginal;

    void Start()
    {
        if (cameraPrincipal == null)
            cameraPrincipal = Camera.main;

        posicaoOriginal = imagemArma.localPosition;
        escalaOriginal = imagemArma.localScale;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 posicaoMouse = Mouse.current.position.ReadValue();

        Vector3 mouseMundo = cameraPrincipal.ScreenToWorldPoint(new Vector3(posicaoMouse.x, posicaoMouse.y, Mathf.Abs(cameraPrincipal.transform.position.z - transform.position.z)));

        Vector2 direcao = mouseMundo - transform.position;

        if (direcao.sqrMagnitude < 0.01f) return;

        // Espelha a arma quando mira para a direita
        bool olhandoDireita = direcao.x > 0;

        imagemArma.localScale = new Vector3(olhandoDireita ? -escalaOriginal.x : escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        imagemArma.localPosition = new Vector3(olhandoDireita ? -posicaoOriginal.x : posicaoOriginal.x, posicaoOriginal.y, posicaoOriginal.z);

        // Calcula o ângulo do mouse
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        // Impede a arma de apontar para baixo
        if (olhandoDireita)
        {
            angulo = Mathf.Clamp(angulo, 0, anguloMaximo);
        }
        else
        {
            if (angulo < 0) angulo += 360;

            angulo = Mathf.Clamp(angulo, 180 - anguloMaximo, 180);
        }

        // Alinha a ponta do cano com a direção desejada
        Vector3 posicaoCano = transform.InverseTransformPoint(pontaDoCano.position);
        float anguloCano = Mathf.Atan2(posicaoCano.y, posicaoCano.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angulo - anguloCano);
    }
}
