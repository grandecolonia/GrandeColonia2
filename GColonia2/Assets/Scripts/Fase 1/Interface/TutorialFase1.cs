
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TutorialFase1 : MonoBehaviour
{
    [Header("Tutorial")]

    // Painel do tutorial que vai desaparecer quando o jogador apertar alguma tecla.
    public CanvasGroup tutorial;

    // Controla a velocidade com que o tutorial vai ficando invisível.
    public float velocidadefade = 2f;

    // Impede que a animação seja iniciada várias vezes ao mesmo tempo.
    private bool desaparecer = false;

    void Start()
    {
        // Começa com o tutorial totalmente visível.
        tutorial.alpha = 1f;
    }

    void Update()
    {
        // Verifica se alguma tecla foi apertada usando o New Input System.
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame && !desaparecer)
        {
            // Começa a esconder o tutorial aos poucos.
            StartCoroutine(DesaparecerTutorial());
        }
    }

    IEnumerator DesaparecerTutorial()
    {
        // Marca que o tutorial já começou a desaparecer para não repetir a animação.
        desaparecer = true;

        // Vai diminuindo a transparência até o tutorial sumir completamente.
        while (tutorial.alpha > 0f)
        {
            // Diminui a opacidade conforme a velocidade configurada.
            tutorial.alpha -= Time.deltaTime * velocidadefade;

            // Espera o próximo frame para continuar o desaparecimento.
            yield return null;
        }

        // Garante que o tutorial termine completamente invisível.
        tutorial.alpha = 0f;
    }
}
