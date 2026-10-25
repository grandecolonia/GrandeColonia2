
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class ControleCreditos : MonoBehaviour
{
    [Header("Texto")]

    // Texto que avisa ao jogador para apertar E e voltar ao menu.
    public TMP_Text textoVoltar;

    [Header("Animação")]

    // Define o menor tamanho que o texto pode ficar.
    public float tamanhoMinimo = 0.95f;

    // Define o maior tamanho que o texto pode alcançar.
    public float tamanhoMaximo = 1.08f;

    // Controla a velocidade com que o texto aumenta e diminui.
    public float velocidade = 2f;

    [Header("Transição")]

    // Controla a tela preta que aparece antes de voltar ao menu.
    public CanvasGroup fade;

    // Tempo que a tela vai levar para escurecer completamente.
    public float duracaoFade = 1f;

    // Cena que será carregada quando o jogador sair dos créditos.
    public string cenaMenu = "01_Menu";

    // Impede que o jogador inicie a transição várias vezes.
    private bool voltando = false;

    void Start()
    {
        // Começa com a tela totalmente visível, sem o fundo preto atrapalhando.
        if (fade != null)
        {
            fade.alpha = 0f;
            fade.blocksRaycasts = false;
        }
    }

    void Update()
    {
        // Usa o seno para fazer o texto aumentar e diminuir suavemente.
        float escala = Mathf.Lerp(tamanhoMinimo, tamanhoMaximo, (Mathf.Sin(Time.unscaledTime * velocidade) + 1f) / 2f);

        // Aplica a animação de tamanho no texto, caso ele exista.
        if (textoVoltar != null)
        {
            textoVoltar.transform.localScale = Vector3.one * escala;
        }

        // Se apertou E e ainda não está voltando, começa a transição para o menu.
        if (!voltando && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(VoltarMenu());
        }
    }

    IEnumerator VoltarMenu()
    {
        // Marca que a transição começou para impedir outros comandos de voltar.
        voltando = true;

        // Se existe uma tela de fade, começa a escurecer tudo.
        if (fade != null)
        {
            // Bloqueia os cliques enquanto a tela está escurecendo.
            fade.blocksRaycasts = true;

            // Começa a contar o tempo da transição.
            float tempo = 0f;

            // Vai aumentando a escuridão até completar o tempo definido.
            while (tempo < duracaoFade)
            {
                // Conta o tempo mesmo se o jogo estiver pausado.
                tempo += Time.unscaledDeltaTime;

                // Aumenta a opacidade do fundo preto aos poucos.
                fade.alpha = Mathf.Clamp01(tempo / duracaoFade);

                // Espera o próximo frame para continuar a animação.
                yield return null;
            }

            // Garante que a tela termine completamente preta.
            fade.alpha = 1f;
        }

        // Depois que a tela escurece, carrega o menu principal.
        SceneManager.LoadScene(cenaMenu);
    }
}
