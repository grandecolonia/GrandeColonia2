using UnityEngine;

public class AnimacaoSeta : MonoBehaviour
{
    // Configura o tamanho, a distância e a velocidade da animação da seta.
    public float aumento = 1.1f;
    public float distanciaDireita = 0.3f;
    public float velocidade = 2f;

    // Guardam a escala e a posição originais da seta.
    private Vector3 escalaInicial;
    private Vector3 posicaoInicial;

    void Start()
    {
        escalaInicial = transform.localScale;
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        // Verifica se o jogador começou a se mover para os lados.
        float movseta = Input.GetAxisRaw("Horizontal");

        // Quando o jogador se move, a seta deixa de aparecer.
        if (movseta != 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Cria um movimento suave que varia entre 0 e 1.
        float movimento = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f;

        // Aumenta e diminui a seta suavemente.
        float fatorEscala = Mathf.Lerp(1f, aumento, movimento);
        transform.localScale = escalaInicial * fatorEscala;

        // Move a seta suavemente para a direita e depois retorna.
        float deslocamento = Mathf.Lerp(0f, distanciaDireita, movimento);
        transform.localPosition = posicaoInicial + new Vector3(deslocamento, 0f, 0f);
    }
}