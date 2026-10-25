
using UnityEngine;
using System.Collections;

public class HelicopteroFase2 : MonoBehaviour
{
    [Header("Movimento")]

    // Ponto onde o helicóptero deve parar depois de entrar na tela.
    public Transform pontoParada;

    // Velocidade com que o helicóptero chega até o ponto de parada.
    public float velocidadeEntrada = 4f;

    [Header("Sobrevoo")]

    // Distância que o helicóptero sobe e desce enquanto está parado no ar.
    public float distanciaSobrevoo = 0.15f;

    // Controla a velocidade desse movimento de subida e descida.
    public float velocidadeSobrevoo = 2f;

    [Header("Ataque")]

    // Ponto de onde as bombas serão lançadas.
    public Transform pontoBomba;

    // Prefab da bomba que o helicóptero vai criar durante os ataques.
    public GameObject prefabBomba;

    // Referência ao Maxwell para o helicóptero conseguir mirar nele.
    public Transform jogador;

    // Marcador que aparece no chão para avisar onde a bomba vai cair.
    public GameObject marcadorPerigo;

    // Tempo que o helicóptero espera antes de começar a mirar.
    public float tempoAntesDoAlvo = 1f;

    // Tempo em que o marcador acompanha a posição do Maxwell.
    public float tempoMarcando = 0.8f;

    // Tempo de espera depois que a mira trava e antes de lançar a bomba.
    public float tempoAntesAtaque = 0.5f;

    // Intervalo entre os ataques, permitindo que os guardas voltem a aparecer.
    public float intervaloAtaques = 5f;

    [Header("Segurança")]

    // Sistema responsável por criar os guardas durante a fase.
    public SpawnGuardaFase2 spawnGuardas;

    // Pequena espera depois que os guardas saem, antes de começar o bombardeio.
    public float intervaloSeguranca = 0.5f;

    // Marca quando o helicóptero já chegou ao ponto de parada.
    private bool chegou = false;

    // Guarda a posição onde o helicóptero vai ficar sobrevoando.
    private Vector3 posicaoSobrevoo;

    // Guarda a altura original do marcador para ele não subir junto com Maxwell.
    private float posicaoYMarcador;

    void Awake()
    {
        // Salva a altura do marcador antes de começar os ataques.
        posicaoYMarcador = marcadorPerigo.transform.position.y;

        // Começa com o marcador de perigo escondido.
        marcadorPerigo.SetActive(false);
    }

    void OnEnable()
    {
        // Marca que o helicóptero ainda precisa chegar ao ponto de parada.
        chegou = false;
    }

    void OnDisable()
    {
        // Interrompe todos os ataques caso o helicóptero seja desativado.
        StopAllCoroutines();

        // Esconde o marcador para não deixar um aviso perdido no cenário.
        if (marcadorPerigo != null)
        {
            marcadorPerigo.SetActive(false);
        }

        // Libera o sistema de guardas caso ele tenha sido pausado pelo helicóptero.
        if (spawnGuardas != null)
        {
            spawnGuardas.pausarSpawn = false;
        }
    }

    void Update()
    {
        // Enquanto o helicóptero não chegou ao destino, continua entrando na tela.
        if (!chegou)
        {
            EntrarNaTela();
            return;
        }

        // Depois que chegou, fica subindo e descendo suavemente.
        Sobrevoar();
    }

    void EntrarNaTela()
    {
        // Move o helicóptero até o ponto de parada sem ultrapassar o destino.
        transform.position = Vector3.MoveTowards(transform.position, pontoParada.position, velocidadeEntrada * Time.deltaTime);

        // Se chegou perto o suficiente, termina a entrada e prepara os ataques.
        if (Vector3.Distance(transform.position, pontoParada.position) < 0.02f)
        {
            // Coloca o helicóptero exatamente no ponto configurado.
            transform.position = pontoParada.position;

            // Guarda essa posição como referência para o movimento de sobrevoo.
            posicaoSobrevoo = transform.position;

            // Marca que a entrada terminou.
            chegou = true;

            // Começa a sequência contínua de ataques.
            StartCoroutine(AtacarContinuamente());
        }
    }

    void Sobrevoar()
    {
        // Usa o seno para fazer o helicóptero subir e descer suavemente.
        float movimentoY = Mathf.Sin(Time.time * velocidadeSobrevoo) * distanciaSobrevoo;

        // Aplica o movimento vertical sem deixar o helicóptero sair do lugar.
        transform.position = new Vector3(posicaoSobrevoo.x, posicaoSobrevoo.y + movimentoY, posicaoSobrevoo.z);
    }

    IEnumerator AtacarContinuamente()
    {
        // Mantém os ataques acontecendo enquanto o helicóptero estiver ativo.
        while (true)
        {
            // Impede que novos guardas apareçam enquanto prepara o bombardeio.
            if (spawnGuardas != null)
            {
                spawnGuardas.pausarSpawn = true;
            }

            // Espera todos os guardas existentes saírem do cenário.
            while (FindObjectsByType<GuardaFase2>(FindObjectsSortMode.None).Length > 0)
            {
                yield return null;
            }

            // Dá uma pequena pausa para garantir que o caminho esteja livre.
            yield return new WaitForSeconds(intervaloSeguranca);

            // Começa a marcar a posição do Maxwell e prepara o lançamento.
            yield return StartCoroutine(PrepararAtaque());

            // Espera todas as bombas desaparecerem antes de liberar novos guardas.
            while (FindObjectsByType<BombaFase2>(FindObjectsSortMode.None).Length > 0)
            {
                yield return null;
            }

            // Permite que os guardas voltem a aparecer depois do ataque.
            if (spawnGuardas != null)
            {
                spawnGuardas.pausarSpawn = false;
            }

            // Espera um intervalo antes de preparar o próximo bombardeio.
            yield return new WaitForSeconds(intervaloAtaques);
        }
    }

    IEnumerator PrepararAtaque()
    {
        // Espera um pouco antes de começar a marcar onde a bomba vai cair.
        yield return new WaitForSeconds(tempoAntesDoAlvo);

        // Mostra o marcador de perigo para avisar que um ataque está chegando.
        marcadorPerigo.SetActive(true);

        // Começa a contar o tempo em que o marcador vai acompanhar Maxwell.
        float tempo = 0f;

        // Mantém o marcador seguindo Maxwell enquanto a mira ainda não travou.
        while (tempo < tempoMarcando)
        {
            // Conta o tempo de marcação a cada frame.
            tempo += Time.deltaTime;

            // Acompanha Maxwell na horizontal, sem mudar a altura do marcador.
            marcadorPerigo.transform.position = new Vector3(jogador.position.x, posicaoYMarcador, marcadorPerigo.transform.position.z);

            // Espera o próximo frame para atualizar a mira.
            yield return null;
        }

        // A mira fica parada e o jogador ainda tem um tempo para escapar.
        yield return new WaitForSeconds(tempoAntesAtaque);

        // Terminou a preparação, então lança a bomba no local marcado.
        SoltarBomba();
    }

    void SoltarBomba()
    {
        // Guarda a posição marcada como destino da bomba.
        Vector3 destino = marcadorPerigo.transform.position;

        // Cria uma nova bomba na ponta do helicóptero.
        GameObject bomba = Instantiate(prefabBomba, pontoBomba.position, Quaternion.identity);

        // Procura o script que controla a queda e a explosão da bomba criada.
        BombaFase2 scriptBomba = bomba.GetComponent<BombaFase2>();

        // Se encontrou o script, informa onde a bomba deve cair.
        if (scriptBomba != null)
        {
            scriptBomba.ConfigurarDestino(destino);
        }

        // Esconde o marcador depois que a bomba foi lançada.
        marcadorPerigo.SetActive(false);
    }
}
