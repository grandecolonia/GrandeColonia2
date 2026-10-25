
using UnityEngine;
using System.Collections;

public class SistemaAlerta : MonoBehaviour
{
    [Header("Objetivo")]

    // Indicador que mostra onde Maxwell precisa chegar para sair do labirinto.
    public GameObject indicadorSaida;

    [Header("Jogador")]

    // Referência ao Maxwell que será levado para o começo da perseguição.
    public Transform jogador;

    // Local para onde Maxwell volta quando o alerta é ativado.
    public Transform pontoInicio;

    // Rigidbody usado para interromper o movimento antes de reposicionar Maxwell.
    private Rigidbody2D rbJogador;

    [Header("Controle")]

    // Sistema que informa quando o alerta do labirinto foi ativado.
    public ControleLabirinto controleLabirinto;

    [Header("Visual")]

    // Objeto que contém o efeito vermelho mostrado durante o alerta.
    public GameObject alertaVisual;

    // CanvasGroup usado para controlar a intensidade do vermelho.
    public CanvasGroup canvasGroup;

    // Intensidade mais forte do efeito vermelho na tela.
    [Range(0f, 1f)]
    public float vermelhoMaximo = 0.25f;

    // Intensidade mais fraca do efeito vermelho na tela.
    [Range(0f, 1f)]
    public float vermelhoMinimo = 0.08f;

    [Header("Áudio")]

    // AudioSource responsável por reproduzir a sirene.
    public AudioSource audioSource;

    // Som que será reproduzido quando a perseguição começar.
    public AudioClip sirene;

    [Header("Duração")]

    // Tempo que a sirene e o efeito vermelho permanecem ativos.
    public float duracaoAlarme = 5f;

    // Impede que o alerta seja iniciado mais de uma vez.
    private bool alertaIniciado = false;

    // Controla se a sirene e o efeito vermelho ainda devem continuar.
    private bool alarmeAtivo = false;

    void Start()
    {
        // Procura o Rigidbody2D de Maxwell para controlar seu reposicionamento.
        rbJogador = jogador.GetComponent<Rigidbody2D>();

        // Começa com o indicador de saída escondido.
        indicadorSaida.SetActive(false);
    }

    void Update()
    {
        // Se o alerta foi liberado e ainda não começou, inicia a perseguição.
        if (controleLabirinto.alertaAtivo && !alertaIniciado)
        {
            IniciarAlerta();
        }
    }

    void IniciarAlerta()
    {
        // Marca que o alerta começou para impedir novas ativações.
        alertaIniciado = true;

        // Libera a animação do efeito vermelho.
        alarmeAtivo = true;

        // Leva Maxwell de volta ao ponto onde a perseguição deve começar.
        VoltarJogadorParaInicio();

        // Mostra o indicador que aponta para a saída.
        indicadorSaida.SetActive(true);

        // Ativa o efeito vermelho na tela.
        alertaVisual.SetActive(true);

        // Configura a sirene para ficar repetindo enquanto o alarme estiver ativo.
        audioSource.clip = sirene;
        audioSource.loop = true;

        // Começa a tocar o som da sirene.
        audioSource.Play();

        // Inicia o efeito vermelho piscando na tela.
        StartCoroutine(PiscarAlerta());

        // Começa a contar o tempo até desligar a sirene e o efeito visual.
        StartCoroutine(DesligarAlarme());
    }

    IEnumerator PiscarAlerta()
    {
        // Mantém o vermelho piscando enquanto o alarme estiver ativo.
        while (alarmeAtivo)
        {
            // Deixa o efeito vermelho na intensidade mais forte.
            canvasGroup.alpha = vermelhoMaximo;

            // Espera um pouco antes de diminuir a intensidade.
            yield return new WaitForSeconds(0.4f);

            // Diminui a intensidade do vermelho.
            canvasGroup.alpha = vermelhoMinimo;

            // Espera novamente antes de repetir o efeito.
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator DesligarAlarme()
    {
        // Mantém o alarme funcionando pelo tempo configurado.
        yield return new WaitForSeconds(duracaoAlarme);

        // Encerra a repetição do efeito vermelho.
        alarmeAtivo = false;

        // Para o som da sirene.
        audioSource.Stop();

        // Esconde o efeito visual do alerta.
        alertaVisual.SetActive(false);

        // Garante que não fique nenhuma transparência vermelha na tela.
        canvasGroup.alpha = 0f;
    }

    void VoltarJogadorParaInicio()
    {
        // Se Maxwell possui Rigidbody2D, reposiciona usando a física.
        if (rbJogador != null)
        {
            // Interrompe qualquer movimento antes de mudar de posição.
            rbJogador.linearVelocity = Vector2.zero;

            // Leva Maxwell para o ponto inicial da perseguição.
            rbJogador.position = pontoInicio.position;
        }
        else
        {
            // Se não possui Rigidbody2D, reposiciona pelo Transform.
            jogador.position = pontoInicio.position;
        }

        // Atualiza as posições no sistema de física depois do reposicionamento.
        Physics2D.SyncTransforms();
    }
}
