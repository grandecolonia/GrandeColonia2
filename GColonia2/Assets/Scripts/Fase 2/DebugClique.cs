using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DebugClique : MonoBehaviour
{
    void Update()
    {
        // Verifica se o botão esquerdo do mouse foi clicado.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData dados = new PointerEventData(EventSystem.current);
            dados.position = Mouse.current.position.ReadValue();

            // Guarda todos os elementos de UI encontrados no ponto do clique.
            List<RaycastResult> resultados = new List<RaycastResult>();

            EventSystem.current.RaycastAll(dados, resultados);

            // Mostra no Console quais elementos estavam recebendo o clique.
            foreach (RaycastResult resultado in resultados)
            {
                Debug.Log("UI encontrada no clique: " + resultado.gameObject.name);
            }
        }
    }
}