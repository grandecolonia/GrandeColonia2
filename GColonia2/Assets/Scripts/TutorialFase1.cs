using UnityEngine;
using System.Collections;

public class TutorialFase1 : MonoBehaviour
{
    public CanvasGroup tutorial;
    public float velocidadefade = 2f;
    private bool desaparecer = false;

    
    void Start()
    {
       tutorial.alpha = 1f;
    }

    void Update()
    {
       if (Input.anyKeyDown && !desaparecer)
       {
          Debug.Log("AAAAAAAAAAAAAAAAAAA");
          StartCoroutine(DesaparecerTutorial());
       }
    }

    IEnumerator DesaparecerTutorial()
    {
        desaparecer = true;

        while (tutorial.alpha > 0)
        {
            tutorial.alpha -= Time.deltaTime * velocidadefade;
            yield return null;
        }

        tutorial.alpha = 0;
    }
}
