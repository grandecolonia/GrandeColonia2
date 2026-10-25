
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimacaoSeta : MonoBehaviour
{
    [Header("Animação")]

    // Define o quanto a seta vai aumentar de tamanho.
    public float aumento = 1.1f;

    // Distância que a seta vai percorrer para a direita.
    public float distanciaDireita = 0.3f;

    // Controla a velocidade da animação.
    public float velocidade = 2f;

    [Header("Input")]

    // Usa a mesma ação de movimento do jogador para saber quando esconder a seta.
    public InputActionReference mover;

    // Guardam o tamanho e a posição originais da seta.
    private Vector3 escalaInicial;
    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva o tamanho original para não deixar a seta crescendo sem parar.
        escalaInicial = transform.localScale;

        // Guarda a posição original para a seta não sair andando pelo cenário.
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Pega a direção que o jogador está apertando.
        Vector2 movimentoJogador = mover.action.ReadValue<Vector2>();

        // Se o jogador apertar para a esquerda ou direita, esconde a seta.
        if (Mathf.Abs(movimentoJogador.x) > 0.01f)
        {
            gameObject.SetActive(false);
            return;
        }

        // Usa o seno para criar um movimento suave que vai de 0 até 1.
        float movimento = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;

        // Faz a seta aumentar e diminuir sem perder o tamanho original.
        float fatorEscala = Mathf.Lerp(1f, aumento, movimento);
        transform.localScale = escalaInicial * fatorEscala;

        // Calcula o quanto a seta vai andar para a direita.
        float deslocamento = Mathf.Lerp(0f, distanciaDireita, movimento);

        // Move a seta e depois traz de volta para a posição original.
        transform.localPosition = posicaoInicial + new Vector3(deslocamento, 0f, 0f);
    }
}
