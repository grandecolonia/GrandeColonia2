using UnityEngine;

public class ControleLabirinto : MonoBehaviour
{
    [Header("Itens")]

    public bool temCano = false;
    public bool temAlicate = false;
    public bool temCartao = false;

    [Header("Labirinto")]

    public bool energiaLigada = false;

    [Header("Perseguição")]

    public bool alertaAtivo = false;
}