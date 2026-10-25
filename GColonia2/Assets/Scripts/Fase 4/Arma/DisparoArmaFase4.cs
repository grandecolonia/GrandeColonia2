
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisparoArmaFase4 : MonoBehaviour
{
    [Header("Controles")]

    // Ação utilizada para atirar ou carregar um disparo.
    public InputActionReference acaoAtirar;

    // Ação utilizada para recarregar a arma manualmente.
    public InputActionReference acaoRecarregar;

    [Header("Referências")]

    // Ponto onde as balas serão criadas.
    public Transform pontaDoCano;

    // Prefab da bala utilizada nos disparos.
    public BalaFase4 prefabBala;

    [Header("Disparo")]

    // Tempo mínimo de espera entre dois disparos.
    public float intervaloEntreTiros = 0.35f;

    // Ajuste da rotação visual da bala ao ser criada.
    public float ajusteRotacaoBala = 180f;

    // Quantidade de dano causada por um disparo normal.
    public int danoPorTiro = 10;

    [Header("Munição")]

    // Quantidade máxima de balas que cabem no carregador.
    public int capacidadeCarregador = 6;

    // Tempo necessário para completar a recarga.
    public float tempoRecarga = 1.5f;

    [Header("Tiro Carregado")]

    // Permite utilizar o sistema de tiro carregado.
    public bool tiroCarregadoAtivo = false;

    // Tempo necessário para carregar completamente o disparo.
    public float tempoParaCarregar = 1.2f;

    // Multiplicador aplicado ao dano do tiro carregado.
    public int multiplicadorDanoCarregado = 2;

    // Aumento do tamanho visual da bala carregada.
    public float escalaBalaCarregada = 1.4f;

    [Header("Efeito do Disparo")]

    // Objeto visual que aparece quando a arma dispara.
    public GameObject claraoTiro;

    // Tempo que o clarão permanece visível.
    public float duracaoClarao = 0.08f;

    // Guarda a quantidade atual de balas.
    private int municaoAtual;

    // Guarda quando a arma poderá disparar novamente.
    private float proximoDisparo;

    // Informa se a arma está recarregando.
    private bool recarregando;

    // Informa se Maxwell está segurando o comando para carregar um tiro.
    private bool carregando;

    // Guarda o momento em que Maxwell começou a carregar o disparo.
    private float inicioCarga;

    // Guarda a corrotina do clarão para impedir efeitos sobrepostos.
    private Coroutine rotinaClarao;

    public bool EstaCarregando()
    {
        // Verifica se o tiro carregado está ativo e a arma está carregando um disparo.
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
        // Devolve a quantidade atual de balas para outros scripts.
        return municaoAtual;
    }

    public bool EstaRecarregando()
    {
        // Informa se a arma está realizando uma recarga.
        return recarregando;
    }

    public float ProgressoCarga()
    {
        // Se Maxwell está carregando um tiro, calcula o progresso de zero a um.
        if (EstaCarregando())
        {
            return Mathf.Clamp01((Time.time - inicioCarga) / Mathf.Max(0.01f, tempoParaCarregar));
        }
        else
        {
            // Se não está carregando, devolve zero.
            return 0f;
        }
    }

    void OnEnable()
    {
        // Habilita a ação de disparo.
        if (acaoAtirar != null)
        {
            acaoAtirar.action.Enable();
        }

        // Habilita a ação de recarga.
        if (acaoRecarregar != null)
        {
            acaoRecarregar.action.Enable();
        }
    }

    void Start()
    {
        // Começa com o carregador completamente cheio.
        municaoAtual = capacidadeCarregador;
    }

    void Update()
    {
        // Se alguma ação não foi configurada, não tenta utilizar os controles.
        if (acaoAtirar == null || acaoRecarregar == null)
        {
            return;
        }

        // Se Maxwell pediu recarga e ainda existem espaços no carregador, começa a recarregar.
        if (acaoRecarregar.action.WasPressedThisFrame() && !recarregando && municaoAtual < capacidadeCarregador)
        {
            // Cancela qualquer disparo que estivesse sendo carregado.
            carregando = false;

            // Inicia a recarga da arma.
            StartCoroutine(Recarregar());
        }

        // Impede novos disparos enquanto a arma estiver recarregando.
        if (recarregando)
        {
            return;
        }

        // Se o tiro carregado está ativo, utiliza o sistema de segurar e soltar.
        if (tiroCarregadoAtivo)
        {
            // Começa a carregar um disparo quando Maxwell aperta o comando.
            if (acaoAtirar.action.WasPressedThisFrame() && Time.time >= proximoDisparo)
            {
                // Se ainda existem balas, começa a contar o tempo de carga.
                if (municaoAtual > 0)
                {
                    carregando = true;
                    inicioCarga = Time.time;
                }
                else
                {
                    // Se acabou a munição, começa a recarga.
                    StartCoroutine(Recarregar());
                }
            }

            // Quando Maxwell solta o comando, verifica se carregou o suficiente.
            if (acaoAtirar.action.WasReleasedThisFrame() && carregando)
            {
                // Confere se o tempo necessário para o tiro carregado foi alcançado.
                bool disparoCarregado = Time.time - inicioCarga >= tempoParaCarregar;

                // Encerra o carregamento e dispara a bala.
                carregando = false;
                Atirar(disparoCarregado);
            }
        }
        else if (acaoAtirar.action.WasPressedThisFrame() && Time.time >= proximoDisparo)
        {
            // Se o tiro carregado está desativado, dispara normalmente ao apertar.
            if (municaoAtual > 0)
            {
                Atirar(false);
            }
            else
            {
                // Se não existem balas, começa a recarga.
                StartCoroutine(Recarregar());
            }
        }
    }

    void Atirar(bool disparoCarregado)
    {
        // Calcula a direção entre a arma e a ponta do cano.
        Vector2 direcao = pontaDoCano.position - transform.position;

        // Impede o disparo se a direção calculada for praticamente zero.
        if (direcao.sqrMagnitude < 0.001f)
        {
            return;
        }

        // Normaliza a direção para manter a velocidade correta da bala.
        direcao.Normalize();

        // Calcula o ângulo da bala conforme a direção do disparo.
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        // Cria a bala na ponta do cano com a rotação configurada.
        BalaFase4 bala = Instantiate(prefabBala, pontaDoCano.position, Quaternion.Euler(0f, 0f, angulo + ajusteRotacaoBala));

        // Informa à bala a direção em que ela deve viajar.
        bala.Inicializar(direcao);

        // Se existe um efeito de clarão, reproduz a animação do disparo.
        if (claraoTiro != null)
        {
            // Interrompe o clarão anterior caso outro disparo aconteça rapidamente.
            if (rotinaClarao != null)
            {
                StopCoroutine(rotinaClarao);
            }

            // Inicia o novo clarão da arma.
            rotinaClarao = StartCoroutine(MostrarClarao());
        }

        // Reproduz o som correspondente ao tipo de disparo.
        if (AudioFase4.instancia != null)
        {
            AudioFase4.instancia.TocarDisparo(disparoCarregado);
        }

        // Define o dano da bala conforme o disparo normal ou carregado.
        bala.dano = disparoCarregado ? danoPorTiro * multiplicadorDanoCarregado : danoPorTiro;

        // Aumenta o tamanho visual da bala quando o tiro está carregado.
        if (disparoCarregado)
        {
            bala.transform.localScale *= escalaBalaCarregada;
        }

        // Gasta uma bala do carregador.
        municaoAtual--;

        // Define quando Maxwell poderá disparar novamente.
        proximoDisparo = Time.time + intervaloEntreTiros;

        // Se a última bala foi utilizada, começa a recarga automática.
        if (municaoAtual == 0)
        {
            StartCoroutine(Recarregar());
        }
    }

    IEnumerator Recarregar()
    {
        // Marca que a arma está recarregando.
        recarregando = true;

        // Cancela qualquer tiro que estivesse sendo carregado.
        carregando = false;

        // Reproduz o som de recarga.
        if (AudioFase4.instancia != null)
        {
            AudioFase4.instancia.TocarRecarga();
        }

        // Espera o tempo configurado para completar a recarga.
        yield return new WaitForSeconds(tempoRecarga);

        // Enche o carregador novamente.
        municaoAtual = capacidadeCarregador;

        // Libera a arma para voltar a disparar.
        recarregando = false;
    }

    public void AumentarCarregador(int quantidade)
    {
        // Impede aumentar o carregador com valores inválidos.
        if (quantidade <= 0)
        {
            return;
        }

        // Aumenta a quantidade máxima de balas.
        capacidadeCarregador += quantidade;

        // Preenche completamente o carregador depois da melhoria.
        municaoAtual = capacidadeCarregador;
    }

    void OnDisable()
    {
        // Cancela qualquer disparo que estivesse sendo carregado.
        carregando = false;

        // Desabilita a ação de disparo.
        if (acaoAtirar != null)
        {
            acaoAtirar.action.Disable();
        }

        // Desabilita a ação de recarga.
        if (acaoRecarregar != null)
        {
            acaoRecarregar.action.Disable();
        }
    }

    IEnumerator MostrarClarao()
    {
        // Mostra o efeito de luz na ponta da arma.
        claraoTiro.SetActive(true);

        // Mantém o clarão visível pelo tempo configurado.
        yield return new WaitForSeconds(duracaoClarao);

        // Esconde o efeito depois do disparo.
        claraoTiro.SetActive(false);

        // Libera a referência da corrotina.
        rotinaClarao = null;
    }
}
