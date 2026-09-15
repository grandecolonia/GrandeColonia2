using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GuardaPerseguicao : MonoBehaviour
{
    [Header("Referências")]

    // Maxwell que o guarda vai perseguir.
    public Transform jogador;

    // Controle geral do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Visual")]

    // Filho Visual do guarda.
    public Transform visual;

    // Animator usado na caminhada.
    public Animator animator;

    private NavMeshAgent agente;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Configura o NavMeshAgent para funcionar no jogo 2D.
        agente.updateRotation = false;
        agente.updateUpAxis = false;
        agente.autoBraking = false;

        ColocarGuardaNoNavMesh();
    }

    void Update()
    {
        // O guarda só começa a perseguir quando o alerta estiver ativo.
        if (!controleLabirinto.alertaAtivo)
        {
            PararGuarda();
            return;
        }

        // Tenta colocar o guarda novamente no NavMesh caso ele saia dele.
        if (!agente.isOnNavMesh)
        {
            ColocarGuardaNoNavMesh();
            return;
        }

        // Atualiza o destino do guarda para a posição atual de Maxwell.
        agente.SetDestination(jogador.position);

        AtualizarVisual();
    }

    void ColocarGuardaNoNavMesh()
    {
        NavMeshHit hit;

        // Procura um ponto navegável próximo da posição atual do guarda.
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            agente.Warp(hit.position);
        }
    }

    void PararGuarda()
    {
        if (agente != null && agente.isOnNavMesh)
        {
            agente.ResetPath();
        }

        if (animator != null)
        {
            animator.SetBool("andando", false);
        }
    }

    void AtualizarVisual()
    {
        Vector2 direcao = new Vector2(agente.velocity.x, agente.velocity.y);

        bool andando = direcao.sqrMagnitude > 0.01f;

        if (animator != null)
        {
            animator.SetBool("andando", andando);
        }

        if (!andando || visual == null)
        {
            return;
        }

        // Gira o visual dependendo da direção em que o guarda está andando.
        if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
        {
            if (direcao.x > 0)
            {
                // Direita.
                visual.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                // Esquerda.
                visual.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (direcao.y > 0)
            {
                // Cima.
                visual.localRotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                // Baixo.
                visual.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Reinicia o labirinto se o guarda encostar no jogador.
        if (collision.CompareTag("Player"))
        {
            ReiniciarLabirinto();
        }
    }

    void ReiniciarLabirinto()
    {
        // Recarrega a cena atual.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}