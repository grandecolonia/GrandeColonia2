using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GerenciadorPrincipal : MonoBehaviour
{
    [Header("Particulas")]
    public GameObject particulas;
    public SpriteRenderer personagem;
    public float velocidadePiscar = 5f;

    [Header("Controle de Oxigênio")]
    public float oxigenio = 100f;
    public float perdaOxigenioNormal = 2f;
    public float perdaOxigenioToxico = 10f;
    public TextMeshProUGUI oxigenioText;

    [Header("Movimentação")]
    public MovimentacaoPersonagem vel;
    private bool ativo = false;
    public float velMaxima = 5f;
    public float velMinima = 2f;

    bool caiunaAgua = false;

    public GameObject boxEscada;
    public GameObject guardaVisivel;

    public SeguindoGuarda guarda;

    void Start()
    {
        particulas.SetActive(false);
        boxEscada.SetActive(false);
        guardaVisivel.SetActive(false);
    }

    void Update()
    {

        oxigenio -= perdaOxigenioNormal * Time.deltaTime;

        if (ativo)
        {

            oxigenio -= perdaOxigenioToxico * Time.deltaTime;
            float porcentagemOxigenio = oxigenio / 100f;
            vel.velocidade = Mathf.Lerp(velMinima, velMaxima, porcentagemOxigenio);
        }

        oxigenio = Mathf.Clamp(oxigenio, 0f, 100f);

        if (oxigenio <= 30f)
        {
            oxigenioText.color = Color.red;
            float alpha = Mathf.PingPong(Time.time * velocidadePiscar, 1f);
            personagem.color = new Color(personagem.color.r, personagem.color.g, personagem.color.b, alpha);
        }
        else
        {
            oxigenioText.color = Color.white;
            personagem.color = new Color(personagem.color.r, personagem.color.g, personagem.color.b, 1f);
        }

        oxigenioText.text = "Oxigênio: " + Mathf.Round(oxigenio).ToString() + "%";

        if (oxigenio <= 0f)
        {
            Debug.Log("Oxigênio acabou!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Particulas"))
        {
            particulas.SetActive(true);
            ativo = true;
        }
        if (other.gameObject.CompareTag("Agua"))
        {
            StartCoroutine(AtivarDano());
        }

        if (other.gameObject.CompareTag("ativarGuarda"))
        {
            boxEscada.SetActive(true);
            guardaVisivel.SetActive(true);
            guarda.AtivarGuarda();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Guarda"))
        {
            Debug.Log("Colidiu com o guarda!");
            StartCoroutine(DanoGuarda());
        }
    }

    IEnumerator AtivarDano()
    {
        float tempo = 0f;

        while (tempo < 2f)
        {
            personagem.color = Color.green;
            yield return new WaitForSeconds(0.15f);

            personagem.color = Color.white;
            yield return new WaitForSeconds(0.15f);

            tempo += 0.3f;
        
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DanoGuarda()
    {
        float tempo = 0f;

        while (tempo < 2f)
        {
            personagem.color = Color.red;
            yield return new WaitForSeconds(0.15f);

            personagem.color = Color.white;
            yield return new WaitForSeconds(0.15f);

            tempo += 0.3f;
        
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}