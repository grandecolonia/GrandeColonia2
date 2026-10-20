
using System.Collections;
using UnityEngine;

public class SpawnAlvosFase4 : MonoBehaviour
{
    [Header("Pontos de Spawn")]
    public Transform[] pontosSpawn;

    [Header("Aliados")]
    public GameObject[] aliados;

    [Header("Aparição")]
    [Range(0, 100)] public int chanceAliado = 30;
    public float intervaloEntreAlvos = 2f;

    public FinalizacaoFase4 finalizacao;

    public IEnumerator GerarSequencia(GameObject[] inimigosDisponiveis, int quantidadeAlvos)
    {
        if (inimigosDisponiveis == null || inimigosDisponiveis.Length == 0)
        {
            Debug.LogWarning("Nenhum inimigo configurado para esta onda!");
            yield break;
        }

        for (int i = 0; i < quantidadeAlvos; i++)
        {
            // Sorteia entre aliado e inimigo
            bool gerarAliado = aliados != null && aliados.Length > 0 && Random.Range(0, 100) < chanceAliado;

            GameObject prefab;

            if (gerarAliado)
                prefab = aliados[Random.Range(0, aliados.Length)];
            else
                prefab = inimigosDisponiveis[Random.Range(0, inimigosDisponiveis.Length)];

            GameObject alvo = CriarAlvo(prefab);

            if (alvo == null)
                yield break;

            VidaInimigoFase4 inimigo = alvo.GetComponent<VidaInimigoFase4>();
            AliadoFase4 aliado = alvo.GetComponent<AliadoFase4>();

            if (inimigo == null && aliado == null)
            {
                Debug.LogError("O alvo precisa de um script de inimigo ou aliado!");
                Destroy(alvo);
                yield break;
            }

            float tempoRestante = inimigo != null ? inimigo.tempoNaTela : aliado.tempoNaTela;

            // Aguarda o alvo morrer ou sair da tela
            while (alvo != null && tempoRestante > 0f)
            {
                tempoRestante -= Time.deltaTime;
                yield return null;
            }

            if (alvo != null)
            {
                Destroy(alvo);

                if (inimigo != null)
{
    Debug.Log("O inimigo escapou!");

    if (finalizacao != null)
        finalizacao.RegistrarFuga();
}
else
{
    Debug.Log("O aliado saiu da tela.");
}
            }

            if (i < quantidadeAlvos - 1)
                yield return new WaitForSeconds(intervaloEntreAlvos);
        }
    }

    public GameObject CriarAlvo(GameObject prefab)
    {
        if (prefab == null || pontosSpawn == null || pontosSpawn.Length == 0)
        {
            Debug.LogWarning("Configure o personagem e os pontos de spawn!");
            return null;
        }

        int indice = Random.Range(0, pontosSpawn.Length);
        Transform ponto = pontosSpawn[indice];

        return Instantiate(prefab, ponto.position, Quaternion.identity, transform);
    }
}
