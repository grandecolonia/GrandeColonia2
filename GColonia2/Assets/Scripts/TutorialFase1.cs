using UnityEngine;
using System.Collections;

public class TutorialFase1 : MonoBehaviour
{
    // Configura o painel do tutorial e a velocidade do desaparecimento.
    public CanvasGroup tutorial;
    public float velocidadefade = 2f;

    // Impede que a animação seja iniciada mais de uma vez.
    private bool desaparecer = false;

    void Start()
    {
        // O tutorial começa completamente visível.
        tutorial.alpha = 1f;
    }

    void Update()
    {
        // Inicia o desaparecimento quando qualquer tecla for pressionada.
        if (Input.anyKeyDown && !desaparecer)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            StartCoroutine(DesaparecerTutorial());
        }
    }

    IEnumerator DesaparecerTutorial()
    {
        desaparecer = true;

        // Diminui a transparência gradualmente até o tutorial desaparecer.
        while (tutorial.alpha > 0)
        {
            tutorial.alpha -= Time.deltaTime * velocidadefade;
            yield return null;
        }

        tutorial.alpha = 0;
    }
}