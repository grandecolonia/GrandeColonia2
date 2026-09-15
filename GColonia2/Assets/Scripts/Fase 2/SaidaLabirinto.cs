using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaidaLabirinto : MonoBehaviour
{
    [Header("Referências")]

    // Referências usadas para controlar a saída do labirinto.
    public ControleLabirinto controleLabirinto;
    public GameObject indicadorSaida;
    public GameObject luzPorta;
    public CanvasGroup fadeSaida;
    public MovimentacaoLabirinto movimentacao;

    [Header("Cena")]

    // Cena carregada depois que a saída terminar.
    public string proximaCena;

    [Header("Efeito")]

    // Controla o tempo da luz e do fade final.
    public float duracaoLuz = 0.7f;
    public float duracaoFade = 1.2f;

    private bool finalizando = false;

    void Start()
    {
        // Começa com a luz escondida e sem fade na tela.
        luzPorta.SetActive(false);
        fadeSaida.alpha = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Só continua se quem entrou for Maxwell.
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        // Só permite sair depois de pegar o cartão e evita repetir a finalização.
        if (!controleLabirinto.temCartao || finalizando)
        {
            return;
        }

        StartCoroutine(FinalizarFase(collision.gameObject));
    }

    IEnumerator FinalizarFase(GameObject jogador)
    {
        finalizando = true;

        // Impede Maxwell de continuar andando.
        if (movimentacao != null)
        {
            movimentacao.enabled = false;
        }

        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Esconde a seta porque Maxwell já encontrou a saída.
        indicadorSaida.SetActive(false);

        // Mostra a luz que vai crescer a partir da porta.
        luzPorta.SetActive(true);

        Vector3 escalaInicial = luzPorta.transform.localScale;
        Vector3 escalaFinal = escalaInicial * 3f;

        float tempo = 0f;

        // Aumenta a luz aos poucos.
        while (tempo < duracaoLuz)
        {
            tempo += Time.deltaTime;

            float progresso = tempo / duracaoLuz;

            luzPorta.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, progresso);

            yield return null;
        }

        // Faz o branco cobrir a tela inteira.
        tempo = 0f;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;

            fadeSaida.alpha = Mathf.Clamp01(tempo / duracaoFade);

            yield return null;
        }

        // Vai para a próxima cena.
        SceneManager.LoadScene(proximaCena);
    }
}