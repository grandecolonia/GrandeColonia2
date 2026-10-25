
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControleFaseCorrida : MonoBehaviour
{
    [Header("Duração das ondas")]

    // Tempo da primeira onda, quando aparecem somente os guardas.
    public float duracaoOnda1 = 20f;

    // Tempo da segunda onda, quando os guardas e o helicóptero atacam juntos.
    public float duracaoOnda2 = 25f;

    // Tempo de sobrevivência depois que os ataques terminam.
    public float duracaoFinal = 5f;

    [Header("Elementos da fase")]

    // Sistema responsável por criar os guardas durante a corrida.
    public SpawnGuardaFase2 spawnGuardas;

    // Helicóptero que começa a atacar na segunda onda.
    public GameObject helicoptero;

    [Header("Conclusão")]

    // Cena que será carregada quando Maxwell sobreviver até o final.
    public string cenaVitoria;

    // Marca se a fase já terminou para impedir vitória e derrota ao mesmo tempo.
    private bool terminou = false;

    // Permite que outros scripts descubram se a fase acabou.
    public bool FaseTerminou => terminou;

    void Start()
    {
        // Começa com o helicóptero escondido.
        helicoptero.SetActive(false);

        // Libera o aparecimento dos guardas desde o início.
        spawnGuardas.enabled = true;

        // Começa a sequência das ondas da fase.
        StartCoroutine(ControlarOndas());
    }

    IEnumerator ControlarOndas()
    {
        // ONDA 1: espera o tempo da primeira onda, com apenas os guardas.
        yield return new WaitForSeconds(duracaoOnda1);

        // Se a fase já terminou, não continua a sequência.
        if (terminou)
        {
            yield break;
        }

        // ONDA 2: ativa o helicóptero enquanto os guardas continuam aparecendo.
        helicoptero.SetActive(true);

        // Mantém a segunda onda acontecendo pelo tempo configurado.
        yield return new WaitForSeconds(duracaoOnda2);

        // Se Maxwell já perdeu, não tenta continuar a fase.
        if (terminou)
        {
            yield break;
        }

        // Desliga o sistema que cria novos guardas.
        spawnGuardas.enabled = false;

        // Esconde o helicóptero para encerrar os ataques.
        helicoptero.SetActive(false);

        // Procura todos os guardas que ainda estão na cena.
        foreach (GuardaFase2 guarda in FindObjectsByType<GuardaFase2>(FindObjectsSortMode.None))
        {
            // Remove os guardas restantes para deixar o caminho livre.
            Destroy(guarda.gameObject);
        }

        // Procura todas as bombas que ainda não desapareceram.
        foreach (BombaFase2 bomba in FindObjectsByType<BombaFase2>(FindObjectsSortMode.None))
        {
            // Destrói as bombas restantes para encerrar os perigos da fase.
            Destroy(bomba.gameObject);
        }

        // FUGA FINAL: deixa Maxwell correr mais um pouco sem novos ataques.
        yield return new WaitForSeconds(duracaoFinal);

        // Se Maxwell sobreviveu até aqui, conclui a fase.
        Vitoria();
    }

    public void Derrota()
    {
        // Se a fase já terminou, impede que outra derrota seja iniciada.
        if (terminou)
        {
            return;
        }

        // Marca que a fase acabou.
        terminou = true;

        // Interrompe as ondas para não continuar criando eventos depois da derrota.
        StopAllCoroutines();

        // Recarrega a cena atual para Maxwell tentar novamente.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Vitoria()
    {
        // Se a fase já terminou, não permite outra finalização.
        if (terminou)
        {
            return;
        }

        // Se nenhuma cena de vitória foi configurada, cancela a transição.
        if (string.IsNullOrEmpty(cenaVitoria))
        {
            return;
        }

        // Marca que Maxwell concluiu a fase.
        terminou = true;

        // Carrega a próxima cena configurada no Inspector.
        SceneManager.LoadScene(cenaVitoria);
    }
}
