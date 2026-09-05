using UnityEngine;
using UnityEngine.InputSystem;

public class Teste : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movimento;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movimento = context.ReadValue<Vector2>();
    }

    public void Pular(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector2.up * 100);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector2.right * movimento.x * 100);
    }
}