using UnityEngine;

public class ControleLabirinto : MonoBehaviour
{
    [Header("Itens")]

    // Guarda quais itens o jogador já conseguiu.
    public bool temCano = false;
    public bool temAlicate = false;
    public bool temCartao = false;

    [Header("Labirinto")]

    // Diz se a energia da sala já foi ligada.
    public bool energiaLigada = false;

    [Header("Perseguição")]

    // Ativa a perseguição depois que o alerta começa.
    public bool alertaAtivo = false;
}