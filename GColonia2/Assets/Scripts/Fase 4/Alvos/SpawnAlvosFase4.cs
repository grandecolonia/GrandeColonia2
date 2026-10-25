
using System.Collections;
using UnityEngine;

public class SpawnAlvosFase4 : MonoBehaviour
{
    [Header("Pontos de Spawn")]

    // Locais onde os personagens podem aparecer durante a fase.
    public Transform[] pontosSpawn;

    [Header("Aliados")]

    // Prefabs dos rebeldes aliados que podem aparecer entre os inimigos.
    public GameObject[] aliados;

    [Header("Aparição")]

    // Porcentagem de chance de aparecer um aliado em vez de um inimigo.
    [Range(0, 100)]
    public int chanceAliado = 30;

    // Tempo de espera entre um alvo e o próximo.
    public float intervaloEntreAlvos = 2f;

    [Header("Finalização")]

    // Sistema responsável por registrar os inimigos que conseguem escapar.
    public FinalizacaoFase4 finalizacao;

    public IEnumerator GerarSequencia(GameObject[] inimigosDisponiveis, int quantidadeAlvos)
    {
        // Se não existem inimigos configurados para a onda, cancela a sequência.
        if (inimigosDisponiveis == null || inimigosDisponiveis.Length == 0)
        {
            yield break;
        }

        // Continua criando personagens até atingir a quantidade da onda.
        for (int i = 0; i < quantidadeAlvos; i++)
        {
            // Sorteia se o próximo personagem será um aliado ou um inimigo.
            bool gerarAliado = aliados != null && aliados.Length > 0 && Random.Range(0, 100) < chanceAliado;

            // Guarda o prefab que será utilizado para criar o próximo alvo.
            GameObject prefab;

            // Se o sorteio escolheu um aliado, pega um dos rebeldes disponíveis.
            if (gerarAliado)
            {
                prefab = aliados[Random.Range(0, aliados.Length)];
            }
            else
            {
                // Caso contrário, escolhe um dos inimigos configurados para a onda.
                prefab = inimigosDisponiveis[Random.Range(0, inimigosDisponiveis.Length)];
            }

            // Cria o personagem em um dos pontos de spawn.
            GameObject alvo = CriarAlvo(prefab);

            // Se não conseguiu criar o personagem, interrompe a sequência.
            if (alvo == null)
            {
                yield break;
            }

            // Verifica se o personagem criado é um inimigo.
            VidaInimigoFase4 inimigo = alvo.GetComponent<VidaInimigoFase4>();

            // Verifica se o personagem criado é um aliado.
            AliadoFase4 aliado = alvo.GetComponent<AliadoFase4>();

            // Se não possui nenhum dos dois scripts, o prefab está configurado incorretamente.
            if (inimigo == null && aliado == null)
            {
                Debug.LogError("O alvo precisa de um script de inimigo ou aliado!");

                // Remove o personagem inválido.
                Destroy(alvo);
                yield break;
            }

            // Define quanto tempo o personagem pode permanecer na tela.
            float tempoRestante = inimigo != null ? inimigo.tempoNaTela : aliado.tempoNaTela;

            // Espera até o alvo ser destruído ou seu tempo acabar.
            while (alvo != null && tempoRestante > 0f)
            {
                // Diminui o tempo disponível a cada frame.
                tempoRestante -= Time.deltaTime;

                // Espera o próximo frame para verificar novamente.
                yield return null;
            }

            // Se o personagem ainda existe depois do tempo, remove ele da cena.
            if (alvo != null)
            {
                // Destrói o personagem que não foi atingido a tempo.
                Destroy(alvo);

                // Se era um inimigo, registra que ele conseguiu escapar.
                if (inimigo != null)
                {
                    if (finalizacao != null)
                    {
                        finalizacao.RegistrarFuga();
                    }
                }
            }

            // Se ainda existem alvos para criar, espera antes do próximo.
            if (i < quantidadeAlvos - 1)
            {
                yield return new WaitForSeconds(intervaloEntreAlvos);
            }
        }
    }

    public GameObject CriarAlvo(GameObject prefab)
    {
        // Se falta o prefab ou os pontos de spawn, não tenta criar o personagem.
        if (prefab == null || pontosSpawn == null || pontosSpawn.Length == 0)
        {
            return null;
        }

        // Sorteia um dos pontos disponíveis para o personagem aparecer.
        int indice = Random.Range(0, pontosSpawn.Length);

        // Guarda o ponto escolhido para o spawn.
        Transform ponto = pontosSpawn[indice];

        // Cria o personagem na posição sorteada, dentro do objeto de spawn.
        return Instantiate(prefab, ponto.position, Quaternion.identity, transform);
    }
}
