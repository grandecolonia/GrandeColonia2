
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class ControleCreditos : MonoBehaviour
{
    [Header("Texto")]

    public TMP_Text textoVoltar;

    [Header("Animação")]

    public float tamanhoMinimo = 0.95f;
    public float tamanhoMaximo = 1.08f;
    public float velocidade = 2f;

    [Header("Transição")]

    public CanvasGroup fade;
    public float duracaoFade = 1f;
    public string cenaMenu = "01_Menu";

    private bool voltando = false;

    void Start()
    {
        // Começa com a tela visível.
        if (fade != null)
        {
            fade.alpha = 0f;
            fade.blocksRaycasts = false;
        }
    }

    void Update()
    {
        // Faz o texto aumentar e diminuir suavemente.
        float escala = Mathf.Lerp(tamanhoMinimo, tamanhoMaximo, (Mathf.Sin(Time.unscaledTime * velocidade) + 1f) / 2f);

        if (textoVoltar != null)
        {
            textoVoltar.transform.localScale = Vector3.one * escala;
        }

        // Volta para o menu quando apertar E.
        if (!voltando && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(VoltarMenu());
        }
    }

    IEnumerator VoltarMenu()
    {
        voltando = true;

        // Escurece a tela antes de mudar de cena.
        if (fade != null)
        {
            fade.blocksRaycasts = true;

            float tempo = 0f;

            while (tempo < duracaoFade)
            {
                tempo += Time.unscaledDeltaTime;
                fade.alpha = Mathf.Clamp01(tempo / duracaoFade);
                yield return null;
            }

            fade.alpha = 1f;
        }

        SceneManager.LoadScene(cenaMenu);
    }
}
