
using UnityEngine;

public class IniciarDialogo : MonoBehaviour
{
    [Header("Diálogo")]

    // Sistema responsável por mostrar as falas na tela.
    public SistemaDialogo sistemaDialogo;

    // Diálogo que será apresentado quando a cena começar.
    public DialogoData dialogo;

    void Start()
    {
        // Inicia automaticamente o diálogo configurado no Inspector.
        sistemaDialogo.IniciarDialogo(dialogo);
    }
}
