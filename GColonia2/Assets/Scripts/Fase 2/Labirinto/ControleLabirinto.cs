
using UnityEngine;

public class ControleLabirinto : MonoBehaviour
{
    [Header("Itens")]

    // Marca se Maxwell já conseguiu pegar o cano.
    public bool temCano = false;

    // Marca se Maxwell já encontrou o alicate.
    public bool temAlicate = false;

    // Guarda se Maxwell já pegou o cartão necessário para continuar.
    public bool temCartao = false;

    [Header("Labirinto")]

    // Informa se Maxwell já ligou a energia do labirinto.
    public bool energiaLigada = false;

    [Header("Perseguição")]

    // Marca se o alerta foi ativado e a perseguição pode começar.
    public bool alertaAtivo = false;
}
