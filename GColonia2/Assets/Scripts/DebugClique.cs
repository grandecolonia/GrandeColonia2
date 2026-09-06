using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DebugClique : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData dados = new PointerEventData(EventSystem.current);
            dados.position = Mouse.current.position.ReadValue();

            List<RaycastResult> resultados = new List<RaycastResult>();
            EventSystem.current.RaycastAll(dados, resultados);

            foreach (RaycastResult resultado in resultados)
            {
                Debug.Log("UI encontrada no clique: " + resultado.gameObject.name);
            }
        }
    }
}