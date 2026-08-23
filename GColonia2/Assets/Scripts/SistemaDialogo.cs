using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SistemaDialogo : MonoBehaviour
{
    [Header("Caixa de diálogo")]

    public GameObject caixaDialogo;

    [Header("Elementos da caixa")]

    public TMP_Text nomePersonagem;
    public TMP_Text texto;
    public Image rosto;
    public Button botaoAvancar;

    [Header("Velocidade do texto")]

    public float velocidadeTexto = 0.03f;

    // Guardam o diálogo atual e qual fala está sendo mostrada.
    private DialogoData dialogoAtual;
    private int falaAtual = 0;

    // Controlam a animação de escrita do texto.
    private bool escrevendo = false;
    private Coroutine escrevendoTexto;

    // Guarda o personagem para bloquear seu movimento durante o diálogo.
    private MovimentoPersonagem jogador;

    void Start()
    {
        // A caixa de diálogo começa escondida.
        caixaDialogo.SetActive(false);
    }

    public void IniciarDialogo(DialogoData novoDialogo)
    {
        // Procura o jogador presente na cena e bloqueia seu movimento.
        jogador = FindFirstObjectByType<MovimentoPersonagem>();

        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        // Impede o início caso nenhum diálogo tenha sido configurado.
        if (novoDialogo == null)
        {
            Debug.LogError("Nenhum DialogoData foi configurado!");
            return;
        }

        dialogoAtual = novoDialogo;
        falaAtual = 0;

        caixaDialogo.SetActive(true);

        MostrarFala();
    }

    void MostrarFala()
    {
        // Fecha o diálogo quando não existem mais falas.
        if (falaAtual >= dialogoAtual.falas.Length)
        {
            FecharDialogo();
            return;
        }

        // Recupera e mostra os dados da fala atual.
        DialogoData.Fala fala = dialogoAtual.falas[falaAtual];

        nomePersonagem.text = fala.nomePersonagem;
        rosto.sprite = fala.rosto;

        // Interrompe a escrita anterior antes de iniciar uma nova.
        if (escrevendoTexto != null)
        {
            StopCoroutine(escrevendoTexto);
        }

        escrevendoTexto = StartCoroutine(EscreverTexto(fala.texto));
    }

    IEnumerator EscreverTexto(string fala)
    {
        // Mostra o texto uma letra por vez.
        escrevendo = true;
        texto.text = "";

        foreach (char letra in fala)
        {
            texto.text += letra;
            yield return new WaitForSeconds(velocidadeTexto);
        }

        escrevendo = false;
    }

    public void Avancar()
    {
        if (dialogoAtual == null)
        {
            return;
        }

        // Caso o texto ainda esteja sendo escrito, mostra a fala inteira.
        if (escrevendo)
        {
            StopCoroutine(escrevendoTexto);

            texto.text = dialogoAtual.falas[falaAtual].texto;

            escrevendo = false;
            return;
        }

        // Avança para a próxima fala do diálogo.
        falaAtual++;

        MostrarFala();
    }

    void FecharDialogo()
    {
        // Libera o movimento do jogador ao terminar o diálogo.
        if (jogador != null)
        {
            jogador.podeMover = true;
        }

        caixaDialogo.SetActive(false);
        texto.text = "";

        dialogoAtual = null;
    }
}