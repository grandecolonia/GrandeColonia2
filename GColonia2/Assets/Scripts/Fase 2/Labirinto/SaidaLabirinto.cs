
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaidaLabirinto : MonoBehaviour
{
    [Header("Referências")]

    // Sistema que informa se Maxwell já conseguiu o cartão.
    public ControleLabirinto controleLabirinto;

    // Indicador que mostra onde fica a saída do labirinto.
    public GameObject indicadorSaida;

    // Luz que aparece e aumenta durante a transição.
    public GameObject luzPorta;

    // CanvasGroup usado para clarear a tela antes de trocar de cena.
    public CanvasGroup fadeSaida;

    // Script usado para bloquear a movimentação de Maxwell.
    public MovimentacaoLabirinto movimentacao;

    [Header("Cena")]

    // Cena carregada quando Maxwell conclui o labirinto.
    public string proximaCena;

    [Header("Transição")]

    // Tempo que a luz demora para crescer.
    public float duracaoLuz = 1f;

    // Tempo necessário para o branco cobrir completamente a tela.
    public float duracaoFade = 1f;

    // Multiplicador do tamanho final da luz.
    public float escalaFinalLuz = 6f;

    // Impede que a saída seja ativada várias vezes.
    private bool finalizando = false;

    // Guarda o tamanho original da luz.
    private Vector3 escalaInicialLuz;

    // SpriteRenderer utilizado para controlar a transparência da luz.
    private SpriteRenderer spriteLuz;

    // Guarda a cor original para preservar a aparência da luz.
    private Color corInicialLuz;

    void Start()
    {
        // Salva o tamanho original da luz antes de começar a transição.
        escalaInicialLuz = luzPorta.transform.localScale;

        // Procura o SpriteRenderer da luz.
        spriteLuz = luzPorta.GetComponent<SpriteRenderer>();

        // Se encontrou o sprite, guarda sua cor original.
        if (spriteLuz != null)
        {
            corInicialLuz = spriteLuz.color;
        }

        // Começa com a luz escondida.
        luzPorta.SetActive(false);

        // Deixa a tela sem o efeito branco no início.
        fadeSaida.alpha = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se não foi Maxwell que entrou na saída, ignora a colisão.
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // Se Maxwell não possui o cartão ou a saída já foi ativada, não continua.
        if (!controleLabirinto.temCartao || finalizando)
        {
            return;
        }

        // Começa a animação de conclusão do labirinto.
        StartCoroutine(FinalizarFase(collision.gameObject));
    }

    IEnumerator FinalizarFase(GameObject jogador)
    {
        // Marca que a saída já foi ativada.
        finalizando = true;

        // Bloqueia o movimento de Maxwell durante a transição.
        if (movimentacao != null)
        {
            movimentacao.podeMover = false;
            movimentacao.enabled = false;
        }

        // Procura o Rigidbody2D de Maxwell.
        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        // Se encontrou o Rigidbody, interrompe o movimento e a rotação.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Esconde o indicador porque Maxwell já encontrou a saída.
        indicadorSaida.SetActive(false);

        // Volta a luz para o tamanho original antes de começar.
        luzPorta.transform.localScale = escalaInicialLuz;

        // Prepara a luz para aparecer suavemente.
        if (spriteLuz != null)
        {
            Color cor = corInicialLuz;
            cor.a = 0f;
            spriteLuz.color = cor;
        }

        // Mostra a luz, inicialmente transparente.
        luzPorta.SetActive(true);

        // Começa a contar o tempo da animação da luz.
        float tempo = 0f;

        // Aumenta a luz até completar o tempo configurado.
        while (tempo < duracaoLuz)
        {
            // Atualiza o tempo da animação.
            tempo += Time.deltaTime;

            // Calcula quanto da animação já aconteceu.
            float progresso = Mathf.Clamp01(tempo / duracaoLuz);

            // Deixa o crescimento e o aparecimento mais suaves.
            float suavidade = Mathf.SmoothStep(0f, 1f, progresso);

            // Faz a luz crescer progressivamente.
            luzPorta.transform.localScale = Vector3.Lerp(escalaInicialLuz, escalaInicialLuz * escalaFinalLuz, suavidade);

            // Aumenta a transparência da luz conforme ela cresce.
            if (spriteLuz != null)
            {
                Color cor = corInicialLuz;
                cor.a = suavidade * corInicialLuz.a;
                spriteLuz.color = cor;
            }

            // Começa a clarear a tela quando a luz já estiver crescendo.
            fadeSaida.alpha = Mathf.Clamp01((progresso - 0.55f) / 0.45f) * 0.35f;

            // Espera o próximo frame para continuar a animação.
            yield return null;
        }

        // Reinicia a contagem para completar o clareamento da tela.
        tempo = 0f;

        // Guarda a transparência atual para continuar a transição sem cortes.
        float alphaInicial = fadeSaida.alpha;

        // Continua clareando até a tela ficar completamente branca.
        while (tempo < duracaoFade)
        {
            // Atualiza o tempo do fade.
            tempo += Time.deltaTime;

            // Calcula o progresso dessa segunda parte da transição.
            float progresso = Mathf.Clamp01(tempo / duracaoFade);

            // Suaviza o clareamento da tela.
            float suavidade = Mathf.SmoothStep(0f, 1f, progresso);

            // Aumenta a transparência do branco até cobrir toda a tela.
            fadeSaida.alpha = Mathf.Lerp(alphaInicial, 1f, suavidade);

            // Espera o próximo frame para continuar o efeito.
            yield return null;
        }

        // Garante que a tela termine completamente branca.
        fadeSaida.alpha = 1f;

        // Carrega a próxima cena depois da animação.
        SceneManager.LoadScene(proximaCena);
    }
}
