
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelecaoCartas : MonoBehaviour
{
    [Header("Seleção das Cartas")]

    // Moldura que mostra qual carta o jogador está escolhendo.
    public RectTransform molduraSelecao;

    // Guarda as quatro cartas disponíveis no menu.
    public RectTransform[] cartas;

    // Guarda qual carta está selecionada no momento.
    private int cartaSelecionada = 0;

    // Marca quais cartas já foram entregues para impedir que sejam usadas novamente.
    private bool[] cartasUsadas = new bool[4];

    // Script responsável por fazer o menu tremer quando o jogador erra.
    private TremidaMenu tremidaMenu;

    void Awake()
    {
        // Procura o efeito de tremida no próprio objeto do menu.
        tremidaMenu = GetComponent<TremidaMenu>();
    }

    void OnEnable()
    {
        // Sempre começa selecionando a primeira carta que ainda não foi entregue.
        cartaSelecionada = EncontrarPrimeiraDisponivel();

        // Atualiza a aparência das cartas e coloca a moldura na seleção atual.
        AtualizarCartas();
        AtualizarMoldura();
    }

    void Update()
    {
        // Pega o teclado usando o New Input System.
        Keyboard teclado = Keyboard.current;

        // Se não existe um teclado disponível, não tenta ler nenhum comando.
        if (teclado == null)
        {
            return;
        }

        // Se apertou D ou seta direita, passa para a próxima carta disponível.
        if (teclado.dKey.wasPressedThisFrame || teclado.rightArrowKey.wasPressedThisFrame)
        {
            MoverSelecao(1);
        }

        // Se apertou A ou seta esquerda, volta para a carta anterior.
        if (teclado.aKey.wasPressedThisFrame || teclado.leftArrowKey.wasPressedThisFrame)
        {
            MoverSelecao(-1);
        }

        // Se apertou Espaço, tenta entregar a carta selecionada.
        if (teclado.spaceKey.wasPressedThisFrame)
        {
            ConfirmarCarta();
        }
    }

    void MoverSelecao(int direcao)
    {
        // Se todas as cartas já foram entregues, não tem mais para onde mover.
        if (TodasAsCartasUsadas())
        {
            return;
        }

        // Continua passando pelas cartas até encontrar uma que ainda esteja disponível.
        do
        {
            // Avança ou volta conforme a direção escolhida.
            cartaSelecionada += direcao;

            // Se passou da última carta, volta para a primeira.
            if (cartaSelecionada >= cartas.Length)
            {
                cartaSelecionada = 0;
            }

            // Se voltou antes da primeira, pula para a última.
            if (cartaSelecionada < 0)
            {
                cartaSelecionada = cartas.Length - 1;
            }
        }
        while (cartasUsadas[cartaSelecionada]);

        // Coloca a moldura em cima da nova carta selecionada.
        AtualizarMoldura();
    }

    void AtualizarMoldura()
    {
        // Se não sobrou nenhuma carta, esconde a moldura.
        if (TodasAsCartasUsadas())
        {
            molduraSelecao.gameObject.SetActive(false);
            return;
        }

        // Garante que a moldura esteja aparecendo.
        molduraSelecao.gameObject.SetActive(true);

        // Move a moldura para a posição da carta escolhida.
        molduraSelecao.position = cartas[cartaSelecionada].position;
    }

    void ConfirmarCarta()
    {
        // Se o sistema de entregas não existe, não tenta entregar nada.
        if (ControleEntrega.instancia == null)
        {
            return;
        }

        // Se essa carta já foi utilizada, impede que seja entregue novamente.
        if (cartasUsadas[cartaSelecionada])
        {
            return;
        }

        // Pega a cor da carta pelo nome do objeto, retirando a palavra "Carta".
        string corCartaEscolhida = cartas[cartaSelecionada].name.Replace("Carta", "");

        // Descobre a cor da caixa com a qual Maxwell está interagindo.
        string corDaCaixa = ControleEntrega.instancia.corCaixaAtual;

        // Se a cor da carta combina com a caixa, a entrega está correta.
        if (corCartaEscolhida == corDaCaixa)
        {
            // Toca o som de acerto.
            ControleEntrega.instancia.TocarSomCartaCerta();

            // Marca a carta como usada para impedir outra entrega.
            cartasUsadas[cartaSelecionada] = true;

            // Atualiza a aparência da carta que acabou de ser entregue.
            AtualizarCarta(cartaSelecionada);

            // Guarda a caixa atual antes de fechar o menu.
            CaixaCorreio caixaConcluida = ControleEntrega.instancia.caixaAtual;

            // Registra que essa caixa já recebeu sua carta corretamente.
            ControleEntrega.instancia.MarcarCaixaConcluida(corDaCaixa);

            // Fecha o menu e devolve o controle ao jogador.
            ControleEntrega.instancia.FecharMenuCartas();

            // Se a caixa ainda existe, desativa ela para impedir novas interações.
            if (caixaConcluida != null)
            {
                caixaConcluida.DesativarCaixa();
            }

            // Verifica se todas as cartas já foram entregues para finalizar a fase.
            ControleEntrega.instancia.VerificarVitoria();
        }
        else
        {
            // Se escolheu a carta errada, toca o som de erro.
            ControleEntrega.instancia.TocarSomCartaErrada();

            // Faz somente o menu tremer para mostrar que a escolha foi incorreta.
            if (tremidaMenu != null)
            {
                tremidaMenu.Tremer();
            }
        }
    }

    void AtualizarCartas()
    {
        // Passa por todas as cartas para atualizar quais estão disponíveis.
        for (int i = 0; i < cartas.Length; i++)
        {
            AtualizarCarta(i);
        }
    }

    void AtualizarCarta(int indice)
    {
        // Pega a imagem da carta que precisa ser atualizada.
        Image imagemCarta = cartas[indice].GetComponent<Image>();

        // Se não encontrou uma imagem, não tenta mudar sua aparência.
        if (imagemCarta == null)
        {
            return;
        }

        // Se a carta já foi entregue, deixa ela escura e transparente.
        if (cartasUsadas[indice])
        {
            imagemCarta.color = new Color(0.35f, 0.35f, 0.35f, 0.55f);
        }
        else
        {
            // Se ainda está disponível, mantém a cor normal.
            imagemCarta.color = Color.white;
        }
    }

    int EncontrarPrimeiraDisponivel()
    {
        // Procura a primeira carta que ainda não foi utilizada.
        for (int i = 0; i < cartasUsadas.Length; i++)
        {
            if (!cartasUsadas[i])
            {
                return i;
            }
        }

        // Se não encontrou nenhuma, devolve o primeiro índice.
        return 0;
    }

    bool TodasAsCartasUsadas()
    {
        // Passa por todas as cartas para conferir se ainda existe alguma disponível.
        for (int i = 0; i < cartasUsadas.Length; i++)
        {
            // Se encontrou uma carta não utilizada, ainda não terminou.
            if (!cartasUsadas[i])
            {
                return false;
            }
        }

        // Se todas já foram utilizadas, não sobrou nenhuma carta.
        return true;
    }
}
