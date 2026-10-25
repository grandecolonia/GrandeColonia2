
using UnityEngine;
using UnityEngine.InputSystem;

public class InteracaoCartao : MonoBehaviour
{
    [Header("Cartão")]

    // Cartão que vai desaparecer depois que Maxwell coletar.
    public GameObject cartao;

    // Sistema que guarda o progresso e controla o alerta do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Interface")]

    // Ícone que aparece quando Maxwell pode pegar o cartão.
    public GameObject iconeE;

    [Header("Input")]

    // Ação que identifica quando o jogador aperta o comando de interação.
    public InputActionReference interagir;

    // Guarda se Maxwell está dentro da área do cartão.
    private bool jogadorNaArea = false;

    // Impede que o cartão seja coletado mais de uma vez.
    private bool coletado = false;

    void OnEnable()
    {
        // Habilita a ação de interação quando este componente fica ativo.
        if (interagir != null)
        {
            interagir.action.Enable();
        }
    }

    void Start()
    {
        // Começa com o ícone escondido até Maxwell poder interagir.
        iconeE.SetActive(false);
    }

    void Update()
    {
        // Se Maxwell já pegou o cartão ou está longe, não faz nada.
        if (coletado || !jogadorNaArea)
        {
            return;
        }

        // Só mostra o ícone quando a energia do labirinto estiver ligada.
        iconeE.SetActive(controleLabirinto.energiaLigada);

        // Se a energia está ligada e Maxwell apertou o comando, coleta o cartão.
        if (controleLabirinto.energiaLigada && interagir.action.WasPressedThisFrame())
        {
            ColetarCartao();
        }
    }

    void ColetarCartao()
    {
        // Se a energia ainda não foi ligada, impede a coleta.
        if (!controleLabirinto.energiaLigada)
        {
            return;
        }

        // Marca que o cartão já foi coletado.
        coletado = true;

        // Avisa ao controle do labirinto que Maxwell agora possui o cartão.
        controleLabirinto.temCartao = true;

        // Ativa o alerta para os guardas começarem a perseguição.
        controleLabirinto.alertaAtivo = true;

        // Esconde o ícone porque o cartão já foi coletado.
        iconeE.SetActive(false);

        // Remove o cartão da cena depois da coleta.
        cartao.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se Maxwell entrou na área e ainda não pegou o cartão, permite verificar a interação.
        if (collision.CompareTag("Player") && !coletado)
        {
            // Marca que Maxwell está perto do cartão.
            jogadorNaArea = true;

            // Se a energia já está ligada, mostra o ícone imediatamente.
            if (controleLabirinto.energiaLigada)
            {
                iconeE.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se Maxwell se afastou, cancela a interação e esconde o ícone.
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            iconeE.SetActive(false);
        }
    }
}
