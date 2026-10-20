
using UnityEngine;

public class AudioFase4 : MonoBehaviour
{
    public static AudioFase4 instancia;

    [Header("Sons")]
    public AudioClip somDisparo, somTiroCarregado, somRecarga, somImpacto, somAliado, somMelhoria, somVitoria, somDerrota;

    private AudioSource audioSource;

    void Awake()
    {
        instancia = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Tocar(AudioClip som)
    {
        if (som != null)
            audioSource.PlayOneShot(som);
    }

    public void TocarDisparo(bool carregado)
    {
        if (carregado && somTiroCarregado != null)
            Tocar(somTiroCarregado);
        else
            Tocar(somDisparo);
    }

    public void TocarRecarga()
    {
        Tocar(somRecarga);
    }

    public void TocarImpacto()
    {
        Tocar(somImpacto);
    }

    public void TocarAliado()
    {
        Tocar(somAliado);
    }

    public void TocarMelhoria()
    {
        Tocar(somMelhoria);
    }

    public void TocarFinalizacao(bool venceu)
    {
        if (venceu)
            Tocar(somVitoria);
        else
            Tocar(somDerrota);
    }
}
