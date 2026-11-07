using UnityEngine.Events;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string horizontalAxis = "Horizontal", verticalAxis = "Vertical";
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float speed = 3f;

    public UnityEvent OnPlayerDie;

    private Vector2 input;

    private void FixedUpdate()
    {
        rb2d.linearVelocity = input * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(horizontalAxis); // Izquierda a Derecha
        float verticalInput = Input.GetAxisRaw(verticalAxis); // Abajo y Arriba
        input = new Vector2(horizontalInput, verticalInput);
        input.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnPlayerDie != null)
        {
            OnPlayerDie.Invoke();
        }
        Destroy(gameObject);
    }

    public void AddSpeed(float amount)
    {
        speed += amount;
    }
}
