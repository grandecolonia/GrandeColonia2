
using UnityEngine;

public class BalaFase4 : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade com que a bala se movimenta pelo cenário.
    public float velocidade = 12f;

    // Tempo máximo que a bala pode permanecer na cena.
    public float tempoDeVida = 3f;

    [Header("Dano")]

    // Quantidade de dano que a bala causa nos inimigos e no General.
    public int dano = 10;

    // Rigidbody usado para movimentar a bala.
    private Rigidbody2D rb;

    // Guarda a direção em que a bala deve seguir.
    private Vector2 direcao;

    void Awake()
    {
        // Pega o Rigidbody2D da própria bala.
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Destrói a bala depois do tempo configurado para não acumular objetos na cena.
        Destroy(gameObject, tempoDeVida);
    }

    public void Inicializar(Vector2 novaDirecao)
    {
        // Guarda a direção do disparo sem deixar a intensidade alterar a velocidade.
        direcao = novaDirecao.normalized;
    }

    void FixedUpdate()
    {
        // Movimenta a bala na direção escolhida, respeitando a velocidade configurada.
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        // Verifica se a bala atingiu o General.
        GeneralFase4 general = outro.GetComponent<GeneralFase4>();

        // Verifica se a bala atingiu um guarda inimigo.
        VidaInimigoFase4 inimigo = outro.GetComponent<VidaInimigoFase4>();

        // Se atingiu um inimigo ou o General, reproduz os efeitos do impacto.
        if (general != null || inimigo != null)
        {
            // Procura o componente responsável por fazer o personagem piscar.
            EfeitoDanoFase4 efeito = outro.GetComponent<EfeitoDanoFase4>();

            // Se encontrou o efeito, faz o personagem piscar ao receber o tiro.
            if (efeito != null)
            {
                efeito.Piscar();
            }

            // Reproduz o som de impacto caso o sistema de áudio esteja disponível.
            if (AudioFase4.instancia != null)
            {
                AudioFase4.instancia.TocarImpacto();
            }
        }

        // Se a bala atingiu o General, aplica o dano e remove o projétil.
        if (general != null)
        {
            general.ReceberDano(dano);

            // Destrói a bala para impedir outros impactos.
            Destroy(gameObject);
            return;
        }

        // Se atingiu um guarda, aplica o dano correspondente.
        if (inimigo != null)
        {
            inimigo.ReceberDano(dano);

            // Remove a bala depois de atingir o inimigo.
            Destroy(gameObject);
            return;
        }

        // Verifica se a bala atingiu um rebelde aliado.
        AliadoFase4 aliado = outro.GetComponent<AliadoFase4>();

        // Se atingiu um aliado, registra o tiro e remove a bala.
        if (aliado != null)
        {
            aliado.ReceberTiro();

            // Destrói o projétil depois de atingir o aliado.
            Destroy(gameObject);
        }
    }
}
