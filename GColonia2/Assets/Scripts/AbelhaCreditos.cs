using UnityEngine;

public class AbelhaCreditos : MonoBehaviour
{
    public float velocidade = 5f;
    public float distanciaDescida = 1F;
    private Vector2 direcao = Vector2.right;
    private bool descendo = false;
    private float alturaInicial;

    void Start()
    {
        alturaInicial = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (descendo)
        {
           transform.Translate(Vector2.down * velocidade * Time.deltaTime);
           if (transform.position.y <= alturaInicial - distanciaDescida)
           {
                descendo = false;

                alturaInicial = transform.position.y;
            }
        } 
        else if (direcao == Vector2.right)
        {
            transform.Translate(
                Vector2.right * velocidade * Time.deltaTime
            );
        }
        else if (direcao == Vector2.left)
        {
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("A abelha entrou em: " + other.gameObject.name);
        Debug.Log("Tag: " + other.gameObject.tag);
        
        if (other.CompareTag("Nome"))
        {
            RevelarNome nome = other.GetComponent<RevelarNome>();

            if (nome != null)
            {
                nome.Revelar();
            }
        }

        if (other.CompareTag("PontoDireita"))
        {
            direcao = Vector2.left;

            descendo = true;

            Flip();
        }

        else if (other.CompareTag("PontoEsquerda"))
        {
            direcao = Vector2.right;

            descendo = true;

            Flip();
        }
    }

    void Flip()
    {
        Vector3 escala = transform.localScale;

        escala.x *= -1;

        transform.localScale = escala;
    }
}
