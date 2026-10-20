
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisparoArmaFase4 : MonoBehaviour
{
    [Header("Controles")]
    public InputActionReference acaoAtirar;
    public InputActionReference acaoRecarregar;

    [Header("Referências")]
    public Transform pontaDoCano;
    public BalaFase4 prefabBala;

    [Header("Disparo")]
    public float intervaloEntreTiros = 0.35f;
    public float ajusteRotacaoBala = 180f;
    public int danoPorTiro = 10;

    [Header("Munição")]
    public int capacidadeCarregador = 6;
    public float tempoRecarga = 1.5f;

    [Header("Tiro Carregado")]
    public bool tiroCarregadoAtivo = false;
    public float tempoParaCarregar = 1.2f;
    public int multiplicadorDanoCarregado = 2;
    public float escalaBalaCarregada = 1.4f;

    private int municaoAtual;
    private float proximoDisparo;
    private bool recarregando;

    private bool carregando;
    private float inicioCarga;

    [Header("Efeito do Disparo")]
public GameObject claraoTiro;
public float duracaoClarao = 0.08f;

private Coroutine rotinaClarao;

    public bool EstaCarregando()
{
    if (tiroCarregadoAtivo && carregando && !recarregando)
    {
        return true;
    }
    else
    {
        return false;
    }
}

public int ConsultarMunicao()
{
    return municaoAtual;
}

public bool EstaRecarregando()
{
    return recarregando;
}

public float ProgressoCarga()
{
    if (EstaCarregando())
    {
        return Mathf.Clamp01((Time.time - inicioCarga) / Mathf.Max(0.01f, tempoParaCarregar));
    }
    else
    {
        return 0f;
    }
}

    void OnEnable()
    {
        if (acaoAtirar != null)
            acaoAtirar.action.Enable();

        if (acaoRecarregar != null)
            acaoRecarregar.action.Enable();
    }

    void Start()
    {
        municaoAtual = capacidadeCarregador;
    }

    void Update()
    {
        if (acaoAtirar == null || acaoRecarregar == null) return;

        if (acaoRecarregar.action.WasPressedThisFrame() && !recarregando && municaoAtual < capacidadeCarregador)
        {
            carregando = false;
            StartCoroutine(Recarregar());
        }

        if (recarregando) return;

        if (tiroCarregadoAtivo)
        {
            if (acaoAtirar.action.WasPressedThisFrame() && Time.time >= proximoDisparo)
            {
                if (municaoAtual > 0)
                {
                    carregando = true;
                    inicioCarga = Time.time;
                }
                else
                {
                    StartCoroutine(Recarregar());
                }
            }

            if (acaoAtirar.action.WasReleasedThisFrame() && carregando)
            {
                bool disparoCarregado = Time.time - inicioCarga >= tempoParaCarregar;

                carregando = false;
                Atirar(disparoCarregado);
            }
        }
        else if (acaoAtirar.action.WasPressedThisFrame() && Time.time >= proximoDisparo)
        {
            if (municaoAtual > 0)
                Atirar(false);
            else
                StartCoroutine(Recarregar());
        }
    }

    void Atirar(bool disparoCarregado)
    {
        Vector2 direcao = pontaDoCano.position - transform.position;

        if (direcao.sqrMagnitude < 0.001f) return;

        direcao.Normalize();

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        BalaFase4 bala = Instantiate(prefabBala, pontaDoCano.position, Quaternion.Euler(0, 0, angulo + ajusteRotacaoBala));

        bala.Inicializar(direcao);

        if (claraoTiro != null)
{
    if (rotinaClarao != null)
        StopCoroutine(rotinaClarao);

    rotinaClarao = StartCoroutine(MostrarClarao());
}

        if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarDisparo(disparoCarregado);

        // Ajusta o dano conforme o disparo
        bala.dano = disparoCarregado ? danoPorTiro * multiplicadorDanoCarregado : danoPorTiro;

        if (disparoCarregado)
            bala.transform.localScale *= escalaBalaCarregada;

        municaoAtual--;
        proximoDisparo = Time.time + intervaloEntreTiros;

        Debug.Log("Dano: " + bala.dano + " | Munição: " + municaoAtual + "/" + capacidadeCarregador);

        if (municaoAtual == 0)
            StartCoroutine(Recarregar());
    }

    IEnumerator Recarregar()
    {
        recarregando = true;
        carregando = false;

        if (AudioFase4.instancia != null)
    AudioFase4.instancia.TocarRecarga();

        Debug.Log("Recarregando...");

        yield return new WaitForSeconds(tempoRecarga);

        municaoAtual = capacidadeCarregador;
        recarregando = false;

        Debug.Log("Arma recarregada!");
    }

    public void AumentarCarregador(int quantidade)
    {
        if (quantidade <= 0) return;

        capacidadeCarregador += quantidade;
        municaoAtual = capacidadeCarregador;

        Debug.Log("Nova capacidade: " + capacidadeCarregador);
    }

    void OnDisable()
    {
        carregando = false;

        if (acaoAtirar != null)
            acaoAtirar.action.Disable();

        if (acaoRecarregar != null)
            acaoRecarregar.action.Disable();
    }

    IEnumerator MostrarClarao()
{
    claraoTiro.SetActive(true);

    yield return new WaitForSeconds(duracaoClarao);

    claraoTiro.SetActive(false);
    rotinaClarao = null;
}
}
