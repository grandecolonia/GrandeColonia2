using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteracaoCarteiro : MonoBehaviour
{
    [Header("Diálogo")]

    // Sistema responsável por mostrar as falas.
    public SistemaDialogo sistemaDialogo;

    // Diálogo do carteiro usado nesta cena.
    public DialogoData dialogo;

    [Header("Personagem")]

    // Movimento do personagem para bloquear durante a conversa.
    public MovimentacaoPersonagem personagem;

    [Header("Próxima cena")]

    // Cena carregada quando o diálogo terminar.
    public string cenaDestino;

    private bool dialogoIniciado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

        SceneManager.LoadScene(cenaDestino);
    }
}