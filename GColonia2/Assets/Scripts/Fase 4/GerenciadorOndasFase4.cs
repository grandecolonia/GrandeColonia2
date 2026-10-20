
using System.Collections;
using UnityEngine;
using TMPro;

public class GerenciadorOndasFase4 : MonoBehaviour
{
    [Header("Referências")]
    public SpawnAlvosFase4 gerenciadorSpawn;
    public MelhoriasFase4 gerenciadorMelhorias;
    public TMP_Text textoOnda;

    [Header("Onda 1")]
    public GameObject[] inimigosOnda1;
    public int quantidadeAlvosOnda1 = 8;

    [Header("Onda 2")]
    public GameObject[] inimigosOnda2;
    public int quantidadeAlvosOnda2 = 10;

    [Header("Onda 3")]
    public GameObject[] inimigosOnda3;
    public int quantidadeAlvosOnda3 = 12;

    [Header("General")]
    public GeneralFase4 prefabGeneral;
    public Transform pontoGeneral;

    [Header("Ondas Aceleradas")]
    public GameObject[] inimigosAcelerados;
    public int derrotasPorTipo = 3;
    public float tempoAlvoAcelerado = 7f;
    public float intervaloAcelerado = 0.5f;

    [Header("Transições")]
    public float tempoAvisoOnda = 2f;
    public float intervaloEntreOndas = 3f;
    public bool iniciarAutomaticamente = true;

    public FinalizacaoFase4 finalizacao;

    public BarraVidaGeneralFase4 barraVidaGeneral;

    void Start()
    {
        if (textoOnda != null)
            textoOnda.gameObject.SetActive(false);

        if (iniciarAutomaticamente)
            StartCoroutine(ExecutarOndas());
    }

    IEnumerator ExecutarOndas()
    {
        if (gerenciadorSpawn == null || gerenciadorMelhorias == null || prefabGeneral == null || pontoGeneral == null)
        {
            Debug.LogError("Configure as referências das ondas!");
            yield break;
        }

        // Onda 1
        yield return StartCoroutine(ExecutarOnda("ONDA 1", inimigosOnda1, quantidadeAlvosOnda1));

        yield return new WaitForSeconds(intervaloEntreOndas);
        yield return StartCoroutine(gerenciadorMelhorias.AbrirMelhorias());

        // Onda 2
        yield return StartCoroutine(ExecutarOnda("ONDA 2", inimigosOnda2, quantidadeAlvosOnda2));

        yield return new WaitForSeconds(intervaloEntreOndas);
        yield return StartCoroutine(gerenciadorMelhorias.AbrirMelhorias());

        // Onda 3
        yield return StartCoroutine(ExecutarOnda("ONDA 3", inimigosOnda3, quantidadeAlvosOnda3));

        yield return new WaitForSeconds(intervaloEntreOndas);

        // General
        yield return StartCoroutine(ExecutarGeneral());

        finalizacao.Vencer();
    }

    IEnumerator ExecutarOnda(string nomeOnda, GameObject[] inimigos, int quantidade)
    {
        if (inimigos == null || inimigos.Length == 0)
        {
            Debug.LogError("Configure os inimigos de " + nomeOnda);
            yield break;
        }

        if (textoOnda != null)
        {
            textoOnda.text = nomeOnda;
            textoOnda.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(tempoAvisoOnda);

        if (textoOnda != null)
            textoOnda.gameObject.SetActive(false);

        Debug.Log(nomeOnda + " iniciada!");

        yield return StartCoroutine(gerenciadorSpawn.GerarSequencia(inimigos, quantidade));

        Debug.Log(nomeOnda + " finalizada!");
    }

    IEnumerator ExecutarGeneral()
    {
        if (textoOnda != null)
        {
            textoOnda.text = "GENERAL";
            textoOnda.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(tempoAvisoOnda);

        if (textoOnda != null)
            textoOnda.gameObject.SetActive(false);

        GeneralFase4 general = Instantiate(prefabGeneral, pontoGeneral.position, Quaternion.identity, gerenciadorSpawn.transform);

        if (barraVidaGeneral != null)
    barraVidaGeneral.DefinirGeneral(general);

        while (general != null)
        {
            if (general.EstaEscondido())
            {
                yield return StartCoroutine(ExecutarOndaAcelerada());

                if (general != null)
                    general.Voltar();
            }

            yield return null;
        }

        Debug.Log("General eliminado!");
    }

    IEnumerator ExecutarOndaAcelerada()
    {
        if (inimigosAcelerados == null || inimigosAcelerados.Length == 0)
        {
            Debug.LogError("Configure os inimigos acelerados!");
            yield break;
        }

        Debug.Log("ONDA ACELERADA!");

        for (int i = 0; i < inimigosAcelerados.Length; i++)
        {
            int derrotas = 0;

            while (derrotas < derrotasPorTipo)
            {
                GameObject alvo = gerenciadorSpawn.CriarAlvo(inimigosAcelerados[i]);

                if (alvo == null)
                    yield break;

                VidaInimigoFase4 vida = alvo.GetComponent<VidaInimigoFase4>();

                if (vida == null)
                {
                    Debug.LogError("Inimigo acelerado sem script de vida!");
                    Destroy(alvo);
                    yield break;
                }

                bool morreu = false;
                vida.AoMorrer += () => morreu = true;

                float tempoRestante = tempoAlvoAcelerado;

                // Espera o inimigo morrer ou escapar
                while (alvo != null && tempoRestante > 0f)
                {
                    tempoRestante -= Time.deltaTime;
                    yield return null;
                }

                if (alvo != null)
{
    Destroy(alvo);

    if (finalizacao != null)
        finalizacao.RegistrarFuga();
}

                // Só conta se o jogador derrotou
                if (morreu)
                    derrotas++;

                yield return new WaitForSeconds(intervaloAcelerado);
            }
        }

        Debug.Log("Onda acelerada concluída!");
    }

    public void PararOndas()
{
    StopAllCoroutines();
}
}
