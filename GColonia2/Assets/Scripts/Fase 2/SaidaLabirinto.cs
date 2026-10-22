using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaidaLabirinto : MonoBehaviour
{
    [Header("Referências")]

    public ControleLabirinto controleLabirinto;
    public GameObject indicadorSaida;
    public GameObject luzPorta;
    public CanvasGroup fadeSaida;
    public MovimentacaoLabirinto movimentacao;

    [Header("Cena")]

    public string proximaCena;

    [Header("Transição")]

    // Controla o crescimento da luz e o clareamento da tela.
    public float duracaoLuz = 1f;
    public float duracaoFade = 1f;
    public float escalaFinalLuz = 6f;

    private bool finalizando = false;
    private Vector3 escalaInicialLuz;
    private SpriteRenderer spriteLuz;
    private Color corInicialLuz;

    void Start()
    {
        escalaInicialLuz = luzPorta.transform.localScale;
        spriteLuz = luzPorta.GetComponent<SpriteRenderer>();

        if (spriteLuz != null)
        {
            corInicialLuz = spriteLuz.color;
        }

        luzPorta.SetActive(false);
        fadeSaida.alpha = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (!controleLabirinto.temCartao || finalizando)
        {
            return;
        }

        StartCoroutine(FinalizarFase(collision.gameObject));
    }

    IEnumerator FinalizarFase(GameObject jogador)
    {
        finalizando = true;

        // Impede Maxwell de se movimentar durante a saída.
        if (movimentacao != null)
        {
            movimentacao.podeMover = false;
            movimentacao.enabled = false;
        }

        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        indicadorSaida.SetActive(false);

        // Prepara a luz para começar suavemente.
        luzPorta.transform.localScale = escalaInicialLuz;

        if (spriteLuz != null)
        {
            Color cor = corInicialLuz;
            cor.a = 0f;
            spriteLuz.color = cor;
        }

        luzPorta.SetActive(true);

        float tempo = 0f;

        // Faz a luz crescer e aparecer progressivamente.
        while (tempo < duracaoLuz)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(tempo / duracaoLuz);
            float suavidade = Mathf.SmoothStep(0f, 1f, progresso);

            luzPorta.transform.localScale = Vector3.Lerp(escalaInicialLuz, escalaInicialLuz * escalaFinalLuz, suavidade);

            if (spriteLuz != null)
            {
                Color cor = corInicialLuz;
                cor.a = suavidade * corInicialLuz.a;
                spriteLuz.color = cor;
            }

            // Começa a clarear a tela enquanto a luz ainda cresce.
            fadeSaida.alpha = Mathf.Clamp01((progresso - 0.55f) / 0.45f) * 0.35f;

            yield return null;
        }

        // Termina de cobrir a tela com branco.
        tempo = 0f;
        float alphaInicial = fadeSaida.alpha;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(tempo / duracaoFade);
            float suavidade = Mathf.SmoothStep(0f, 1f, progresso);

            fadeSaida.alpha = Mathf.Lerp(alphaInicial, 1f, suavidade);

            yield return null;
        }

        fadeSaida.alpha = 1f;

        SceneManager.LoadScene(proximaCena);
    }
}