
using System.Collections;
using UnityEngine;

public class EfeitoDanoFase4 : MonoBehaviour
{
    [Header("Referências")]
    public SpriteRenderer imagem;

    [Header("Efeito")]
    public Color corDano = new Color(1f, 0.4f, 0.4f);
    public float duracao = 0.12f;

    private Color corOriginal;
    private Coroutine efeitoAtual;

    void Awake()
    {
        if (imagem == null)
            imagem = GetComponent<SpriteRenderer>();

        if (imagem != null)
            corOriginal = imagem.color;
    }

    public void Piscar()
    {
        if (imagem == null) return;

        if (efeitoAtual != null)
            StopCoroutine(efeitoAtual);

        efeitoAtual = StartCoroutine(EfeitoPiscar());
    }

    IEnumerator EfeitoPiscar()
    {
        // Muda a cor rapidamente
        imagem.color = corDano;

        yield return new WaitForSeconds(duracao);

        imagem.color = corOriginal;
        efeitoAtual = null;
    }
}
