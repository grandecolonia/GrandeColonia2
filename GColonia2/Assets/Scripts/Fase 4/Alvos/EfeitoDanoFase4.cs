
using System.Collections;
using UnityEngine;

public class EfeitoDanoFase4 : MonoBehaviour
{
    [Header("Referências")]

    // SpriteRenderer que terá a cor alterada quando receber um tiro.
    public SpriteRenderer imagem;

    [Header("Efeito")]

    // Cor utilizada para mostrar visualmente que o personagem recebeu dano.
    public Color corDano = new Color(1f, 0.4f, 0.4f);

    // Tempo que o personagem permanece com a cor de dano.
    public float duracao = 0.12f;

    // Guarda a cor original do personagem.
    private Color corOriginal;

    // Guarda o efeito atual para evitar animações sobrepostas.
    private Coroutine efeitoAtual;

    void Awake()
    {
        // Se nenhuma imagem foi configurada, procura o SpriteRenderer do objeto.
        if (imagem == null)
        {
            imagem = GetComponent<SpriteRenderer>();
        }

        // Guarda a cor original para restaurar depois do efeito.
        if (imagem != null)
        {
            corOriginal = imagem.color;
        }
    }

    public void Piscar()
    {
        // Se não existe uma imagem, não tenta executar o efeito.
        if (imagem == null)
        {
            return;
        }

        // Interrompe o efeito anterior caso o personagem receba outro tiro.
        if (efeitoAtual != null)
        {
            StopCoroutine(efeitoAtual);
        }

        // Começa novamente o efeito de dano.
        efeitoAtual = StartCoroutine(EfeitoPiscar());
    }

    IEnumerator EfeitoPiscar()
    {
        // Muda a cor do personagem para indicar que recebeu um tiro.
        imagem.color = corDano;

        // Mantém a cor durante o tempo configurado.
        yield return new WaitForSeconds(duracao);

        // Devolve a cor original do personagem.
        imagem.color = corOriginal;

        // Marca que o efeito terminou.
        efeitoAtual = null;
    }
}
