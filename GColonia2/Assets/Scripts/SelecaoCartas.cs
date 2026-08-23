using UnityEngine;
using UnityEngine.UI;

public class SelecaoCartas : MonoBehaviour
{
    // Elementos usados para mostrar e movimentar a seleção entre as cartas.
    public RectTransform molduraSelecao;
    public RectTransform[] cartas;

    // Guarda qual carta está selecionada no momento.
    private int cartaSelecionada = 0;

    // Registra quais cartas já foram entregues corretamente.
    private bool[] cartasUsadas = new bool[4];

    // Controla o efeito visual exibido quando o jogador escolhe a carta errada.
    private TremidaMenu tremidaMenu;

    void Awake()
    {
        tremidaMenu = GetComponent<TremidaMenu>();
    }

    void OnEnable()
    {
        // Sempre começa selecionando a primeira carta que ainda estiver disponível.
        cartaSelecionada = EncontrarPrimeiraDisponivel();

        AtualizarCartas();
        AtualizarMoldura();
    }

    void Update()
    {
        // Move a seleção para a direita.
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoverSelecao(1);
        }

        // Move a seleção para a esquerda.
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoverSelecao(-1);
        }

        // Confirma a carta que está selecionada.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmarCarta();
        }
    }

    void MoverSelecao(int direcao)
    {
        // Impede a movimentação caso todas as cartas já tenham sido usadas.
        if (TodasAsCartasUsadas())
        {
            return;
        }

        // Continua procurando até encontrar uma carta disponível.
        do
        {
            cartaSelecionada += direcao;

            if (cartaSelecionada >= cartas.Length)
            {
                cartaSelecionada = 0;
            }

            if (cartaSelecionada < 0)
            {
                cartaSelecionada = cartas.Length - 1;
            }
        }
        while (cartasUsadas[cartaSelecionada]);

        AtualizarMoldura();
    }

    void AtualizarMoldura()
    {
        // Esconde a moldura quando não existem mais cartas disponíveis.
        if (TodasAsCartasUsadas())
        {
            molduraSelecao.gameObject.SetActive(false);
            return;
        }

        // Posiciona a moldura sobre a carta selecionada.
        molduraSelecao.gameObject.SetActive(true);
        molduraSelecao.position = cartas[cartaSelecionada].position;
    }

    void ConfirmarCarta()
    {
        // Interrompe a confirmação caso o sistema de entregas não esteja ativo.
        if (ControleEntrega.instancia == null)
        {
            return;
        }

        // Impede que uma carta já entregue seja escolhida novamente.
        if (cartasUsadas[cartaSelecionada])
        {
            return;
        }

        string corCartaEscolhida = cartas[cartaSelecionada].name.Replace("Carta", "");
        string corDaCaixa = ControleEntrega.instancia.corCaixaAtual;

        // Verifica se a cor da carta escolhida corresponde à caixa atual.
        if (corCartaEscolhida == corDaCaixa)
        {
            Debug.Log("ACERTOU! Carta: " + corCartaEscolhida);

            ControleEntrega.instancia.TocarSomCartaCerta();

            cartasUsadas[cartaSelecionada] = true;
            AtualizarCarta(cartaSelecionada);

            CaixaCorreio caixaConcluida = ControleEntrega.instancia.caixaAtual;

            ControleEntrega.instancia.MarcarCaixaConcluida(corDaCaixa);
            ControleEntrega.instancia.FecharMenuCartas();

            if (caixaConcluida != null)
            {
                caixaConcluida.DesativarCaixa();
            }

            ControleEntrega.instancia.VerificarVitoria();
        }
        else
        {
            Debug.Log("ERROU! Escolheu " + corCartaEscolhida + ", mas a caixa é " + corDaCaixa);

            ControleEntrega.instancia.TocarSomCartaErrada();

            // Aplica o efeito de tremida no menu após uma escolha incorreta.
            if (tremidaMenu != null)
            {
                tremidaMenu.Tremer();
            }
        }
    }

    void AtualizarCartas()
    {
        // Atualiza a aparência de todas as cartas do menu.
        for (int i = 0; i < cartas.Length; i++)
        {
            AtualizarCarta(i);
        }
    }

    void AtualizarCarta(int indice)
    {
        Image imagemCarta = cartas[indice].GetComponent<Image>();

        if (imagemCarta == null)
        {
            return;
        }

        if (cartasUsadas[indice])
        {
            // Deixa a carta entregue escura e transparente.
            imagemCarta.color = new Color(0.35f, 0.35f, 0.35f, 0.55f);
        }
        else
        {
            // Mantém a aparência normal das cartas disponíveis.
            imagemCarta.color = Color.white;
        }
    }

    int EncontrarPrimeiraDisponivel()
    {
        // Procura a primeira carta que ainda não foi entregue.
        for (int i = 0; i < cartasUsadas.Length; i++)
        {
            if (!cartasUsadas[i])
            {
                return i;
            }
        }

        return 0;
    }

    bool TodasAsCartasUsadas()
    {
        // Verifica se ainda existe alguma carta disponível.
        for (int i = 0; i < cartasUsadas.Length; i++)
        {
            if (!cartasUsadas[i])
            {
                return false;
            }
        }

        return true;
    }
}