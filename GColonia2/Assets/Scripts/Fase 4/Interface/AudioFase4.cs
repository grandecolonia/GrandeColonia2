
using UnityEngine;

public class AudioFase4 : MonoBehaviour
{
    // Permite que outros scripts acessem o sistema de áudio da Fase 4.
    public static AudioFase4 instancia;

    [Header("Disparos")]

    // Som reproduzido quando Maxwell realiza um disparo normal.
    public AudioClip somDisparo;

    // Som reproduzido quando Maxwell utiliza o tiro carregado.
    public AudioClip somTiroCarregado;

    // Som reproduzido quando Maxwell recarrega a arma.
    public AudioClip somRecarga;

    [Header("Impactos")]

    // Som reproduzido quando um inimigo ou o General recebe um tiro.
    public AudioClip somImpacto;

    // Som reproduzido quando Maxwell acerta um rebelde aliado.
    public AudioClip somAliado;

    [Header("Melhorias")]

    // Som reproduzido quando o jogador escolhe um upgrade.
    public AudioClip somMelhoria;

    [Header("Finalização")]

    // Som reproduzido quando Maxwell vence a fase.
    public AudioClip somVitoria;

    // Som reproduzido quando Maxwell perde a fase.
    public AudioClip somDerrota;

    // Componente responsável por reproduzir os efeitos sonoros.
    private AudioSource audioSource;

    void Awake()
    {
        // Guarda a referência deste sistema para os outros scripts utilizarem.
        instancia = this;

        // Procura o AudioSource no próprio GameObject.
        audioSource = GetComponent<AudioSource>();
    }

    void Tocar(AudioClip som)
    {
        // Se o som foi configurado, reproduz sem interromper os outros efeitos.
        if (som != null)
        {
            audioSource.PlayOneShot(som);
        }
    }

    public void TocarDisparo(bool carregado)
    {
        // Se o tiro está carregado e possui um som próprio, reproduz esse efeito.
        if (carregado && somTiroCarregado != null)
        {
            Tocar(somTiroCarregado);
        }
        else
        {
            // Caso contrário, reproduz o som do disparo normal.
            Tocar(somDisparo);
        }
    }

    public void TocarRecarga()
    {
        // Reproduz o som de recarga da arma.
        Tocar(somRecarga);
    }

    public void TocarImpacto()
    {
        // Reproduz o som quando um inimigo recebe um disparo.
        Tocar(somImpacto);
    }

    public void TocarAliado()
    {
        // Reproduz o som quando Maxwell acerta um aliado.
        Tocar(somAliado);
    }

    public void TocarMelhoria()
    {
        // Reproduz o som quando o jogador escolhe um upgrade.
        Tocar(somMelhoria);
    }

    public void TocarFinalizacao(bool venceu)
    {
        // Se Maxwell venceu, reproduz o som de vitória.
        if (venceu)
        {
            Tocar(somVitoria);
        }
        else
        {
            // Se Maxwell perdeu, reproduz o som de derrota.
            Tocar(somDerrota);
        }
    }
}
