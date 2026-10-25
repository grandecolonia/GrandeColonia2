
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteracaoCarteiro : MonoBehaviour
{
    [Header("Diálogo")]

    // Sistema responsável por mostrar as falas do carteiro.
    public SistemaDialogo sistemaDialogo;

    // Guarda as falas que o carteiro vai apresentar nesta cena.
    public DialogoData dialogo;

    [Header("Personagem")]

    // Script usado para bloquear a movimentação de Maxwell durante a conversa.
    public MovimentacaoPersonagem personagem;

    [Header("Próxima cena")]

    // Cena que será carregada depois que o diálogo terminar.
    public string cenaDestino;

    // Impede que a conversa comece várias vezes.
    private bool dialogoIniciado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e ainda não conversou com o carteiro, inicia o diálogo.
        if (collision.CompareTag("Player") && !dialogoIniciado)
        {
            // Marca que a conversa já começou para impedir outra ativação.
            dialogoIniciado = true;

            // Bloqueia a movimentação de Maxwell enquanto o carteiro fala.
            personagem.podeMover = false;

            // Manda o sistema de diálogo mostrar as falas configuradas.
            sistemaDialogo.IniciarDialogo(dialogo);

            // Começa a esperar o fim da conversa para mudar de cena.
            StartCoroutine(EsperarFimDialogo());
        }
    }

    IEnumerator EsperarFimDialogo()
    {
        // Espera a caixa de diálogo aparecer na tela.
        yield return new WaitUntil(() => sistemaDialogo.caixaDialogo.activeSelf);

        // Depois espera todas as falas terminarem e a caixa ser fechada.
        yield return new WaitUntil(() => !sistemaDialogo.caixaDialogo.activeSelf);

        // Terminou a conversa, então carrega a próxima cena.
        SceneManager.LoadScene(cenaDestino);
    }
}
