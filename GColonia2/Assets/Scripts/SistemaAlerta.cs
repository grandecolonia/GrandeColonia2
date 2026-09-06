using UnityEngine;
using System.Collections;

public class SistemaAlerta : MonoBehaviour
{
    [Header("Controle")]

    public ControleLabirinto controleLabirinto;

    [Header("Visual")]

    public GameObject alertaVisual;
    public CanvasGroup canvasGroup;

    [Header("Áudio")]

    public AudioSource audioSource;
    public AudioClip sirene;

    [Header("Duração")]

    // Tempo que a sirene e o aviso ficam ativos.
    public float duracaoAlarme = 5f;

    private bool alertaIniciado = false;
    private bool alarmeAtivo = false;

    void Update()
    {
        if (controleLabirinto.alertaAtivo && !alertaIniciado)
        {
            IniciarAlerta();
        }
    }

    void IniciarAlerta()
    {
        alertaIniciado = true;
        alarmeAtivo = true;

        alertaVisual.SetActive(true);

        audioSource.clip = sirene;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(PiscarAlerta());
        StartCoroutine(DesligarAlarme());
    }

    IEnumerator PiscarAlerta()
    {
        while (alarmeAtivo)
        {
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(0.4f);

            canvasGroup.alpha = 0.25f;
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator DesligarAlarme()
    {
        yield return new WaitForSeconds(duracaoAlarme);

        alarmeAtivo = false;

        // Para somente o som e o aviso visual.
        audioSource.Stop();
        alertaVisual.SetActive(false);
    }
}