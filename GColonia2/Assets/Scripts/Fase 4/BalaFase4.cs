
using UnityEngine;

public class BalaFase4 : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 12f;
    public float tempoDeVida = 3f;

    [Header("Dano")]
    public int dano = 10;

    private Rigidbody2D rb;
    private Vector2 direcao;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    public void Inicializar(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    
void OnTriggerEnter2D(Collider2D outro)
{
    GeneralFase4 general = outro.GetComponent<GeneralFase4>();
    VidaInimigoFase4 inimigo = outro.GetComponent<VidaInimigoFase4>();

    // Faz inimigos e General piscarem ao receber tiro
    if (general != null || inimigo != null)
    {
        EfeitoDanoFase4 efeito = outro.GetComponent<EfeitoDanoFase4>();

        if (efeito != null)
            efeito.Piscar();

            if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarImpacto();
    }

    if (general != null)
    {
        general.ReceberDano(dano);
        Destroy(gameObject);
        return;
    }

    if (inimigo != null)
    {
        inimigo.ReceberDano(dano);
        Destroy(gameObject);
        return;
    }

    AliadoFase4 aliado = outro.GetComponent<AliadoFase4>();

    if (aliado != null)
    {
        aliado.ReceberTiro();
        Destroy(gameObject);
    }
}

}
