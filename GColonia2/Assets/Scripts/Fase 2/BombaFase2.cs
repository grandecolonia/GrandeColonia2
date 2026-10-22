
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombaFase2 : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeQueda = 7f;

    [Header("Explosão")]
    public Animator animator;
    public float raioExplosao = 1f;
    public float duracaoExplosao = 0.6f;

    private Vector3 destino;
    private bool configurada = false;
    private bool explodiu = false;

    public void ConfigurarDestino(Vector3 novoDestino)
    {
        destino = novoDestino;
        configurada = true;
    }

    void Update()
    {
        if (!configurada || explodiu) return;

        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadeQueda * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.05f)
        {
            StartCoroutine(Explodir());
        }
    }

    IEnumerator Explodir()
{
    explodiu = true;
    configurada = false;

    // Inicia a animação de explosão.
    if (animator != null)
    {
        animator.SetTrigger("Explodir");
    }

    bool atingiuJogador = false;

    // Verifica se Maxwell está dentro da explosão.
    Collider2D[] atingidos = Physics2D.OverlapCircleAll(transform.position, raioExplosao);

    foreach (Collider2D atingido in atingidos)
    {
        if (atingido.CompareTag("Player"))
        {
            atingiuJogador = true;
            break;
        }
    }

    // Espera a animação aparecer antes de continuar.
    yield return new WaitForSeconds(duracaoExplosao);

    if (atingiuJogador)
    {
        ControleFaseCorrida controle = FindFirstObjectByType<ControleFaseCorrida>();

        if (controle != null)
        {
            controle.Derrota();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    Destroy(gameObject);
}

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, raioExplosao);
    }
}
