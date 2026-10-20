
using System.Collections;
using UnityEngine;

public class EntradaAlvoFase4 : MonoBehaviour
{
    [Header("Animação")]
    public float duracao = 0.2f;
    public float escalaInicial = 0.8f;

    private Vector3 escalaOriginal;

    void Awake()
    {
        escalaOriginal = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(AnimarEntrada());
    }

    IEnumerator AnimarEntrada()
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(tempo / Mathf.Max(0.01f, duracao));

            transform.localScale = escalaOriginal * Mathf.Lerp(escalaInicial, 1f, progresso);

            yield return null;
        }

        transform.localScale = escalaOriginal;
    }
}
