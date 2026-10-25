
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BombaFase2 : MonoBehaviour
{
    [Header("Movimento")]

    // Velocidade com que a bomba vai cair até o ponto escolhido.
    public float velocidadeQueda = 7f;

    [Header("Explosão")]

    // Animator responsável por mostrar a explosão da bomba.
    public Animator animator;

    // Distância que a explosão consegue atingir ao redor da bomba.
    public float raioExplosao = 1f;

    // Tempo que a animação de explosão vai ficar acontecendo.
    public float duracaoExplosao = 0.6f;

    // Guarda o ponto onde a bomba deve cair.
    private Vector3 destino;

    // Indica se a bomba já recebeu um destino para começar a cair.
    private bool configurada = false;

    // Impede que a mesma bomba exploda várias vezes.
    private bool explodiu = false;

    public void ConfigurarDestino(Vector3 novoDestino)
    {
        // Guarda o ponto onde a bomba precisa chegar.
        destino = novoDestino;

        // Libera a movimentação da bomba até o destino.
        configurada = true;
    }

    void Update()
    {
        // Se ainda não recebeu um destino ou já explodiu, não faz mais nada.
        if (!configurada || explodiu)
        {
            return;
        }

        // Move a bomba até o destino sem ultrapassar o ponto escolhido.
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadeQueda * Time.deltaTime);

        // Se chegou perto o suficiente do destino, começa a explosão.
        if (Vector3.Distance(transform.position, destino) < 0.05f)
        {
            StartCoroutine(Explodir());
        }
    }

    IEnumerator Explodir()
    {
        // Marca que a bomba explodiu para impedir outra explosão.
        explodiu = true;

        // Interrompe a movimentação para a bomba ficar parada durante a explosão.
        configurada = false;

        // Se existe um Animator, manda reproduzir a animação de explosão.
        if (animator != null)
        {
            animator.SetTrigger("Explodir");
        }

        // Guarda se Maxwell foi atingido pela explosão.
        bool atingiuJogador = false;

        // Procura todos os Colliders2D dentro do raio da explosão.
        Collider2D[] atingidos = Physics2D.OverlapCircleAll(transform.position, raioExplosao);

        // Verifica cada objeto encontrado dentro da área da explosão.
        foreach (Collider2D atingido in atingidos)
        {
            // Se encontrou um objeto com a tag Player, considera que Maxwell foi atingido.
            if (atingido.CompareTag("Player"))
            {
                atingiuJogador = true;
                break;
            }
        }

        // Espera a animação da explosão acontecer antes de finalizar.
        yield return new WaitForSeconds(duracaoExplosao);

        // Se Maxwell foi atingido, inicia a derrota.
        if (atingiuJogador)
        {
            // Procura o sistema que controla a fase de corrida.
            ControleFaseCorrida controle = FindFirstObjectByType<ControleFaseCorrida>();

            if (controle != null)
            {
                // Se encontrou o controlador, deixa ele cuidar da derrota.
                controle.Derrota();
            }
            else
            {
                // Se não existe controlador, reinicia a cena como alternativa.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Remove a bomba depois que a explosão termina.
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Mostra no Editor o tamanho da explosão quando a bomba está selecionada.
        Gizmos.DrawWireSphere(transform.position, raioExplosao);
    }
}
