
using UnityEngine;
using System.Collections;

public class HelicopteroFase2 : MonoBehaviour
{
    [Header("Movimento")]
    public Transform pontoParada;
    public float velocidadeEntrada = 4f;

    [Header("Sobrevoo")]
    public float distanciaSobrevoo = 0.15f;
    public float velocidadeSobrevoo = 2f;

    [Header("Ataque")]
    public Transform pontoBomba;
    public GameObject prefabBomba;
    public Transform jogador;
    public GameObject marcadorPerigo;

    public float tempoAntesDoAlvo = 1f;
    public float tempoMarcando = 0.8f;
    public float tempoAntesAtaque = 0.5f;
    public float intervaloAtaques = 2f;

    private bool chegou = false;
    private Vector3 posicaoSobrevoo;
    private float posicaoYMarcador;

    void Awake()
    {
        posicaoYMarcador = marcadorPerigo.transform.position.y;
        marcadorPerigo.SetActive(false);
    }

    void OnEnable()
    {
        chegou = false;
    }

    void OnDisable()
    {
        StopAllCoroutines();

        if (marcadorPerigo != null)
        {
            marcadorPerigo.SetActive(false);
        }
    }

    void Update()
    {
        if (!chegou)
        {
            EntrarNaTela();
            return;
        }

        Sobrevoar();
    }

    void EntrarNaTela()
    {
        transform.position = Vector3.MoveTowards(transform.position, pontoParada.position, velocidadeEntrada * Time.deltaTime);

        if (Vector3.Distance(transform.position, pontoParada.position) < 0.02f)
        {
            transform.position = pontoParada.position;
            posicaoSobrevoo = transform.position;
            chegou = true;

            StartCoroutine(AtacarContinuamente());
        }
    }

    void Sobrevoar()
    {
        float movimentoY = Mathf.Sin(Time.time * velocidadeSobrevoo) * distanciaSobrevoo;

        transform.position = new Vector3(posicaoSobrevoo.x, posicaoSobrevoo.y + movimentoY, posicaoSobrevoo.z);
    }

    IEnumerator AtacarContinuamente()
    {
        while (true)
        {
            yield return StartCoroutine(PrepararAtaque());
            yield return new WaitForSeconds(intervaloAtaques);
        }
    }

    IEnumerator PrepararAtaque()
    {
        yield return new WaitForSeconds(tempoAntesDoAlvo);

        marcadorPerigo.SetActive(true);

        float tempo = 0f;

        // O marcador acompanha Maxwell antes de travar a mira.
        while (tempo < tempoMarcando)
        {
            tempo += Time.deltaTime;

            marcadorPerigo.transform.position = new Vector3(jogador.position.x, posicaoYMarcador, marcadorPerigo.transform.position.z);

            yield return null;
        }

        // O alvo para de acompanhar Maxwell.
        yield return new WaitForSeconds(tempoAntesAtaque);

        SoltarBomba();
    }

    void SoltarBomba()
    {
        Vector3 destino = marcadorPerigo.transform.position;

        GameObject bomba = Instantiate(prefabBomba, pontoBomba.position, Quaternion.identity);

        BombaFase2 scriptBomba = bomba.GetComponent<BombaFase2>();

        if (scriptBomba != null)
        {
            scriptBomba.ConfigurarDestino(destino);
        }

        marcadorPerigo.SetActive(false);
    }
}
