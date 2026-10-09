using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GerenciadorCaçaPalavras : MonoBehaviour
{
    [Header("Grade")]

    // Objeto onde todas as letras serão criadas.
    public Transform gradeCacaPalavras;

    // Prefab usado para criar cada letra.
    public GameObject prefabLetra;


    [Header("Caça-palavras")]

    // Cada string representa uma linha da grade.
    public string[] linhas =
    {
        "QWERTYUIOP",
        "MARIGHELLA",
        "LAMARCAQWE",
        "HERZOGTYUI",
        "BUARQUEFGH",
        "VANDREJKLP",
        "ROUSSEFFQW",
        "GUIMARAESX",
        "ZXCVBNMASD",
        "POIUYTREWA"
    };

    // Palavras que o jogador precisa encontrar.
    public string[] palavras =
    {
        "MARIGHELLA",
        "LAMARCA",
        "HERZOG",
        "BUARQUE",
        "VANDRE",
        "ROUSSEFF",
        "GUIMARAES"
    };


    [Header("Nomes na lateral")]

    public TMP_Text textoMarighella;
    public TMP_Text textoLamarca;
    public TMP_Text textoHerzog;
    public TMP_Text textoBuarque;
    public TMP_Text textoVandre;
    public TMP_Text textoGuimaraes;
    public TMP_Text textoRousseff;


    [Header("Cores")]

    // Transparente quando não está selecionada.
    public Color corNormal = new Color(1f, 1f, 1f, 0f);

    // Vermelho enquanto a seleção ainda não formar uma palavra.
    public Color corErrada = new Color(1f, 0f, 0f, 0.45f);

    // Verde quando a palavra estiver correta.
    public Color corCorreta = new Color(0f, 1f, 0f, 0.45f);


    // Guarda as letras na ordem do arraste.
    private Stack<LetraCaçaPalavras> letrasSelecionadas =
        new Stack<LetraCaçaPalavras>();


    // Lista de palavras que já foram encontradas.
    private List<string> palavrasEncontradas =
        new List<string>();


    private bool selecionando = false;

    private Vector2Int direcao;

    private bool direcaoDefinida = false;


    void Start()
    {
        CriarGrade();
    }


    void CriarGrade()
    {
        for (int linha = 0; linha < linhas.Length; linha++)
        {
            for (int coluna = 0; coluna < linhas[linha].Length; coluna++)
            {
                GameObject novaLetra =
                    Instantiate(prefabLetra, gradeCacaPalavras);

                LetraCaçaPalavras letra =
                    novaLetra.GetComponent<LetraCaçaPalavras>();

                if (letra != null)
                {
                    letra.Configurar(
                        linhas[linha][coluna],
                        linha,
                        coluna,
                        this
                    );
                }
            }
        }
    }


    public void ComecarSelecao(LetraCaçaPalavras letra)
    {
        // Limpa qualquer seleção antiga.
        LimparSelecao();

        selecionando = true;

        direcaoDefinida = false;

        letrasSelecionadas.Push(letra);

        AtualizarCores();
    }


    public void PassarPorLetra(LetraCaçaPalavras letra)
    {
        if (!selecionando || letra.bloqueada)
        {
            return;
        }


        // Impede colocar novamente a letra que já está no topo.
        if (letrasSelecionadas.Count > 0 &&
            letrasSelecionadas.Peek() == letra)
        {
            return;
        }


        // Permite voltar uma letra.
        if (EhLetraAnterior(letra))
        {
            LetraCaçaPalavras removida =
                letrasSelecionadas.Pop();

            removida.MudarCor(corNormal);


            if (letrasSelecionadas.Count < 2)
            {
                direcaoDefinida = false;
            }

            AtualizarCores();

            return;
        }


        LetraCaçaPalavras ultimaLetra =
            letrasSelecionadas.Peek();


        int diferencaLinha =
            letra.linha - ultimaLetra.linha;

        int diferencaColuna =
            letra.coluna - ultimaLetra.coluna;


        // A letra precisa estar encostada na anterior.
        if (Mathf.Abs(diferencaLinha) > 1 ||
            Mathf.Abs(diferencaColuna) > 1)
        {
            return;
        }


        // Não pode ser a mesma posição.
        if (diferencaLinha == 0 &&
            diferencaColuna == 0)
        {
            return;
        }


        Vector2Int novaDirecao =
            new Vector2Int(
                diferencaColuna == 0
                    ? 0
                    : (int)Mathf.Sign(diferencaColuna),

                diferencaLinha == 0
                    ? 0
                    : (int)Mathf.Sign(diferencaLinha)
            );


        // A segunda letra define a direção.
        if (!direcaoDefinida)
        {
            direcao = novaDirecao;

            direcaoDefinida = true;
        }
        else if (novaDirecao != direcao)
        {
            // Impede selecionar em formato de cobra.
            return;
        }


        letrasSelecionadas.Push(letra);

        AtualizarCores();
    }


    public void TerminarSelecao()
    {
        if (!selecionando)
        {
            return;
        }


        selecionando = false;


        string palavraSelecionada =
            MontarPalavra();


        if (PalavraValida(palavraSelecionada))
        {
            ConfirmarPalavra(palavraSelecionada);
        }
        else
        {
            LimparSelecao();
        }


        direcaoDefinida = false;
    }


    bool EhLetraAnterior(LetraCaçaPalavras letra)
    {
        if (letrasSelecionadas.Count < 2)
        {
            return false;
        }


        LetraCaçaPalavras[] letras =
            letrasSelecionadas.ToArray();


        // Índice 0 = topo da pilha.
        // Índice 1 = letra anterior.
        return letras[1] == letra;
    }


    void AtualizarCores()
    {
        string palavraAtual =
            MontarPalavra();


        Color cor =
            PalavraValida(palavraAtual)
                ? corCorreta
                : corErrada;


        foreach (LetraCaçaPalavras letra
                 in letrasSelecionadas)
        {
            letra.MudarCor(cor);
        }
    }


    string MontarPalavra()
    {
        // A Stack entrega do último para o primeiro.
        // Reverse coloca novamente na ordem do arraste.

        return string.Concat(
            letrasSelecionadas
            .Reverse()
            .Select(letra => letra.textoLetra.text)
        );
    }


    bool PalavraValida(string palavra)
    {
        foreach (string palavraCorreta in palavras)
        {
            string palavraInvertida =
                new string(
                    palavraCorreta
                    .Reverse()
                    .ToArray()
                );


            if (palavra == palavraCorreta ||
                palavra == palavraInvertida)
            {
                // Não permite encontrar a mesma palavra duas vezes.
                return !palavrasEncontradas.Contains(
                    palavraCorreta
                );
            }
        }


        return false;
    }


    void ConfirmarPalavra(string palavraSelecionada)
    {
        string palavraEncontrada = "";


        foreach (string palavraCorreta in palavras)
        {
            string palavraInvertida =
                new string(
                    palavraCorreta
                    .Reverse()
                    .ToArray()
                );


            if (palavraSelecionada == palavraCorreta ||
                palavraSelecionada == palavraInvertida)
            {
                palavraEncontrada = palavraCorreta;

                break;
            }
        }


        if (string.IsNullOrEmpty(palavraEncontrada))
        {
            return;
        }


        // Adiciona na List.
        palavrasEncontradas.Add(
            palavraEncontrada
        );


        AtualizarNome(
            palavraEncontrada
        );


        // Bloqueia as letras da palavra encontrada.
        foreach (LetraCaçaPalavras letra
                 in letrasSelecionadas)
        {
            letra.Bloquear();
        }


        Debug.Log(
            "PALAVRA ENCONTRADA: "
            + palavraEncontrada
        );


        letrasSelecionadas.Clear();


        VerificarFim();
    }


    void AtualizarNome(string palavra)
    {
        if (palavra == "MARIGHELLA")
        {
            textoMarighella.text =
                "Carlos Marighella";
        }

        else if (palavra == "LAMARCA")
        {
            textoLamarca.text =
                "Carlos Lamarca";
        }

        else if (palavra == "HERZOG")
        {
            textoHerzog.text =
                "Vladimir Herzog";
        }

        else if (palavra == "BUARQUE")
        {
            textoBuarque.text =
                "Chico Buarque";
        }

        else if (palavra == "VANDRE")
        {
            textoVandre.text =
                "Geraldo Vandré";
        }

        else if (palavra == "GUIMARAES")
        {
            textoGuimaraes.text =
                "Ulysses Guimarães";
        }

        else if (palavra == "ROUSSEFF")
        {
            textoRousseff.text =
                "Dilma Rousseff";
        }
    }


    void LimparSelecao()
    {
        while (letrasSelecionadas.Count > 0)
        {
            LetraCaçaPalavras letra =
                letrasSelecionadas.Pop();


            if (!letra.bloqueada)
            {
                letra.MudarCor(
                    corNormal
                );
            }
        }
    }


    void VerificarFim()
    {
        if (palavrasEncontradas.Count ==
            palavras.Length)
        {
            Debug.Log(
                "TODAS AS PALAVRAS FORAM ENCONTRADAS!"
            );
        }
    }
}