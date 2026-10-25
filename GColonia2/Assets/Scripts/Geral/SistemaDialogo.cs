
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SistemaDialogo : MonoBehaviour
{
    [Header("Portão - Fase 3")]

    // Fundo do cenário que poderá ser trocado durante o diálogo.
    public SpriteRenderer fundoCena;

    // Nova imagem que vai substituir o fundo original.
    public Sprite novoFundo;

    // Define em qual fala o fundo será trocado. O -1 deixa essa troca desativada.
    public int falaParaTrocarFundo = -1;

    // Cena que será carregada quando o diálogo terminar, caso esteja configurada.
    public string proximaCena;

    [Header("Caixa de diálogo")]

    // Caixa que aparece enquanto os personagens estão conversando.
    public GameObject caixaDialogo;

    [Header("Elementos da caixa")]

    // Nome do personagem que está falando.
    public TMP_Text nomePersonagem;

    // Texto onde as falas vão aparecer.
    public TMP_Text texto;

    // Imagem que mostra o rosto de quem está falando.
    public Image rosto;

    // Botão usado para completar ou avançar as falas.
    public Button botaoAvancar;

    [Header("Velocidade do texto")]

    // Tempo de espera entre uma letra e outra durante a digitação.
    public float velocidadeTexto = 0.03f;

    // Guarda os dados do diálogo que está acontecendo.
    private DialogoData dialogoAtual;

    // Controla qual fala está sendo mostrada no momento.
    private int falaAtual = 0;

    // Indica se o texto ainda está aparecendo letra por letra.
    private bool escrevendo = false;

    // Guarda a corrotina de digitação para poder interrompê-la.
    private Coroutine escrevendoTexto;

    // Referência ao Maxwell para bloquear sua movimentação durante a conversa.
    private MovimentoPersonagem jogador;

    void Awake()
    {
        // Começa com a caixa de diálogo escondida.
        caixaDialogo.SetActive(false);
    }

    public void IniciarDialogo(DialogoData novoDialogo)
    {
        // Procura o Maxwell que está presente na cena atual.
        jogador = FindFirstObjectByType<MovimentoPersonagem>();

        // Impede Maxwell de andar enquanto a conversa acontece.
        if (jogador != null)
        {
            jogador.podeMover = false;
        }

        // Se não existe um diálogo configurado, avisa o erro e cancela.
        if (novoDialogo == null)
        {
            Debug.LogError("Nenhum DialogoData foi configurado!");
            return;
        }

        // Guarda o diálogo escolhido e começa pela primeira fala.
        dialogoAtual = novoDialogo;
        falaAtual = 0;

        // Abre a caixa de diálogo na tela.
        caixaDialogo.SetActive(true);

        // Começa a mostrar a primeira fala.
        MostrarFala();
    }

    void MostrarFala()
    {
        // Se não existem mais falas, encerra a conversa.
        if (falaAtual >= dialogoAtual.falas.Length)
        {
            FecharDialogo();
            return;
        }

        // Pega os dados da fala que precisa aparecer agora.
        DialogoData.Fala fala = dialogoAtual.falas[falaAtual];

        // Se chegou à fala configurada, troca o fundo do cenário.
        if (fundoCena != null && novoFundo != null && falaAtual == falaParaTrocarFundo)
        {
            fundoCena.sprite = novoFundo;
        }

        // Atualiza o nome e o rosto do personagem que está falando.
        nomePersonagem.text = fala.nomePersonagem;
        rosto.sprite = fala.rosto;

        // Se ainda existe uma digitação anterior, interrompe antes de começar outra.
        if (escrevendoTexto != null)
        {
            StopCoroutine(escrevendoTexto);
        }

        // Começa a escrever a nova fala letra por letra.
        escrevendoTexto = StartCoroutine(EscreverTexto(fala.texto));
    }

    IEnumerator EscreverTexto(string fala)
    {
        // Marca que o texto começou a ser escrito.
        escrevendo = true;

        // Limpa a fala anterior antes de escrever a próxima.
        texto.text = "";

        // Passa por cada letra da fala para criar o efeito de digitação.
        foreach (char letra in fala)
        {
            // Adiciona mais uma letra ao texto que está aparecendo.
            texto.text += letra;

            // Espera um pouquinho antes de mostrar a próxima letra.
            yield return new WaitForSeconds(velocidadeTexto);
        }

        // Terminou de escrever, então libera o avanço normal do diálogo.
        escrevendo = false;
    }

    public void Avancar()
    {
        // Se não existe nenhum diálogo acontecendo, não faz nada.
        if (dialogoAtual == null)
        {
            return;
        }

        // Se a fala ainda está sendo escrita, mostra tudo de uma vez.
        if (escrevendo)
        {
            // Interrompe o efeito de digitação.
            StopCoroutine(escrevendoTexto);

            // Mostra a fala completa sem precisar esperar pelas outras letras.
            texto.text = dialogoAtual.falas[falaAtual].texto;

            // Marca que a digitação terminou.
            escrevendo = false;
            return;
        }

        // Passa para a próxima fala da conversa.
        falaAtual++;

        // Atualiza a caixa com a fala seguinte ou encerra o diálogo.
        MostrarFala();
    }

    void FecharDialogo()
    {
        // Quando termina a conversa, devolve o movimento ao Maxwell.
        if (jogador != null)
        {
            jogador.podeMover = true;
        }

        // Esconde a caixa e limpa o texto antigo.
        caixaDialogo.SetActive(false);
        texto.text = "";

        // Limpa o diálogo atual para não continuar uma conversa encerrada.
        dialogoAtual = null;

        // Se existe uma próxima cena configurada, carrega ela.
        if (!string.IsNullOrEmpty(proximaCena))
        {
            SceneManager.LoadScene(proximaCena);
        }
    }
}
