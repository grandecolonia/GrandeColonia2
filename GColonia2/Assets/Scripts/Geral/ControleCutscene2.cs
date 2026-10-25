
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EtapaCutscene
{
    // Texto que será mostrado nesta parte da cutscene.
    [TextArea(2, 5)]
    public string texto;

    // Imagem e som que podem acompanhar essa etapa.
    public Sprite imagem;
    public AudioClip som;

    // Tempo extra de espera depois que o som começar a tocar.
    public float esperaDepoisSom = 0f;
}

public class ControleCutscene2 : MonoBehaviour
{
    [Header("Áudio")]

    // Fonte responsável por reproduzir os sons da cutscene.
    public AudioSource audioSource;

    [Header("Configuração")]

    // Velocidade com que as letras aparecem na tela.
    public float velocidadeTexto = 0.04f;

    // Tempo que cada página de texto fica aparecendo depois da digitação.
    public float tempoTexto = 3f;

    // Tempo que cada imagem permanece na tela.
    public float tempoImagem = 3f;

    [Header("Elementos da cutscene")]

    // Controlam o aparecimento e desaparecimento dos textos e imagens.
    public CanvasGroup texto;
    public CanvasGroup imagem;

    // Elementos onde as falas e imagens serão mostradas.
    public TMP_Text textoInicial;
    public Image imagemInicial;

    [Header("Etapas")]

    // Guarda todas as partes da cutscene na ordem em que vão acontecer.
    public EtapaCutscene[] etapas;

    [Header("Input")]

    // Ação que identifica quando o jogador quer avançar a cutscene.
    public InputActionReference avancarCutscene;

    [Header("Próxima cena")]

    // Cena que será carregada quando a cutscene terminar.
    public string cenaDestino;

    // Guarda se o jogador apertou o comando para avançar.
    private bool clicou = false;

    void OnEnable()
    {
        // Se a ação foi configurada, começa a escutar o comando de avançar.
        if (avancarCutscene != null)
        {
            // Liga o método Avancar ao evento da ação.
            avancarCutscene.action.performed += Avancar;

            // Habilita a ação para receber os comandos do jogador.
            avancarCutscene.action.Enable();
        }
    }

    void OnDisable()
    {
        // Se a ação existe, para de escutar quando este objeto for desativado.
        if (avancarCutscene != null)
        {
            // Remove a ligação com o método para não continuar recebendo comandos.
            avancarCutscene.action.performed -= Avancar;

            // Desabilita a ação enquanto esta cutscene não estiver ativa.
            avancarCutscene.action.Disable();
        }
    }

    IEnumerator Start()
    {
        // Começa com o texto e a imagem completamente escondidos.
        texto.alpha = 0f;
        imagem.alpha = 0f;

        // Passa por todas as etapas cadastradas no Inspector.
        foreach (EtapaCutscene etapa in etapas)
        {
            // Se esta etapa possui um som, começa a reproduzi-lo.
            if (etapa.som != null)
            {
                audioSource.PlayOneShot(etapa.som);

                // Se existe uma pausa configurada, espera antes de continuar.
                if (etapa.esperaDepoisSom > 0f)
                {
                    yield return new WaitForSecondsRealtime(etapa.esperaDepoisSom);
                }
            }

            // Se a etapa possui alguma fala, mostra o texto na tela.
            if (!string.IsNullOrWhiteSpace(etapa.texto))
            {
                yield return StartCoroutine(MostrarTexto(etapa.texto));
            }

            // Se a etapa possui uma imagem, mostra ela depois do texto.
            if (etapa.imagem != null)
            {
                yield return StartCoroutine(MostrarImagem(etapa.imagem));
            }
        }

        // Terminou todas as etapas, então devolve o tempo ao normal.
        Time.timeScale = 1f;

        // Carrega a cena configurada para depois da cutscene.
        SceneManager.LoadScene(cenaDestino);
    }

    void Avancar(InputAction.CallbackContext context)
    {
        // Registra que o jogador apertou o comando para avançar.
        clicou = true;
    }

    IEnumerator MostrarTexto(string fala)
    {
        // Limpa qualquer comando anterior e apaga o texto antigo.
        clicou = false;
        textoInicial.text = "";

        // Faz a caixa de texto aparecer suavemente.
        yield return StartCoroutine(Fadein(texto));

        // Controla quando todas as páginas dessa fala foram mostradas.
        bool terminou = false;

        while (!terminou)
        {
            // Guarda o tamanho atual da fonte para perceber mudanças nas configurações.
            float tamanhoAtual = textoInicial.fontSize;

            // Divide a fala em páginas que cabem dentro da caixa de texto.
            List<string> paginas = DividirTexto(fala);

            // Marca se o jogador mudou o tamanho da fonte durante a leitura.
            bool mudouTamanho = false;

            // Passa por todas as páginas criadas para essa fala.
            foreach (string pagina in paginas)
            {
                // Limpa o clique anterior e coloca a página atual no texto.
                clicou = false;
                textoInicial.text = pagina;

                // Atualiza o TextMeshPro para contar corretamente os caracteres.
                textoInicial.ForceMeshUpdate();

                // Descobre quantos caracteres existem nesta página.
                int totalCaracteres = textoInicial.textInfo.characterCount;

                // Esconde todas as letras antes de começar a digitação.
                textoInicial.maxVisibleCharacters = 0;

                // Vai mostrando uma letra por vez até completar a página.
                for (int i = 1; i <= totalCaracteres; i++)
                {
                    // Se o tamanho da fonte mudou, interrompe para reorganizar as páginas.
                    if (textoInicial.fontSize != tamanhoAtual)
                    {
                        mudouTamanho = true;
                        break;
                    }

                    // Se o jogador pediu para avançar, mostra todas as letras de uma vez.
                    if (clicou)
                    {
                        textoInicial.maxVisibleCharacters = totalCaracteres;
                        clicou = false;
                        break;
                    }

                    // Libera mais uma letra e espera antes de continuar.
                    textoInicial.maxVisibleCharacters = i;
                    yield return new WaitForSecondsRealtime(velocidadeTexto);
                }

                // Se mudou o tamanho da fonte, precisa recalcular o texto.
                if (mudouTamanho)
                {
                    break;
                }

                // Começa a contar quanto tempo a página vai permanecer na tela.
                float tempo = 0f;
                clicou = false;

                // Espera o tempo acabar ou o jogador avançar manualmente.
                while (tempo < tempoTexto && !clicou)
                {
                    // Se a fonte mudou durante a leitura, interrompe a espera.
                    if (textoInicial.fontSize != tamanhoAtual)
                    {
                        mudouTamanho = true;
                        break;
                    }

                    // Conta o tempo mesmo se o jogo estiver pausado.
                    tempo += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Se a fonte mudou, sai para reorganizar as páginas.
                if (mudouTamanho)
                {
                    break;
                }

                // Limpa o comando para não pular a próxima página sem querer.
                clicou = false;
            }

            // Se a fonte não mudou, significa que terminou todas as páginas.
            if (!mudouTamanho)
            {
                terminou = true;
            }
            else
            {
                // Se mudou, limpa o comando e deixa o texto ser dividido novamente.
                clicou = false;
            }
        }

        // Terminou a fala, então esconde a caixa de texto suavemente.
        yield return StartCoroutine(FadeOut(texto));
    }

    List<string> DividirTexto(string fala)
    {
        // Lista que vai guardar todas as páginas da fala.
        List<string> paginas = new List<string>();

        // Separa a fala em palavras para não cortar o texto no meio delas.
        string[] palavras = fala.Split(new char[] { ' ', '\n', '\r', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Começa montando uma página vazia.
        string paginaAtual = "";

        // Calcula o espaço disponível dentro da caixa, deixando uma pequena margem.
        float largura = textoInicial.rectTransform.rect.width - 30f;
        float altura = textoInicial.rectTransform.rect.height - 30f;

        // Testa cada palavra para descobrir se ainda cabe na página.
        foreach (string palavra in palavras)
        {
            // Junta a nova palavra com o texto que já está na página.
            string teste = string.IsNullOrEmpty(paginaAtual) ? palavra : paginaAtual + " " + palavra;

            // Calcula quanto espaço o texto ocuparia dentro da caixa.
            Vector2 tamanho = textoInicial.GetPreferredValues(teste, largura, Mathf.Infinity);

            // Se passou da altura disponível, guarda a página e começa outra.
            if (tamanho.y > altura && !string.IsNullOrEmpty(paginaAtual))
            {
                paginas.Add(paginaAtual);
                paginaAtual = palavra;
            }
            else
            {
                // Se ainda cabe, continua colocando palavras na mesma página.
                paginaAtual = teste;
            }
        }

        // Guarda a última página se ainda existe algum texto sobrando.
        if (!string.IsNullOrEmpty(paginaAtual))
        {
            paginas.Add(paginaAtual);
        }

        // Devolve todas as páginas prontas para aparecerem na tela.
        return paginas;
    }

    IEnumerator MostrarImagem(Sprite novaImagem)
    {
        // Limpa o comando anterior e troca a imagem pela desta etapa.
        clicou = false;
        imagemInicial.sprite = novaImagem;

        // Faz a imagem aparecer suavemente.
        yield return StartCoroutine(Fadein(imagem));

        // Mantém a imagem na tela até o tempo acabar ou o jogador avançar.
        yield return StartCoroutine(EsperarAvancoOuTempo(tempoImagem));

        // Depois esconde a imagem antes de continuar.
        yield return StartCoroutine(FadeOut(imagem));
    }

    IEnumerator EsperarAvancoOuTempo(float tempoMaximo)
    {
        // Começa a contar o tempo que o elemento vai ficar aparecendo.
        float tempo = 0f;

        // Continua esperando enquanto houver tempo e o jogador não avançar.
        while (tempo < tempoMaximo && !clicou)
        {
            // Conta o tempo mesmo se o jogo estiver pausado.
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        // Limpa o comando para não pular duas etapas de uma vez.
        clicou = false;
    }

    IEnumerator Fadein(CanvasGroup objeto)
    {
        // Começa com o elemento completamente invisível.
        float tempo = 0f;

        // Vai aumentando a opacidade até mostrar o elemento inteiro.
        while (tempo < 1f)
        {
            // Conta o tempo independentemente da pausa do jogo.
            tempo += Time.unscaledDeltaTime;

            // Aplica a transparência conforme a animação avança.
            objeto.alpha = tempo;

            // Espera o próximo frame para continuar o efeito.
            yield return null;
        }

        // Garante que o elemento termine totalmente visível.
        objeto.alpha = 1f;
    }

    IEnumerator EscreverTexto()
    {
        // Atualiza o texto e conta quantos caracteres precisam aparecer.
        textoInicial.ForceMeshUpdate();
        int totalCaracteres = textoInicial.textInfo.characterCount;

        // Começa com todas as letras escondidas.
        textoInicial.maxVisibleCharacters = 0;

        // Vai mostrando uma letra por vez até completar o texto.
        for (int i = 1; i <= totalCaracteres; i++)
        {
            // Se o jogador avançou, mostra tudo de uma vez e encerra a digitação.
            if (clicou)
            {
                textoInicial.maxVisibleCharacters = totalCaracteres;
                clicou = false;
                yield break;
            }

            // Mostra mais uma letra e espera um pouquinho.
            textoInicial.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(velocidadeTexto);
        }
    }

    IEnumerator FadeOut(CanvasGroup objeto)
    {
        // Começa com o elemento totalmente visível.
        float tempo = 1f;

        // Vai diminuindo a opacidade até o elemento desaparecer.
        while (tempo > 0f)
        {
            // Conta o tempo mesmo quando o jogo está pausado.
            tempo -= Time.unscaledDeltaTime;

            // Aplica a transparência conforme a animação termina.
            objeto.alpha = tempo;

            // Espera o próximo frame para continuar o efeito.
            yield return null;
        }

        // Garante que o elemento termine completamente invisível.
        objeto.alpha = 0f;
    }
}
