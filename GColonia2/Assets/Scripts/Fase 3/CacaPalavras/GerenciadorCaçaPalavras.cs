
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorCaçaPalavras : MonoBehaviour
{
    [Header("Grade")]

    // Objeto onde todas as letras serão organizadas.
    public Transform gradeCacaPalavras;

    // Prefab utilizado para criar cada letra do caça-palavras.
    public GameObject prefabLetra;

    [Header("Caça-palavras")]

    // Linhas que formam a grade de letras do jogo.
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

    // Palavras que Maxwell precisa encontrar para concluir o minijogo.
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

    // Textos que mostram os nomes completos depois que as palavras são encontradas.
    public TMP_Text textoMarighella;
    public TMP_Text textoLamarca;
    public TMP_Text textoHerzog;
    public TMP_Text textoBuarque;
    public TMP_Text textoVandre;
    public TMP_Text textoGuimaraes;
    public TMP_Text textoRousseff;

    [Header("Cores")]

    // Cor normal do fundo das letras.
    public Color corNormal = new Color(1f, 1f, 1f, 0f);

    // Cor mostrada enquanto a seleção ainda não forma uma palavra correta.
    public Color corErrada = new Color(1f, 0f, 0f, 0.45f);

    // Cor mostrada quando Maxwell seleciona uma palavra correta.
    public Color corCorreta = new Color(0f, 1f, 0f, 0.45f);

    // Guarda as letras selecionadas na ordem em que Maxwell passou por elas.
    private Stack<LetraCaçaPalavras> letrasSelecionadas = new Stack<LetraCaçaPalavras>();

    // Guarda as palavras que Maxwell já encontrou.
    private List<string> palavrasEncontradas = new List<string>();

    // Informa se Maxwell está selecionando letras no momento.
    private bool selecionando = false;

    // Guarda a direção em que as letras estão sendo selecionadas.
    private Vector2 direcao;

    // Informa se a direção da seleção já foi definida.
    private bool direcaoDefinida = false;

    void Start()
    {
        // Cria todas as letras do caça-palavras quando a cena começa.
        CriarGrade();
    }

    void CriarGrade()
    {
        // Percorre cada linha configurada no caça-palavras.
        for (int linha = 0; linha < linhas.Length; linha++)
        {
            // Percorre cada letra da linha atual.
            for (int coluna = 0; coluna < linhas[linha].Length; coluna++)
            {
                // Cria uma nova letra dentro do objeto que organiza a grade.
                GameObject novaLetra = Instantiate(prefabLetra, gradeCacaPalavras);

                // Procura o script responsável por controlar a letra criada.
                LetraCaçaPalavras letra = novaLetra.GetComponent<LetraCaçaPalavras>();

                // Se encontrou o script, configura a letra e sua posição na grade.
                if (letra != null)
                {
                    letra.Configurar(linhas[linha][coluna], linha, coluna, this);
                }
            }
        }
    }

    public void ComecarSelecao(LetraCaçaPalavras letra)
    {
        // Limpa qualquer seleção anterior que ainda esteja aparecendo.
        LimparSelecao();

        // Marca que Maxwell começou uma nova seleção.
        selecionando = true;

        // Permite escolher uma nova direção para a palavra.
        direcaoDefinida = false;

        // Adiciona a primeira letra selecionada.
        letrasSelecionadas.Push(letra);

        // Atualiza as cores para mostrar o início da seleção.
        AtualizarCores();
    }

    public void PassarPorLetra(LetraCaçaPalavras letra)
    {
        // Se não está selecionando ou a letra já foi encontrada, não continua.
        if (!selecionando || letra.bloqueada)
        {
            return;
        }

        // Impede selecionar novamente a última letra.
        if (letrasSelecionadas.Count > 0 && letrasSelecionadas.Peek() == letra)
        {
            return;
        }

        // Se voltou para a letra anterior, desfaz a última seleção.
        if (EhLetraAnterior(letra))
        {
            // Remove a última letra da seleção.
            LetraCaçaPalavras removida = letrasSelecionadas.Pop();

            // Devolve a cor normal para a letra removida.
            removida.MudarCor(corNormal);

            // Se restou menos de duas letras, permite escolher outra direção.
            if (letrasSelecionadas.Count < 2)
            {
                direcaoDefinida = false;
            }

            // Atualiza as cores das letras que continuam selecionadas.
            AtualizarCores();
            return;
        }

        // Pega a última letra escolhida para comparar as posições.
        LetraCaçaPalavras ultimaLetra = letrasSelecionadas.Peek();

        // Calcula a distância entre as linhas das duas letras.
        int diferencaLinha = letra.linha - ultimaLetra.linha;

        // Calcula a distância entre as colunas das duas letras.
        int diferencaColuna = letra.coluna - ultimaLetra.coluna;

        // Impede selecionar letras que não estejam próximas.
        if (Mathf.Abs(diferencaLinha) > 1 || Mathf.Abs(diferencaColuna) > 1)
        {
            return;
        }

        // Impede selecionar novamente a mesma posição.
        if (diferencaLinha == 0 && diferencaColuna == 0)
        {
            return;
        }

        // Descobre a direção da nova letra em relação à anterior.
        Vector2 novaDirecao = new Vector2(diferencaColuna == 0 ? 0 : Mathf.Sign(diferencaColuna), diferencaLinha == 0 ? 0 : Mathf.Sign(diferencaLinha));

        // Se ainda não existe direção, utiliza a segunda letra para defini-la.
        if (!direcaoDefinida)
        {
            direcao = novaDirecao;
            direcaoDefinida = true;
        }
        else if (novaDirecao != direcao)
        {
            // Se Maxwell tentou mudar de direção, impede continuar a seleção.
            return;
        }

        // Adiciona a nova letra à seleção.
        letrasSelecionadas.Push(letra);

        // Atualiza as cores conforme a palavra formada.
        AtualizarCores();
    }

    public void TerminarSelecao()
    {
        // Se não existe uma seleção em andamento, não faz nada.
        if (!selecionando)
        {
            return;
        }

        // Marca que Maxwell terminou de selecionar as letras.
        selecionando = false;

        // Junta as letras selecionadas para formar a palavra.
        string palavraSelecionada = MontarPalavra();

        // Se a palavra estiver correta, confirma que foi encontrada.
        if (PalavraValida(palavraSelecionada))
        {
            ConfirmarPalavra(palavraSelecionada);
        }
        else
        {
            // Se a palavra estiver errada, limpa a seleção.
            LimparSelecao();
        }

        // Permite escolher outra direção na próxima tentativa.
        direcaoDefinida = false;
    }

    bool EhLetraAnterior(LetraCaçaPalavras letra)
    {
        // É necessário ter pelo menos duas letras para voltar.
        if (letrasSelecionadas.Count < 2)
        {
            return false;
        }

        // Transforma a seleção em um vetor, começando pela última letra.
        LetraCaçaPalavras[] letras = letrasSelecionadas.ToArray();

        // Verifica se Maxwell voltou para a letra anterior.
        return letras[1] == letra;
    }

    void AtualizarCores()
    {
        // Monta a palavra que Maxwell está selecionando.
        string palavraAtual = MontarPalavra();

        // Usa verde se a palavra estiver correta e vermelho caso contrário.
        Color cor = PalavraValida(palavraAtual) ? corCorreta : corErrada;

        // Aplica a cor escolhida a todas as letras da seleção.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.MudarCor(cor);
        }
    }

    string MontarPalavra()
    {
        // Começa com uma palavra vazia.
        string palavra = "";

        // Percorre as letras na ordem correta, da primeira até a última.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas.Reverse())
        {
            // Acrescenta cada letra para formar a palavra.
            palavra += letra.textoLetra.text;
        }

        // Devolve a palavra formada.
        return palavra;
    }

    bool PalavraValida(string palavra)
    {
        // Compara a seleção com todas as palavras que precisam ser encontradas.
        foreach (string palavraCorreta in palavras)
        {
            // Inverte a palavra para permitir encontrar também de trás para frente.
            string palavraInvertida = new string(palavraCorreta.Reverse().ToArray());

            // Confere se a palavra selecionada corresponde a uma das duas direções.
            if (palavra == palavraCorreta || palavra == palavraInvertida)
            {
                // Só aceita a palavra se ela ainda não tiver sido encontrada.
                return !palavrasEncontradas.Contains(palavraCorreta);
            }
        }

        // Se não corresponde a nenhuma palavra, a seleção está errada.
        return false;
    }

    void ConfirmarPalavra(string palavraSelecionada)
    {
        // Guarda o nome da palavra correta encontrada.
        string palavraEncontrada = "";

        // Procura qual das palavras corresponde à seleção.
        foreach (string palavraCorreta in palavras)
        {
            // Prepara também a versão invertida da palavra.
            string palavraInvertida = new string(palavraCorreta.Reverse().ToArray());

            // Verifica se Maxwell encontrou a palavra em alguma das direções.
            if (palavraSelecionada == palavraCorreta || palavraSelecionada == palavraInvertida)
            {
                palavraEncontrada = palavraCorreta;
                break;
            }
        }

        // Se nenhuma palavra foi identificada, cancela a confirmação.
        if (string.IsNullOrEmpty(palavraEncontrada))
        {
            return;
        }

        // Guarda a palavra para impedir que ela seja encontrada novamente.
        palavrasEncontradas.Add(palavraEncontrada);

        // Mostra o nome completo da pessoa na lateral do caça-palavras.
        AtualizarNome(palavraEncontrada);

        // Bloqueia as letras utilizadas para marcar que a palavra foi encontrada.
        foreach (LetraCaçaPalavras letra in letrasSelecionadas)
        {
            letra.Bloquear();
        }

        // Limpa a seleção atual sem alterar as letras já bloqueadas.
        letrasSelecionadas.Clear();

        // Verifica se Maxwell encontrou todas as palavras.
        VerificarFim();
    }

    void AtualizarNome(string palavra)
    {
        // Mostra o nome completo correspondente à palavra encontrada.
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

    void LimparSelecao()
    {
        // Continua removendo letras até a seleção ficar vazia.
        while (letrasSelecionadas.Count > 0)
        {
            // Retira a última letra selecionada.
            LetraCaçaPalavras letra = letrasSelecionadas.Pop();

            // Só devolve a cor normal se a letra ainda não estiver bloqueada.
            if (!letra.bloqueada)
            {
                letra.MudarCor(corNormal);
            }
        }
    }

    void VerificarFim()
    {
        // Se todas as palavras foram encontradas, termina o minijogo.
        if (palavrasEncontradas.Count == palavras.Length)
        {
            // Carrega a cutscene que vem depois do caça-palavras.
            SceneManager.LoadScene("21_Cutscene04");
        }
    }
}
