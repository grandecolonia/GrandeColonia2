using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ControleCutscene : MonoBehaviour
{
    public float velocidadeTexto = 0.04f;
    public CanvasGroup texto;
    public CanvasGroup imagem;
    public TMP_Text textoInicial;
    public Image imagemInicial;
    public Sprite imagem1;
    public Sprite imagem2;
    public Sprite imagem3;
    public Sprite imagem4;
    public Sprite imagem5;
    public Sprite imagem6;
    public Sprite imagem7;
    public Sprite imagem8;
    public Sprite imagem9;
    public Sprite imagem10;

    IEnumerator Start()
    {
       texto.alpha = 0;
       imagem.alpha = 0;
       
       // CUTSCENE 1
       textoInicial.text = "É de conhecimento geral que as abelhas vivem em uma sociedade altamente organizada.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem1;
       yield return StartCoroutine(Fadein(imagem));
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem));
       
       // CUTSCENE 2
       textoInicial.text = "Cada uma cumpre seu papel na colmeia e se orgulha de fazer parte dela.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem2;
       yield return StartCoroutine(Fadein(imagem));
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem));
       
       // CUTSCENE 3
       textoInicial.text = "Tudo segue seu ciclo. A cada quatro anos, uma rainha deixa seu posto, dando lugar a uma nova soberana.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));
    
       imagemInicial.sprite = imagem3;
       yield return StartCoroutine(Fadein(imagem));
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem));

       // CUTSCENE 4
       textoInicial.text = "No entanto, o que aconteceria se esse ciclo fosse interrompido?";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem4;
       yield return StartCoroutine(Fadein(imagem));
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem));

       // CUTSCENE 5
       textoInicial.text = "A atual rainha, já velha, faleceu antes que uma nova pudesse assumir seu lugar.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem5;
       yield return StartCoroutine(Fadein(imagem));
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 

       // CUTSCENE 6
       textoInicial.text = "Sem uma regente, o caos tomou conta da colmeia...";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem6;
       yield return StartCoroutine(Fadein(imagem)); 
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 

       // CUTSCENE 7
       textoInicial.text = "Para apaziguar a situação, os generais da realeza decidiram assumir o controle enquanto a jovem princesa não estivesse pronta para governar.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem7;
       yield return StartCoroutine(Fadein(imagem)); 
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 

       // CUTSCENE 8
       textoInicial.text = "Mas o poder subiu à cabeça. Os generais se tornaram ditadores.";
       textoInicial.maxVisibleCharacters = 0;
       yield return StartCoroutine(Fadein(texto));
       yield return StartCoroutine(EscreverTexto());
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(texto));

       imagemInicial.sprite = imagem8;
       yield return StartCoroutine(Fadein(imagem)); 
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 

       // CUTSCENE 9
       imagemInicial.sprite = imagem9;
       yield return StartCoroutine(Fadein(imagem)); 
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 
       
       // CUTSCENE 10
       imagemInicial.sprite = imagem10;
       yield return StartCoroutine(Fadein(imagem)); 
       yield return new WaitForSeconds(3);
       yield return StartCoroutine(FadeOut(imagem)); 
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
       float tempo = 0;

       while (tempo < 1)
       {
          tempo += Time.deltaTime;
          objeto.alpha = tempo;
          yield return null;
       }

       objeto.alpha = 1;
    }

    IEnumerator EscreverTexto()
    {
       textoInicial.maxVisibleCharacters = 0;
       for (int i = 0; i <= textoInicial.text.Length; i++)
        {
            textoInicial.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadeTexto);
        }
    }

    IEnumerator FadeOut(CanvasGroup objeto)
    {
      float tempo = 1;

      while (tempo > 0)
      {
        tempo -= Time.deltaTime;
        objeto.alpha = tempo;
        yield return null;
      }

      objeto.alpha = 0;
    }


}
