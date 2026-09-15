using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        "LIBERDADEX",
        "QWERTYUIOP",
        "MARIGHELLA",
        "ASDFGHJKLZ",
        "LAMARCAQWE",
        "ZXCVBNMASD",
        "HERZOGTYUI",
        "BUARQUEFGH",
        "VANDREJKLP",
        "ULYSSESCVB"
    };

    // Palavras que podem ser encontradas.
    public string[] palavras =
    {
        "LIBERDADE",
        "MARIGHELLA",
        "LAMARCA",
        "HERZOG",
        "BUARQUE",
        "VANDRE",
        "ULYSSES"
    };

    [Header("Cores")]

    // Transparente quando não está selecionada.
    public Color corNormal = new Color(1f, 1f, 1f, 0f);

    // Vermelho enquanto a seleção ainda não formar uma palavra.
    public Color corErrada = new Color(1f, 0f, 0f, 0.45f);

    // Verde quando a palavra estiver correta.
    public Color corCorreta = new Color(0f, 1f, 0f, 0.45f);

    // Pilha usada para guardar as letras na ordem do arraste.
    private Stack<LetraCaçaPalavras> letrasSelecionadas = new Stack<LetraCaçaPalavras>();

    private bool selecionando = false;

    private Vector2Int direcao;
    private bool direcaoDefinida = false;

    private HashSet<string> palavrasEncontradas = new HashSet<string>();

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
                GameObject novaLetra = Instantiate(prefabLetra, gradeCacaPalavras);

                LetraCaçaPalavras letra = novaLetra.GetComponent<LetraCaçaPalavras>();

                if (letra != null)
                {
                    letra.Configurar(linhas[linha][coluna], linha, coluna, this);
                }
            }
        }
    }

    public void ComecarSelecao(LetraCaçaPalavras letra)
    {
        // Limpa qualquer seleção antiga e começa uma nova.
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

        // Não adiciona novamente a letra que já está no topo da pilha.
        if (letrasSelecionadas.Count > 0 && letrasSelecionadas.Peek() == letra)
        {
            return;
        }

        // Permite voltar uma casa e desfazer a última seleção.
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

        LetraCaçaPalavras ultimaLetra = letrasSelecionadas.Peek();

        int diferencaLinha = letra.linha - ultimaLetra.linha;
        int diferencaColuna = letra.coluna - ultimaLetra.coluna;

        // A próxima letra precisa estar encostada na anterior.
        if (Mathf.Abs(diferencaLinha) > 1 || Mathf.Abs(diferencaColuna) > 1)
        {
            return;
        }

        if (diferencaLinha == 0 && diferencaColuna == 0)
        {
            return;
        }

        Vector2Int novaDirecao = new Vector2Int(
            diferencaColuna == 0 ? 0 : (int)Mathf.Sign(diferencaColuna),
            diferencaLinha == 0 ? 0 : (int)Mathf.Sign(diferencaLinha)
        );

        // A segunda letra define a direção da palavra.
        if (!direcaoDefinida)
        {
            direcao = novaDirecao;
            direcaoDefinida = true;
        }
        else if (novaDirecao != direcao)
        {
            // Impede que o jogador faça uma seleção em formato de cobra.
            return;
        }

        // Usa Push para colocar a nova letra no topo da pilha.
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

    bool EhLetraAnterior(LetraCaçaPalavras letra)
    {
        if (letrasSelecionadas.Count < 2)
        {
            return false;
        }

        LetraCaçaPalavras[] letras = letrasSelecionadas.ToArray();

        // O índice 0 é o topo e o 1 é a letra selecionada antes dele.
        return letras[1] == letra;
    }

    void AtualizarCores()
    {
        string palavraAtual = MontarPalavra();

        Color cor = PalavraValida(palavraAtual) ? corCorreta : corErrada;

        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.MudarCor(cor);
        }
    }

    string MontarPalavra()
    {
        // A Stack entrega do último para o primeiro, então Reverse volta para a ordem do arraste.
        return string.Concat(letrasSelecionadas.Reverse().Select(letra => letra.textoLetra.text));
    }

    bool PalavraValida(string palavra)
    {
        foreach (string palavraCorreta in palavras)
        {
            // Aceita tanto a palavra normal quanto selecionada ao contrário.
            string palavraInvertida = new string(palavraCorreta.Reverse().ToArray());

            if (palavra == palavraCorreta || palavra == palavraInvertida)
            {
                return !palavrasEncontradas.Contains(palavraCorreta);
            }
        }

        return false;
    }

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

        palavrasEncontradas.Add(palavraEncontrada);

        // Bloqueia todas as letras que fizeram parte da palavra.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.Bloquear();
        }

        Debug.Log("PALAVRA ENCONTRADA: " + palavraEncontrada);

        letrasSelecionadas.Clear();

        VerificarFim();
    }

    void LimparSelecao()
    {
        // Desfaz somente as letras que ainda não foram bloqueadas.
        while (letrasSelecionadas.Count > 0)
        {
            LetraCaçaPalavras letra = letrasSelecionadas.Pop();

            if (!letra.bloqueada)
            {
                letra.MudarCor(corNormal);
            }
        }
    }

    void VerificarFim()
    {
        if (palavrasEncontradas.Count == palavras.Length)
        {
            Debug.Log("TODAS AS PALAVRAS FORAM ENCONTRADAS!");
        }
    }
}