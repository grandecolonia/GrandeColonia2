using UnityEngine;
using System.Collections;

public class SistemaAlerta : MonoBehaviour
{
    [Header("Objetivo")]

    // Indicador que mostra onde fica a saída depois que o alerta começa.
    public GameObject indicadorSaida;

    [Header("Jogador")]

    // Maxwell e o ponto para onde ele volta quando o alerta é ativado.
    public Transform jogador;
    public Transform pontoInicio;

    private Rigidbody2D rbJogador;

    [Header("Controle")]

    // Controle geral do labirinto.
    public ControleLabirinto controleLabirinto;

    [Header("Visual")]

    // Elementos usados no efeito visual do alerta.
    public GameObject alertaVisual;
    public CanvasGroup canvasGroup;

    // Controla a intensidade máxima e mínima do vermelho.
[Range(0f, 1f)] public float vermelhoMaximo = 0.25f;
[Range(0f, 1f)] public float vermelhoMinimo = 0.08f;

    [Header("Áudio")]

    // AudioSource e som usados na sirene.
    public AudioSource audioSource;
    public AudioClip sirene;

    [Header("Duração")]

    // Tempo que a sirene e o aviso ficam ativos.
    public float duracaoAlarme = 5f;

    private bool alertaIniciado = false;
    private bool alarmeAtivo = false;

    void Start()
    {
        rbJogador = jogador.GetComponent<Rigidbody2D>();

        // Começa com o indicador da saída escondido.
        indicadorSaida.SetActive(false);
    }

    void Update()
    {
        // Inicia o alerta uma única vez quando ele for ativado no controle do labirinto.
        if (controleLabirinto.alertaAtivo && !alertaIniciado)
        {
            IniciarAlerta();
        }
    }

    void IniciarAlerta()
    {
        alertaIniciado = true;
        alarmeAtivo = true;

        // Volta Maxwell para o começo da perseguição.
        VoltarJogadorParaInicio();

        // Mostra onde fica a saída.
        indicadorSaida.SetActive(true);

        // Ativa o aviso visual e começa a sirene.
        alertaVisual.SetActive(true);

        audioSource.clip = sirene;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(PiscarAlerta());
        StartCoroutine(DesligarAlarme());
    }

IEnumerator PiscarAlerta()
{
    while (alarmeAtivo)
    {
        canvasGroup.alpha = vermelhoMaximo;
        yield return new WaitForSeconds(0.4f);

        canvasGroup.alpha = vermelhoMinimo;
        yield return new WaitForSeconds(0.4f);
    }
}
    IEnumerator DesligarAlarme()
    {
        yield return new WaitForSeconds(duracaoAlarme);

        alarmeAtivo = false;

        // Para somente o som e o aviso visual.
        audioSource.Stop();
        alertaVisual.SetActive(false);

        canvasGroup.alpha = 0f;
    }

    void VoltarJogadorParaInicio()
    {
        // Para o movimento e leva Maxwell de volta para o ponto inicial.
        if (rbJogador != null)
        {
            rbJogador.linearVelocity = Vector2.zero;
            rbJogador.position = pontoInicio.position;
        }
        else
        {
            jogador.position = pontoInicio.position;
        }

        Physics2D.SyncTransforms();
    }
}