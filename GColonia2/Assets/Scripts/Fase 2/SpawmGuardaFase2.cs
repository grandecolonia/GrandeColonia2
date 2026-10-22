
using UnityEngine;
using System.Collections;

public class SpawnGuardaFase2 : MonoBehaviour
{
    [Header("Guarda")]
    public GameObject prefabGuarda;

    [Header("Tempo")]
    public float tempoInicial = 3f;
    public float intervaloMinimo = 4f;
    public float intervaloMaximo = 7f;

    void OnEnable()
    {
        StartCoroutine(GerarGuardas());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator GerarGuardas()
    {
        yield return new WaitForSeconds(tempoInicial);

        while (true)
        {
            Instantiate(prefabGuarda, transform.position, Quaternion.identity);

            float intervalo = Random.Range(intervaloMinimo, intervaloMaximo);
            yield return new WaitForSeconds(intervalo);
        }
    }
}
