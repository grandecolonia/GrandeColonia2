using UnityEngine;

public class IniciarDialogo : MonoBehaviour
{
    public SistemaDialogo sistemaDialogo;
    public DialogoData dialogo;

    void Start()
    {
        sistemaDialogo.IniciarDialogo(dialogo);
    }
}