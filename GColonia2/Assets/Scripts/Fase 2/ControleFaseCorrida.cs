
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleFaseCorrida : MonoBehaviour
{
    [Header("Duração das ondas")]
    public float duracaoOnda1 = 20f;
    public float duracaoOnda2 = 25f;
    public float duracaoFinal = 5f;

    [Header("Elementos da fase")]
    public SpawnGuardaFase2 spawnGuardas;
    public GameObject helicoptero;

    [Header("Conclusão")]
    public string cenaVitoria;

    private bool terminou = false;
    public bool FaseTerminou => terminou;

    void Start()
    {
        helicoptero.SetActive(false);
        spawnGuardas.enabled = true;

        StartCoroutine(ControlarOndas());
    }

    IEnumerator ControlarOndas()
    {
        // Onda 1: somente guardas.
        yield return new WaitForSeconds(duracaoOnda1);

        if (terminou) yield break;

        // Onda 2: guardas e helicóptero.
        helicoptero.SetActive(true);

        yield return new WaitForSeconds(duracaoOnda2);

        if (terminou) yield break;

        // Encerra os ataques e remove os perigos restantes.
        spawnGuardas.enabled = false;
        helicoptero.SetActive(false);

        foreach (GuardaFase2 guarda in FindObjectsByType<GuardaFase2>(FindObjectsSortMode.None))
        {
            Destroy(guarda.gameObject);
        }

        foreach (BombaFase2 bomba in FindObjectsByType<BombaFase2>(FindObjectsSortMode.None))
        {
            Destroy(bomba.gameObject);
        }

        // Fuga final.
        yield return new WaitForSeconds(duracaoFinal);

        Vitoria();
    }

    public void Derrota()
    {
        if (terminou) return;

        terminou = true;
        StopAllCoroutines();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Vitoria()
    {
        if (terminou) return;

        if (string.IsNullOrEmpty(cenaVitoria))
        {
            Debug.LogWarning("Configure a cena de vitória no Inspector!");
            return;
        }

        terminou = true;
        SceneManager.LoadScene(cenaVitoria);
    }
}
