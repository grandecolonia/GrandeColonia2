using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorCaçaPalavras : MonoBehaviour
{
    [Header("Grade")]
    public Transform gradeCacaPalavras;
    public GameObject prefabLetra;

    [Header("Caça-palavras")]

    // Linhas que vão formar a grade.
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

    // Palavras que precisam ser achadas.
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

    // Cor normal das letras.
    public Color corNormal = new Color(1f, 1f, 1f, 0f);

    // Cor enquanto a palavra estiver errada.
    public Color corErrada = new Color(1f, 0f, 0f, 0.45f);

    // Cor quando a palavra estiver certa.
    public Color corCorreta = new Color(0f, 1f, 0f, 0.45f);

    // Guarda as letras que estão sendo selecionadas.
    private Stack<LetraCaçaPalavras> letrasSelecionadas = new Stack<LetraCaçaPalavras>();

    // Guarda as palavras que já foram achadas.
    private List<string> palavrasEncontradas = new List<string>();

    // Variaveis para controlar a seleção.
    private bool selecionando = false;
    private Vector2 direcao;
    private bool direcaoDefinida = false;

    void Start()
    {
        CriarGrade();
    }

    // Cria as letras usando o prefab.
    void CriarGrade()
    {
        for (int linha = 0; linha < linhas.Length; linha++)
        {
            for (int coluna = 0; coluna < linhas[linha].Length; coluna++)
            {
                GameObject novaLetra = Instantiate(prefabLetra, gradeCacaPalavras);
                LetraCaçaPalavras letra = novaLetra.GetComponent<LetraCaçaPalavras>();

                if (letra != null)
                {
                    letra.Configurar(linhas[linha][coluna], linha, coluna, this);
                }
            }
        }
    }

    // Começa a selecionar quando clicar numa letra.
    public void ComecarSelecao(LetraCaçaPalavras letra)
    {
        LimparSelecao();

        selecionando = true;
        direcaoDefinida = false;

        letrasSelecionadas.Push(letra);
        AtualizarCores();
    }

    // Vai pegando as letras enquanto arrasta.
    public void PassarPorLetra(LetraCaçaPalavras letra)
    {
        // Se não estiver selecionando ou a letra estiver bloqueada, para.
        if (!selecionando || letra.bloqueada)
        {
            return;
        }

        // Não deixa pegar a mesma letra de novo.
        if (letrasSelecionadas.Count > 0 && letrasSelecionadas.Peek() == letra)
        {
            return;
        }

        // Se voltar pra letra anterior, tira a ultima.
        if (EhLetraAnterior(letra))
        {
            LetraCaçaPalavras removida = letrasSelecionadas.Pop();
            removida.MudarCor(corNormal);

            if (letrasSelecionadas.Count < 2)
            {
                direcaoDefinida = false;
            }

            AtualizarCores();
            return;
        }

        // Pega a ultima letra selecionada.
        LetraCaçaPalavras ultimaLetra = letrasSelecionadas.Peek();

        // Calcula a diferença entre as posições.
        int diferencaLinha = letra.linha - ultimaLetra.linha;
        int diferencaColuna = letra.coluna - ultimaLetra.coluna;

        // Se a letra estiver longe, não seleciona.
        if (Mathf.Abs(diferencaLinha) > 1 || Mathf.Abs(diferencaColuna) > 1)
        {
            return;
        }

        // Não deixa selecionar a mesma posição.
        if (diferencaLinha == 0 && diferencaColuna == 0)
        {
            return;
        }

        // Descobre pra qual direção o jogador está indo.
        Vector2 novaDirecao = new Vector2(diferencaColuna == 0 ? 0 : Mathf.Sign(diferencaColuna), diferencaLinha == 0 ? 0 : Mathf.Sign(diferencaLinha));

        // Define a direção usando a segunda letra.
        if (!direcaoDefinida)
        {
            direcao = novaDirecao;
            direcaoDefinida = true;
        }
        else if (novaDirecao != direcao)
        {
            // Se mudar a direção, não deixa continuar.
            return;
        }

        letrasSelecionadas.Push(letra);
        AtualizarCores();
    }

    // Quando soltar o mouse, verifica a palavra.
    public void TerminarSelecao()
    {
        if (!selecionando)
        {
            return;
        }

        selecionando = false;

        string palavraSelecionada = MontarPalavra();

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

    // Vê se voltou pra letra anterior.
    bool EhLetraAnterior(LetraCaçaPalavras letra)
    {
        if (letrasSelecionadas.Count < 2)
        {
            return false;
        }

        // A posição 0 é a ultima letra e a 1 é a anterior.
        LetraCaçaPalavras[] letras = letrasSelecionadas.ToArray();

        return letras[1] == letra;
    }

    // Muda a cor das letras selecionadas.
    void AtualizarCores()
    {
        string palavraAtual = MontarPalavra();
        Color cor = PalavraValida(palavraAtual) ? corCorreta : corErrada;

        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.MudarCor(cor);
        }
    }

    // Junta as letras pra formar a palavra.
    string MontarPalavra()
    {
        string palavra = "";

        // Reverse coloca as letras na ordem certa.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas.Reverse())
        {
            palavra += letra.textoLetra.text;
        }

        return palavra;
    }

    // Vê se a palavra está certa e se já foi achada.
    bool PalavraValida(string palavra)
    {
        foreach (string palavraCorreta in palavras)
        {
            // Inverte a palavra pra aceitar de trás pra frente.
            string palavraInvertida = new string(palavraCorreta.Reverse().ToArray());

            if (palavra == palavraCorreta || palavra == palavraInvertida)
            {
                return !palavrasEncontradas.Contains(palavraCorreta);
            }
        }

        return false;
    }

    // Confirma quando achar uma palavra.
    void ConfirmarPalavra(string palavraSelecionada)
    {
        string palavraEncontrada = "";

        foreach (string palavraCorreta in palavras)
        {
            string palavraInvertida = new string(palavraCorreta.Reverse().ToArray());

            if (palavraSelecionada == palavraCorreta || palavraSelecionada == palavraInvertida)
            {
                palavraEncontrada = palavraCorreta;
                break;
            }
        }

        if (string.IsNullOrEmpty(palavraEncontrada))
        {
            return;
        }

        // Adiciona a palavra na lista das encontradas.
        palavrasEncontradas.Add(palavraEncontrada);

        // Mostra o nome completo na lateral.
        AtualizarNome(palavraEncontrada);

        // Bloqueia as letras pra não usar de novo.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.Bloquear();
        }

        Debug.Log("PALAVRA ENCONTRADA: " + palavraEncontrada);

        letrasSelecionadas.Clear();
        VerificarFim();
    }

    // Troca o texto pelo nome completo da pessoa.
    void AtualizarNome(string palavra)
    {
        if (palavra == "MARIGHELLA")
        {
            textoMarighella.text = "Carlos Marighella";
        }
        else if (palavra == "LAMARCA")
        {
            textoLamarca.text = "Carlos Lamarca";
        }
        else if (palavra == "HERZOG")
        {
            textoHerzog.text = "Vladimir Herzog";
        }
        else if (palavra == "BUARQUE")
        {
            textoBuarque.text = "Chico Buarque";
        }
        else if (palavra == "VANDRE")
        {
            textoVandre.text = "Geraldo Vandré";
        }
        else if (palavra == "GUIMARAES")
        {
            textoGuimaraes.text = "Ulysses Guimarães";
        }
        else if (palavra == "ROUSSEFF")
        {
            textoRousseff.text = "Dilma Rousseff";
        }
    }

    // Limpa as letras selecionadas quando errar.
    void LimparSelecao()
    {
        while (letrasSelecionadas.Count > 0)
        {
            LetraCaçaPalavras letra = letrasSelecionadas.Pop();

            // Só limpa se a letra não estiver bloqueada.
            if (!letra.bloqueada)
            {
                letra.MudarCor(corNormal);
            }
        }
    }

    // Verifica se já achou todas as palavras.
    void VerificarFim()
    {
        if (palavrasEncontradas.Count == palavras.Length)
        {
            Debug.Log("TODAS AS PALAVRAS FORAM ENCONTRADAS!");
            SceneManager.LoadScene("21_Cutscene04");
        }
    }
}
