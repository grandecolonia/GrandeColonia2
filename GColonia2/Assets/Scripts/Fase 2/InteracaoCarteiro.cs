using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteracaoCarteiro : MonoBehaviour
{
    [Header("Diálogo")]

    // Sistema usado para mostrar as falas do carteiro.
    public SistemaDialogo sistemaDialogo;

    // Diálogo usado pelo carteiro nesta cena.
    public DialogoData dialogo;

    [Header("Personagem")]

    // Movimento do personagem, usado para travar ele durante a conversa.
    public MovimentacaoPersonagem personagem;

    [Header("Próxima cena")]

    // Cena carregada depois que o diálogo terminar.
    public string cenaDestino;

    private bool dialogoIniciado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Inicia o diálogo quando Maxwell entra na área pela primeira vez.
        if (collision.CompareTag("Player") && !dialogoIniciado)
        {
            dialogoIniciado = true;

            personagem.podeMover = false;
            sistemaDialogo.IniciarDialogo(dialogo);

            StartCoroutine(EsperarFimDialogo());
        }
    }

    IEnumerator EsperarFimDialogo()
    {
        // Espera a caixa de diálogo aparecer.
        yield return new WaitUntil(() => sistemaDialogo.caixaDialogo.activeSelf);

        // Espera a última fala terminar e a caixa fechar.
        yield return new WaitUntil(() => !sistemaDialogo.caixaDialogo.activeSelf);

        // Vai para a próxima cena.
        SceneManager.LoadScene(cenaDestino);
    }
}