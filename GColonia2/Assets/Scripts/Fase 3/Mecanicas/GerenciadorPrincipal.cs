
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GerenciadorPrincipal : MonoBehaviour
{
    [Header("Partículas")]

    // Partículas que aparecem quando Maxwell entra na área tóxica.
    public GameObject particulas;

    [Header("Personagem")]

    // Sprite utilizado para mostrar os efeitos de dano.
    public SpriteRenderer personagem;

    // Velocidade com que Maxwell pisca quando está com pouco oxigênio.
    public float velocidadePiscar = 5f;

    [Header("Controle de Oxigênio")]

    // Quantidade de oxigênio disponível no começo da fase.
    public float oxigenio = 100f;

    // Quantidade de oxigênio perdida por segundo normalmente.
    public float perdaOxigenioNormal = 2f;

    // Quantidade adicional de oxigênio perdida na área tóxica.
    public float perdaOxigenioToxico = 10f;

    // Texto que mostra a quantidade de oxigênio na interface.
    public TextMeshProUGUI oxigenioText;

    [Header("Movimentação")]

    // Script responsável pela velocidade de Maxwell.
    public MovimentacaoPersonagem vel;

    // Guarda se Maxwell já entrou na área tóxica.
    private bool ativo = false;

    // Velocidade máxima utilizada no cálculo do movimento.
    public float velMaxima = 5f;

    // Velocidade mínima quando o oxigênio estiver muito baixo.
    public float velMinima = 2f;

    [Header("Eventos da Fase")]

    // Guarda se Maxwell já caiu na água.
    private bool caiunaAgua = false;

    // Objeto da escada que aparece quando o guarda é ativado.
    public GameObject boxEscada;

    // Objeto visual do guarda que aparece durante a perseguição.
    public GameObject guardaVisivel;

    // Sistema responsável por ativar a perseguição do guarda.
    public SeguindoGuarda guarda;

    void Start()
    {
        // Começa com as partículas tóxicas escondidas.
        particulas.SetActive(false);

        // Esconde a escada até o evento correspondente acontecer.
        boxEscada.SetActive(false);

        // Começa com o guarda escondido.
        guardaVisivel.SetActive(false);
    }

    void Update()
    {
        // Diminui o oxigênio de Maxwell continuamente.
        oxigenio -= perdaOxigenioNormal * Time.deltaTime;

        // Se Maxwell entrou na área tóxica, aumenta a perda de oxigênio.
        if (ativo)
        {
            // Aplica a perda adicional causada pelas partículas tóxicas.
            oxigenio -= perdaOxigenioToxico * Time.deltaTime;

            // Calcula a quantidade de oxigênio em uma escala de zero a um.
            float porcentagemOxigenio = oxigenio / 100f;

            // Reduz a velocidade de Maxwell conforme o oxigênio diminui.
            vel.velocidade = Mathf.Lerp(velMinima, velMaxima, porcentagemOxigenio);
        }

        // Impede que o oxigênio fique abaixo de zero ou acima de cem.
        oxigenio = Mathf.Clamp(oxigenio, 0f, 100f);

        // Se o oxigênio chegou a trinta ou menos, ativa o aviso de perigo.
        if (oxigenio <= 30f)
        {
            // Deixa o texto do oxigênio vermelho.
            oxigenioText.color = Color.red;

            // Calcula a transparência para fazer Maxwell piscar.
            float alpha = Mathf.PingPong(Time.time * velocidadePiscar, 1f);

            // Aplica o efeito de piscar no sprite do personagem.
            personagem.color = new Color(personagem.color.r, personagem.color.g, personagem.color.b, alpha);
        }
        else
        {
            // Mantém o texto branco enquanto o oxigênio estiver acima de trinta.
            oxigenioText.color = Color.white;

            // Deixa Maxwell completamente visível.
            personagem.color = new Color(personagem.color.r, personagem.color.g, personagem.color.b, 1f);
        }

        // Atualiza o texto mostrando a quantidade de oxigênio restante.
        oxigenioText.text = "Oxigênio: " + Mathf.Round(oxigenio).ToString() + "%";

        // Se o oxigênio acabou, reinicia a fase.
        if (oxigenio <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se Maxwell entrou na área das partículas, ativa o efeito tóxico.
        if (other.gameObject.CompareTag("Particulas"))
        {
            // Mostra as partículas na cena.
            particulas.SetActive(true);

            // Começa a aplicar a perda adicional de oxigênio.
            ativo = true;
        }

        // Se Maxwell entrou na água, inicia a animação de dano.
        if (other.gameObject.CompareTag("Agua"))
        {
            StartCoroutine(AtivarDano());
        }

        // Se Maxwell entrou na área que libera o guarda, começa a perseguição.
        if (other.gameObject.CompareTag("ativarGuarda"))
        {
            // Mostra a escada que estava escondida.
            boxEscada.SetActive(true);

            // Mostra o guarda na cena.
            guardaVisivel.SetActive(true);

            // Avisa ao guarda que ele já pode perseguir Maxwell.
            guarda.AtivarGuarda();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Se Maxwell encostou no guarda, inicia o efeito de dano.
        if (collision.gameObject.CompareTag("Guarda"))
        {
            StartCoroutine(DanoGuarda());
        }
    }

    IEnumerator AtivarDano()
    {
        // Começa a contar a duração da animação de dano pela água.
        float tempo = 0f;

        // Mantém Maxwell piscando durante aproximadamente dois segundos.
        while (tempo < 2f)
        {
            // Deixa Maxwell verde para representar o dano.
            personagem.color = Color.green;

            // Mantém a cor por um pequeno intervalo.
            yield return new WaitForSeconds(0.15f);

            // Devolve a cor branca ao personagem.
            personagem.color = Color.white;

            // Espera novamente antes de repetir o efeito.
            yield return new WaitForSeconds(0.15f);

            // Atualiza o tempo da animação.
            tempo += 0.3f;
        }

        // Reinicia a fase depois da animação de dano.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DanoGuarda()
    {
        // Começa a contar a duração da animação de dano pelo guarda.
        float tempo = 0f;

        // Mantém Maxwell piscando durante aproximadamente dois segundos.
        while (tempo < 2f)
        {
            // Deixa Maxwell vermelho para representar o dano.
            personagem.color = Color.red;

            // Mantém a cor por um pequeno intervalo.
            yield return new WaitForSeconds(0.15f);

            // Devolve a cor branca ao personagem.
            personagem.color = Color.white;

            // Espera novamente antes de repetir o efeito.
            yield return new WaitForSeconds(0.15f);

            // Atualiza o tempo da animação.
            tempo += 0.3f;
        }

        // Reinicia a fase depois da animação de dano.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
