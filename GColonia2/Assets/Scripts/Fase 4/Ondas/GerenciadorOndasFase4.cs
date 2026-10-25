
using System.Collections;
using UnityEngine;
using TMPro;

public class GerenciadorOndasFase4 : MonoBehaviour
{
    [Header("Referências")]

    // Sistema responsável por criar os alvos durante as ondas.
    public SpawnAlvosFase4 gerenciadorSpawn;

    // Sistema que mostra as melhorias entre as ondas.
    public MelhoriasFase4 gerenciadorMelhorias;

    // Texto que mostra o nome da onda antes de começar.
    public TMP_Text textoOnda;

    [Header("Onda 1")]

    // Prefabs dos inimigos que podem aparecer na primeira onda.
    public GameObject[] inimigosOnda1;

    // Quantidade de alvos que serão criados na primeira onda.
    public int quantidadeAlvosOnda1 = 8;

    [Header("Onda 2")]

    // Prefabs dos inimigos que podem aparecer na segunda onda.
    public GameObject[] inimigosOnda2;

    // Quantidade de alvos que serão criados na segunda onda.
    public int quantidadeAlvosOnda2 = 10;

    [Header("Onda 3")]

    // Prefabs dos inimigos que podem aparecer na terceira onda.
    public GameObject[] inimigosOnda3;

    // Quantidade de alvos que serão criados na terceira onda.
    public int quantidadeAlvosOnda3 = 12;

    [Header("General")]

    // Prefab do General que aparece depois das três ondas.
    public GeneralFase4 prefabGeneral;

    // Ponto onde o General será criado.
    public Transform pontoGeneral;

    [Header("Ondas Aceleradas")]

    // Prefabs utilizados quando o General se esconde.
    public GameObject[] inimigosAcelerados;

    // Quantidade de inimigos de cada tipo que Maxwell precisa derrotar.
    public int derrotasPorTipo = 3;

    // Tempo máximo que cada alvo acelerado permanece na tela.
    public float tempoAlvoAcelerado = 7f;

    // Intervalo entre os alvos das ondas aceleradas.
    public float intervaloAcelerado = 0.5f;

    [Header("Transições")]

    // Tempo que o aviso da onda permanece na tela.
    public float tempoAvisoOnda = 2f;

    // Tempo de espera entre as ondas principais.
    public float intervaloEntreOndas = 3f;

    // Permite iniciar as ondas automaticamente quando a cena começa.
    public bool iniciarAutomaticamente = true;

    [Header("Finalização")]

    // Sistema responsável por controlar a vitória e a derrota.
    public FinalizacaoFase4 finalizacao;

    [Header("Interface do General")]

    // Barra de vida que acompanhará o General durante o combate.
    public BarraVidaGeneralFase4 barraVidaGeneral;

    void Start()
    {
        // Começa com o aviso das ondas escondido.
        if (textoOnda != null)
        {
            textoOnda.gameObject.SetActive(false);
        }

        // Se o início automático está ativado, começa a sequência da fase.
        if (iniciarAutomaticamente)
        {
            StartCoroutine(ExecutarOndas());
        }
    }

    IEnumerator ExecutarOndas()
    {
        // Verifica se os sistemas principais e as referências do General existem.
        if (gerenciadorSpawn == null || gerenciadorMelhorias == null || prefabGeneral == null || pontoGeneral == null)
        {
            Debug.LogError("Configure as referências das ondas!");
            yield break;
        }

        // ONDA 1: cria a primeira sequência de inimigos.
        yield return StartCoroutine(ExecutarOnda("ONDA 1", inimigosOnda1, quantidadeAlvosOnda1));

        // Espera um pouco antes de mostrar as melhorias.
        yield return new WaitForSeconds(intervaloEntreOndas);

        // Abre o painel para Maxwell escolher uma melhoria.
        yield return StartCoroutine(gerenciadorMelhorias.AbrirMelhorias());

        // ONDA 2: cria a segunda sequência de inimigos.
        yield return StartCoroutine(ExecutarOnda("ONDA 2", inimigosOnda2, quantidadeAlvosOnda2));

        // Espera antes da próxima escolha de melhoria.
        yield return new WaitForSeconds(intervaloEntreOndas);

        // Abre novamente o painel de melhorias.
        yield return StartCoroutine(gerenciadorMelhorias.AbrirMelhorias());

        // ONDA 3: cria a última sequência de inimigos comuns.
        yield return StartCoroutine(ExecutarOnda("ONDA 3", inimigosOnda3, quantidadeAlvosOnda3));

        // Aguarda antes da entrada do General.
        yield return new WaitForSeconds(intervaloEntreOndas);

        // Começa o combate contra o General.
        yield return StartCoroutine(ExecutarGeneral());

        // Quando o General for derrotado, encerra a fase com vitória.
        finalizacao.Vencer();
    }

    IEnumerator ExecutarOnda(string nomeOnda, GameObject[] inimigos, int quantidade)
    {
        // Se a onda não possui inimigos configurados, cancela sua execução.
        if (inimigos == null || inimigos.Length == 0)
        {
            Debug.LogError("Configure os inimigos de " + nomeOnda);
            yield break;
        }

        // Mostra o nome da onda antes de começar.
        if (textoOnda != null)
        {
            textoOnda.text = nomeOnda;
            textoOnda.gameObject.SetActive(true);
        }

        // Mantém o aviso aparecendo pelo tempo configurado.
        yield return new WaitForSeconds(tempoAvisoOnda);

        // Esconde o aviso antes dos inimigos começarem a aparecer.
        if (textoOnda != null)
        {
            textoOnda.gameObject.SetActive(false);
        }

        // Pede ao sistema de spawn para criar os inimigos da onda.
        yield return StartCoroutine(gerenciadorSpawn.GerarSequencia(inimigos, quantidade));
    }

    IEnumerator ExecutarGeneral()
    {
        // Mostra o aviso de que o General está chegando.
        if (textoOnda != null)
        {
            textoOnda.text = "GENERAL";
            textoOnda.gameObject.SetActive(true);
        }

        // Espera o tempo do aviso antes de começar o combate.
        yield return new WaitForSeconds(tempoAvisoOnda);

        // Esconde o aviso para mostrar o combate.
        if (textoOnda != null)
        {
            textoOnda.gameObject.SetActive(false);
        }

        // Cria o General no ponto configurado, dentro do objeto de spawn.
        GeneralFase4 general = Instantiate(prefabGeneral, pontoGeneral.position, Quaternion.identity, gerenciadorSpawn.transform);

        // Entrega a referência do General para sua barra de vida.
        if (barraVidaGeneral != null)
        {
            barraVidaGeneral.DefinirGeneral(general);
        }

        // Mantém o combate acontecendo enquanto o General existir.
        while (general != null)
        {
            // Se o General se escondeu, começa uma onda acelerada.
            if (general.EstaEscondido())
            {
                // Maxwell precisa enfrentar os inimigos antes do General voltar.
                yield return StartCoroutine(ExecutarOndaAcelerada());

                // Se o General ainda existe, faz ele voltar ao combate.
                if (general != null)
                {
                    general.Voltar();
                }
            }

            // Espera o próximo frame para verificar o estado do General.
            yield return null;
        }
    }

    IEnumerator ExecutarOndaAcelerada()
    {
        // Se não existem inimigos acelerados configurados, cancela a sequência.
        if (inimigosAcelerados == null || inimigosAcelerados.Length == 0)
        {
            Debug.LogError("Configure os inimigos acelerados!");
            yield break;
        }

        // Percorre cada tipo de inimigo configurado para a onda acelerada.
        for (int i = 0; i < inimigosAcelerados.Length; i++)
        {
            // Guarda quantos inimigos desse tipo Maxwell já derrotou.
            int derrotas = 0;

            // Continua até Maxwell derrotar a quantidade necessária.
            while (derrotas < derrotasPorTipo)
            {
                // Cria um inimigo do tipo atual.
                GameObject alvo = gerenciadorSpawn.CriarAlvo(inimigosAcelerados[i]);

                // Se não conseguiu criar o alvo, interrompe a onda acelerada.
                if (alvo == null)
                {
                    yield break;
                }

                // Procura o componente que controla a vida do inimigo criado.
                VidaInimigoFase4 vida = alvo.GetComponent<VidaInimigoFase4>();

                // Se não encontrou o script de vida, remove o alvo e cancela a sequência.
                if (vida == null)
                {
                    Debug.LogError("Inimigo acelerado sem script de vida!");
                    Destroy(alvo);
                    yield break;
                }

                // Guarda se Maxwell conseguiu derrotar o inimigo.
                bool morreu = false;

                // Recebe o aviso de quando o inimigo for derrotado.
                vida.AoMorrer += () => morreu = true;

                // Começa a contar o tempo disponível para atingir o alvo.
                float tempoRestante = tempoAlvoAcelerado;

                // Espera até o inimigo morrer ou seu tempo na tela acabar.
                while (alvo != null && tempoRestante > 0f)
                {
                    // Diminui o tempo restante a cada frame.
                    tempoRestante -= Time.deltaTime;

                    // Espera o próximo frame para verificar novamente.
                    yield return null;
                }

                // Se o alvo continua na cena depois do tempo, considera que escapou.
                if (alvo != null)
                {
                    // Remove o alvo que não foi derrotado.
                    Destroy(alvo);

                    // Registra uma fuga no sistema de finalização.
                    if (finalizacao != null)
                    {
                        finalizacao.RegistrarFuga();
                    }
                }

                // Só conta uma derrota quando Maxwell realmente eliminou o inimigo.
                if (morreu)
                {
                    derrotas++;
                }

                // Espera um pouco antes de criar o próximo alvo.
                yield return new WaitForSeconds(intervaloAcelerado);
            }
        }
    }

    public void PararOndas()
    {
        // Interrompe todas as sequências quando a fase termina.
        StopAllCoroutines();
    }
}
