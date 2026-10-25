
using System.Collections;
using UnityEngine;

public class EntradaAlvoFase4 : MonoBehaviour
{
    [Header("Animação")]

    // Tempo necessário para completar a animação de entrada.
    public float duracao = 0.2f;

    // Tamanho inicial do alvo em relação à escala original.
    public float escalaInicial = 0.8f;

    // Guarda o tamanho original do personagem.
    private Vector3 escalaOriginal;

    void Awake()
    {
        // Salva a escala original antes de começar a animação.
        escalaOriginal = transform.localScale;
    }

    void Start()
    {
        // Inicia a animação quando o alvo aparece na cena.
        StartCoroutine(AnimarEntrada());
    }

    IEnumerator AnimarEntrada()
    {
        // Começa a contar o tempo da animação.
        float tempo = 0f;

        // Continua aumentando o alvo até completar o tempo configurado.
        while (tempo < duracao)
        {
            // Atualiza o tempo da animação.
            tempo += Time.deltaTime;

            // Calcula o progresso de zero a um.
            float progresso = Mathf.Clamp01(tempo / Mathf.Max(0.01f, duracao));

            // Aumenta o alvo suavemente até seu tamanho original.
            transform.localScale = escalaOriginal * Mathf.Lerp(escalaInicial, 1f, progresso);

            // Espera o próximo frame para continuar crescendo.
            yield return null;
        }

        // Garante que o alvo termine exatamente com o tamanho original.
        transform.localScale = escalaOriginal;
    }
}
