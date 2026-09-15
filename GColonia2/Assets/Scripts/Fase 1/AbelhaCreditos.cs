using UnityEngine;

public class AbelhaCreditos : MonoBehaviour
{
    // Configurações de movimento da abelha.
    public float velocidade = 5f;
    public float distanciaDescida = 1F;

    // Controlam a direção e o momento em que a abelha está descendo.
    private Vector2 direcao = Vector2.right;
    private bool descendo = false;
    private float alturaInicial;

    void Start()
    {
        // Guarda a altura em que a abelha começou.
        alturaInicial = transform.position.y;
    }

    void Update()
    {
        // Primeiro, a abelha desce até alcançar a distância configurada.
        if (descendo)
        {
            transform.Translate(Vector2.down * velocidade * Time.deltaTime);

            if (transform.position.y <= alturaInicial - distanciaDescida)
            {
                descendo = false;
                alturaInicial = transform.position.y;
            }
        }
        // Quando não está descendo, continua se movendo horizontalmente.
        else if (direcao == Vector2.right)
        {
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);
        }
        else if (direcao == Vector2.left)
        {
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mostra no Console qual objeto a abelha encontrou.
        Debug.Log("A abelha entrou em: " + other.gameObject.name);
        Debug.Log("Tag: " + other.gameObject.tag);

        // Revela um nome quando a abelha passa por um objeto com a tag "Nome".
        if (other.CompareTag("Nome"))
        {
            RevelarNome nome = other.GetComponent<RevelarNome>();

            if (nome != null)
            {
                nome.Revelar();
            }
        }

        // Ao chegar no ponto da direita, começa a voltar para a esquerda.
        if (other.CompareTag("PontoDireita"))
        {
            direcao = Vector2.left;
            descendo = true;
            Flip();
        }
        // Ao chegar no ponto da esquerda, começa a voltar para a direita.
        else if (other.CompareTag("PontoEsquerda"))
        {
            direcao = Vector2.right;
            descendo = true;
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a escala horizontal para virar o sprite.
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}