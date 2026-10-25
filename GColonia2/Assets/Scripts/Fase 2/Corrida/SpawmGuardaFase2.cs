
using UnityEngine;
using System.Collections;

public class SpawnGuardaFase2 : MonoBehaviour
{
    [Header("Guarda")]

    // Prefab do guarda que será criado durante a corrida.
    public GameObject prefabGuarda;

    [Header("Tempo")]

    // Tempo de espera antes do primeiro guarda aparecer.
    public float tempoInicial = 3f;

    // Menor intervalo possível entre o aparecimento dos guardas.
    public float intervaloMinimo = 4f;

    // Maior intervalo possível entre o aparecimento dos guardas.
    public float intervaloMaximo = 7f;

    // Permite que o helicóptero pause a criação de novos guardas.
    public bool pausarSpawn = false;

    void OnEnable()
    {
        // Começa a criar guardas quando o sistema é ativado.
        StartCoroutine(GerarGuardas());
    }

    void OnDisable()
    {
        // Interrompe a criação de guardas quando o sistema é desativado.
        StopAllCoroutines();
    }

    IEnumerator GerarGuardas()
    {
        // Espera o tempo inicial antes de criar o primeiro guarda.
        yield return new WaitForSeconds(tempoInicial);

        // Continua criando guardas enquanto o sistema estiver funcionando.
        while (true)
        {
            // Se o helicóptero está preparando um ataque, espera liberar o spawn.
            while (pausarSpawn)
            {
                // Verifica novamente no próximo frame se os guardas podem aparecer.
                yield return null;
            }

            // Cria um guarda na posição do objeto responsável pelo spawn.
            Instantiate(prefabGuarda, transform.position, Quaternion.identity);

            // Sorteia quanto tempo deve esperar antes do próximo guarda.
            float intervalo = Random.Range(intervaloMinimo, intervaloMaximo);

            // Aguarda o intervalo sorteado antes de repetir a sequência.
            yield return new WaitForSeconds(intervalo);
        }
    }
}
