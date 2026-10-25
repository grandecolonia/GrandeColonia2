
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ControleCutscene : MonoBehaviour
{
    [Header("Configurações")]

    // Velocidade com que as letras aparecem na tela.
    public float velocidadeTexto = 0.04f;

    // Tempo que cada imagem fica aparecendo antes de passar para a próxima.
    public float tempoImagem = 3f;

    [Header("Transparência")]

    // Controlam o aparecimento e desaparecimento dos textos e imagens.
    public CanvasGroup texto;
    public CanvasGroup imagem;

    [Header("Elementos da Cutscene")]

    // Texto onde as falas da história vão aparecer.
    public TMP_Text textoInicial;

    // Imagem que será trocada durante a cutscene.
    public Image imagemInicial;

    [Header("Imagens")]

    // Imagens que contam a história na ordem definida abaixo.
    public Sprite imagem1, imagem2, imagem3, imagem4, imagem5, imagem6, imagem7, imagem8, imagem9, imagem10;

    [Header("Próxima cena")]

    // Cena que será carregada quando toda a história terminar.
    public string cenaDestino;

    // Guarda se o jogador clicou para avançar ou completar o texto.
    private bool clicou = false;

    IEnumerator Start()
    {
        // Começa escondendo o texto e a imagem.
        texto.alpha = 0f;
        imagem.alpha = 0f;

        // CUTSCENE 1
        yield return StartCoroutine(MostrarTexto("É de conhecimento geral que as abelhas vivem em uma sociedade altamente organizada."));
        yield return StartCoroutine(MostrarImagem(imagem1));

        // CUTSCENE 2
        yield return StartCoroutine(MostrarTexto("Cada uma cumpre seu papel na colmeia e se orgulha de fazer parte dela."));
        yield return StartCoroutine(MostrarImagem(imagem2));

        // CUTSCENE 3
        yield return StartCoroutine(MostrarTexto("Tudo segue seu ciclo. A cada quatro anos, uma rainha deixa seu posto, dando lugar a uma nova soberana."));
        yield return StartCoroutine(MostrarImagem(imagem3));

        // CUTSCENE 4
        yield return StartCoroutine(MostrarTexto("No entanto, o que aconteceria se esse ciclo fosse interrompido?"));
        yield return StartCoroutine(MostrarImagem(imagem4));

        // CUTSCENE 5
        yield return StartCoroutine(MostrarTexto("A atual rainha, já velha, faleceu antes que uma nova pudesse assumir seu lugar."));
        yield return StartCoroutine(MostrarImagem(imagem5));

        // CUTSCENE 6
        yield return StartCoroutine(MostrarTexto("Sem uma regente, o caos tomou conta da colmeia..."));
        yield return StartCoroutine(MostrarImagem(imagem6));

        // CUTSCENE 7
        yield return StartCoroutine(MostrarTexto("Para apaziguar a situação, os generais da realeza decidiram assumir o controle enquanto a jovem princesa não estivesse pronta para governar."));
        yield return StartCoroutine(MostrarImagem(imagem7));

        // CUTSCENE 8
        yield return StartCoroutine(MostrarTexto("Mas o poder subiu à cabeça. Os generais se tornaram ditadores."));
        yield return StartCoroutine(MostrarImagem(imagem8));

        // CUTSCENE 9
        yield return StartCoroutine(MostrarImagem(imagem9));

        // CUTSCENE 10
        yield return StartCoroutine(MostrarImagem(imagem10));

        // Terminou todas as partes, então carrega a próxima cena.
        SceneManager.LoadScene(cenaDestino);
    }

    void Update()
    {
        // Detecta o clique esquerdo usando o New Input System.
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Avisa às corrotinas que o jogador clicou.
            clicou = true;
        }
    }

    IEnumerator MostrarTexto(string fala)
    {
        // Limpa qualquer clique anterior e apaga o texto antigo.
        clicou = false;
        textoInicial.text = "";

        // Faz a caixa de texto aparecer suavemente.
        yield return StartCoroutine(Fadein(texto));

        // Controla quando todas as páginas dessa fala foram exibidas.
        bool terminou = false;

        while (!terminou)
        {
            // Guarda o tamanho atual da fonte para perceber mudanças nas configurações.
            float tamanhoAtual = textoInicial.fontSize;

            // Divide a fala em páginas que cabem dentro da caixa.
            List<string> paginas = DividirTexto(fala);

            // Marca se o jogador mudou o tamanho da fonte durante a leitura.
            bool mudouTamanho = false;

            foreach (string pagina in paginas)
            {
                // Limpa o clique e coloca a página atual na caixa.
                clicou = false;
                textoInicial.text = pagina;

                // Atualiza o TextMeshPro para contar corretamente as letras.
                textoInicial.ForceMeshUpdate();

                // Descobre quantos caracteres existem na página.
                int totalCaracteres = textoInicial.textInfo.characterCount;

                // Esconde todas as letras antes de começar a digitação.
                textoInicial.maxVisibleCharacters = 0;

                // Vai mostrando uma letra por vez até completar a página.
                for (int i = 1; i <= totalCaracteres; i++)
                {
                    // Se o tamanho da fonte mudou, para para reorganizar as páginas.
                    if (textoInicial.fontSize != tamanhoAtual)
                    {
                        mudouTamanho = true;
                        break;
                    }

                    // Se o jogador clicou, mostra todas as letras de uma vez.
                    if (clicou)
                    {
                        textoInicial.maxVisibleCharacters = totalCaracteres;
                        clicou = false;
                        break;
                    }

                    // Libera mais uma letra e espera um pouquinho.
                    textoInicial.maxVisibleCharacters = i;
                    yield return new WaitForSecondsRealtime(velocidadeTexto);
                }

                // Se a fonte mudou, sai da página para recalcular tudo.
                if (mudouTamanho)
                {
                    break;
                }

                // Espera o jogador clicar para avançar para a próxima página.
                while (!clicou)
                {
                    // Também verifica se a fonte mudou enquanto o jogador lia.
                    if (textoInicial.fontSize != tamanhoAtual)
                    {
                        mudouTamanho = true;
                        break;
                    }

                    yield return null;
                }

                // Se mudou o tamanho, interrompe a sequência de páginas.
                if (mudouTamanho)
                {
                    break;
                }

                // Consome o clique para não avançar duas páginas de uma vez.
                clicou = false;
            }

            // Se a fonte não mudou, terminou todas as páginas dessa fala.
            if (!mudouTamanho)
            {
                terminou = true;
            }
            else
            {
                // Se mudou, limpa o clique e deixa o laço reorganizar a fala.
                clicou = false;
            }
        }

        // Depois de terminar a fala, faz o texto desaparecer.
        yield return StartCoroutine(FadeOut(texto));
    }

    List<string> DividirTexto(string fala)
    {
        // Lista que vai guardar cada pedaço do texto que cabe na tela.
        List<string> paginas = new List<string>();

        // Separa a fala em palavras para montar as páginas sem cortar no meio.
        string[] palavras = fala.Split(new char[] { ' ', '\n', '\r', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Começa com uma página vazia.
        string paginaAtual = "";

        // Pega o espaço disponível na caixa, deixando uma margem de segurança.
        float largura = textoInicial.rectTransform.rect.width - 30f;
        float altura = textoInicial.rectTransform.rect.height - 30f;

        foreach (string palavra in palavras)
        {
            // Testa como ficaria a página se adicionasse mais essa palavra.
            string teste = string.IsNullOrEmpty(paginaAtual) ? palavra : paginaAtual + " " + palavra;

            // Calcula quanto espaço esse texto ocuparia dentro da caixa.
            Vector2 tamanho = textoInicial.GetPreferredValues(teste, largura, Mathf.Infinity);

            // Se passou da altura permitida, guarda a página e começa outra.
            if (tamanho.y > altura && !string.IsNullOrEmpty(paginaAtual))
            {
                paginas.Add(paginaAtual);
                paginaAtual = palavra;
            }
            else
            {
                // Se ainda cabe, continua adicionando palavras na mesma página.
                paginaAtual = teste;
            }
        }

        // Guarda a última página, caso tenha algum texto sobrando.
        if (!string.IsNullOrEmpty(paginaAtual))
        {
            paginas.Add(paginaAtual);
        }

        // Devolve todas as páginas prontas para serem mostradas.
        return paginas;
    }

    IEnumerator EscreverTexto()
    {
        // Atualiza o texto e descobre quantas letras precisam aparecer.
        textoInicial.ForceMeshUpdate();
        int totalCaracteres = textoInicial.textInfo.characterCount;

        // Começa com todas as letras escondidas.
        textoInicial.maxVisibleCharacters = 0;

        // Vai liberando as letras uma por uma.
        for (int i = 1; i <= totalCaracteres; i++)
        {
            // Se o jogador clicar, mostra a página inteira e para de digitar.
            if (clicou)
            {
                textoInicial.maxVisibleCharacters = totalCaracteres;
                clicou = false;
                yield break;
            }

            // Mostra mais uma letra e espera antes de continuar.
            textoInicial.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadeTexto);
        }
    }

    IEnumerator MostrarImagem(Sprite novaImagem)
    {
        // Limpa o clique anterior e coloca a imagem correspondente à história.
        clicou = false;
        imagemInicial.sprite = novaImagem;

        // Faz a imagem aparecer suavemente.
        yield return StartCoroutine(Fadein(imagem));

        // Começa a contar quanto tempo a imagem fica na tela.
        float tempo = 0f;

        // Mantém a imagem até o tempo acabar ou o jogador clicar.
        while (tempo < tempoImagem && !clicou)
        {
            // Conta o tempo mesmo se o jogo estiver pausado.
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // Limpa o clique para não pular a próxima parte sem querer.
        clicou = false;

        // Faz a imagem desaparecer antes de continuar.
        yield return StartCoroutine(FadeOut(imagem));
    }

    IEnumerator EsperarClique()
    {
        // Fica esperando até o jogador clicar.
        while (!clicou)
        {
            yield return null;
        }

        // Consome o clique para não avançar duas vezes.
        clicou = false;
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
        // Começa com o elemento invisível.
        float tempo = 0f;

        // Aumenta a opacidade aos poucos até aparecer completamente.
        while (tempo < 1f)
        {
            tempo += Time.deltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        // Garante que termine totalmente visível.
        objeto.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup objeto)
    {
        // Começa com o elemento totalmente visível.
        float tempo = 1f;

        // Vai diminuindo a opacidade até desaparecer.
        while (tempo > 0f)
        {
            tempo -= Time.deltaTime;
            objeto.alpha = tempo;
            yield return null;
        }

        // Garante que termine completamente invisível.
        objeto.alpha = 0f;
    }
}
