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

    private bool chegou = false;
    private bool ataqueIniciado = false;

    private Vector3 posicaoSobrevoo;
    private float posicaoYMarcador;

    void Start()
    {
        marcadorPerigo.SetActive(false);

        // Guarda a altura do marcador no chão.
        posicaoYMarcador = marcadorPerigo.transform.position.y;
    }

    void Update()
    {
        if (!chegou)
        {
            EntrarNaTela();
            return;
        }

        Sobrevoar();

        if (!ataqueIniciado)
        {
            ataqueIniciado = true;
            StartCoroutine(PrepararAtaque());
        }
    }

    void EntrarNaTela()
    {
        transform.position = Vector3.MoveTowards(transform.position, pontoParada.position, velocidadeEntrada * Time.deltaTime);

        if (Vector3.Distance(transform.position, pontoParada.position) < 0.02f)
        {
            transform.position = pontoParada.position;

            chegou = true;
            posicaoSobrevoo = transform.position;
        }
    }

    void Sobrevoar()
    {
        float movimentoY = Mathf.Sin(Time.time * velocidadeSobrevoo) * distanciaSobrevoo;

        transform.position = new Vector3(posicaoSobrevoo.x, posicaoSobrevoo.y + movimentoY, posicaoSobrevoo.z);
    }

    IEnumerator PrepararAtaque()
    {
        // Dá um pequeno tempo depois que o helicóptero chega.
        yield return new WaitForSeconds(tempoAntesDoAlvo);

        marcadorPerigo.SetActive(true);

        float tempo = 0f;

        // Durante um tempo, o marcador acompanha a posição horizontal de Maxwell.
        while (tempo < tempoMarcando)
        {
            tempo += Time.deltaTime;

            marcadorPerigo.transform.position = new Vector3(jogador.position.x, posicaoYMarcador, marcadorPerigo.transform.position.z);

            yield return null;
        }

        // Agora o alvo fica travado no lugar.
        yield return new WaitForSeconds(tempoAntesAtaque);

        SoltarBomba();
    }

    void SoltarBomba()
    {
       Vector3 destino = marcadorPerigo.transform.position;

       GameObject bomba = Instantiate(prefabBomba, pontoBomba.position, Quaternion.identity);

       BombaFase2 bombaFase2 = bomba.GetComponent<BombaFase2>();

       if (bombaFase2 != null)
       {
           bombaFase2.ConfigurarDestino(destino);
       }

       marcadorPerigo.SetActive(false);
   }
}