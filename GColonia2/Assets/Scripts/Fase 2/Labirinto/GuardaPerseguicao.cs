
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GuardaPerseguicao : MonoBehaviour
{
    [Header("Referências")]

    // Maxwell, que será perseguido pelos guardas quando o alerta começar.
    public Transform jogador;

    // Sistema que informa se a perseguição já foi liberada.
    public ControleLabirinto controleLabirinto;

    [Header("Visual")]

    // Objeto visual que será girado conforme a direção do guarda.
    public Transform visual;

    // Animator que controla a animação de caminhada do guarda.
    public Animator animator;

    // Componente responsável por encontrar caminhos pelo labirinto.
    private NavMeshAgent agente;

    void Start()
    {
        // Pega o NavMeshAgent do próprio guarda.
        agente = GetComponent<NavMeshAgent>();

        // Impede o NavMeshAgent de girar automaticamente o guarda.
        agente.updateRotation = false;

        // Adapta o NavMeshAgent para trabalhar no plano 2D.
        agente.updateUpAxis = false;

        // Evita que o guarda fique freando automaticamente perto do destino.
        agente.autoBraking = false;

        // Procura uma posição válida no NavMesh para começar a navegação.
        ColocarGuardaNoNavMesh();
    }

    void Update()
    {
        // Se o alerta ainda não começou, o guarda não pode perseguir Maxwell.
        if (!controleLabirinto.alertaAtivo)
        {
            // Se existe algum caminho ativo, cancela a movimentação do guarda.
            if (agente.isOnNavMesh && agente.hasPath)
            {
                agente.ResetPath();
            }

            // Para a animação de caminhada enquanto não existe perseguição.
            if (animator != null)
            {
                animator.SetBool("andando", false);
            }

            return;
        }

        // Se o guarda não está em uma área navegável, tenta colocá-lo no NavMesh.
        if (!agente.isOnNavMesh)
        {
            ColocarGuardaNoNavMesh();
            return;
        }

        // Atualiza o destino para a posição atual do Maxwell.
        agente.SetDestination(jogador.position);

        // Atualiza a animação e a direção visual conforme o movimento.
        AtualizarVisual();
    }

    void ColocarGuardaNoNavMesh()
    {
        // Guarda o ponto navegável encontrado perto do personagem.
        NavMeshHit hit;

        // Procura um ponto válido do NavMesh em um raio de duas unidades.
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            // Coloca o guarda nesse ponto para permitir que ele navegue.
            agente.Warp(hit.position);
        }
    }

    void PararGuarda()
    {
        // Se o guarda está no NavMesh, apaga o caminho que estava seguindo.
        if (agente != null && agente.isOnNavMesh)
        {
            agente.ResetPath();
        }

        // Desliga a animação de caminhada.
        if (animator != null)
        {
            animator.SetBool("andando", false);
        }
    }

    void AtualizarVisual()
    {
        // Pega a direção em que o guarda está realmente se movimentando.
        Vector2 direcao = new Vector2(agente.velocity.x, agente.velocity.y);

        // Verifica se o guarda possui velocidade suficiente para estar andando.
        bool andando = direcao.sqrMagnitude > 0.01f;

        // Atualiza a animação de caminhada conforme o movimento.
        if (animator != null)
        {
            animator.SetBool("andando", andando);
        }

        // Se o guarda está parado ou não possui Visual, não tenta girá-lo.
        if (!andando || visual == null)
        {
            return;
        }

        // Se o movimento horizontal é maior, vira o guarda para esquerda ou direita.
        if (Mathf.Abs(direcao.x) > Mathf.Abs(direcao.y))
        {
            // Se está andando para a direita, vira o Visual para esse lado.
            if (direcao.x > 0)
            {
                visual.localRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                // Se está andando para a esquerda, vira para o lado contrário.
                visual.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
        else
        {
            // Se está subindo, faz o guarda olhar para cima.
            if (direcao.y > 0)
            {
                visual.localRotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                // Se está descendo, faz o guarda olhar para baixo.
                visual.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o alerta não está ativo, o guarda não pode capturar Maxwell.
        if (!controleLabirinto.alertaAtivo)
        {
            return;
        }

        // Se o guarda encostou em Maxwell durante a perseguição, reinicia o labirinto.
        if (collision.CompareTag("Player"))
        {
            ReiniciarLabirinto();
        }
    }

    void ReiniciarLabirinto()
    {
        // Recarrega a cena atual para Maxwell tentar novamente.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
