using UnityEngine;
using System.Collections;

public class SpawnGuardaFase2 : MonoBehaviour
{
    [Header("Guarda")]

    // Prefab do guarda que será criado durante a fase.
    public GameObject prefabGuarda;

    [Header("Tempo")]

    // Tempo até o primeiro guarda aparecer.
    public float tempoInicial = 3f;

    // Intervalo mínimo e máximo entre um guarda e outro.
    public float intervaloMinimo = 4f;
    public float intervaloMaximo = 7f;

    void Start()
    {
        StartCoroutine(GerarGuardas());
    }

    IEnumerator GerarGuardas()
    {
        // Espera alguns segundos antes de criar o primeiro guarda.
        yield return new WaitForSeconds(tempoInicial);

        while (true)
        {
            // Cria um novo guarda na posição do Spawn.
            Instantiate(prefabGuarda, transform.position, Quaternion.identity);

            // Escolhe um tempo aleatório antes do próximo guarda.
            float intervalo = Random.Range(intervaloMinimo, intervaloMaximo);

            yield return new WaitForSeconds(intervalo);
        }
    }
}