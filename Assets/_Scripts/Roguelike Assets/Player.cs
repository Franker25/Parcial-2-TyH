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
    [SerializeField]
    private GameObject shieldVisual;
    [SerializeField]
    private float dashSpeed = 12f;
    [SerializeField]
    private float dashDuration = 0.15f;
    [SerializeField]
    private float dashCooldown = 1.5f;
    [SerializeField]
    private AudioClip shieldHitSound;

    public UnityEvent OnPlayerDie;

    private Vector2 input;
    private int shields = 0;
    private bool canDash = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    void Start()
    {
        UpdateShieldVisual();
    }

    private void FixedUpdate()
    {
        if (dashTimer > 0f)
        {
            rb2d.linearVelocity = dashDirection * dashSpeed;
        }
        else
        {
            rb2d.linearVelocity = input * speed;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw(horizontalAxis);
        float verticalInput = Input.GetAxisRaw(verticalAxis);
        input = new Vector2(horizontalInput, verticalInput);
        input.Normalize();

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (canDash && Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && input != Vector2.zero)
        {
            dashDirection = input;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (shields > 0 && enemy != null)
        {
            shields--;
            UpdateShieldVisual();

            if (shieldHitSound != null)
            {
                AudioSource.PlayClipAtPoint(shieldHitSound, Camera.main.transform.position);
            }

            enemy.Die();
            return;
        }

        if (OnPlayerDie != null)
        {
            OnPlayerDie.Invoke();
        }
        Destroy(gameObject);
    }

    private void UpdateShieldVisual()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(shields > 0);
        }
    }

    public void AddSpeed(float amount)
    {
        speed += amount;
    }

    public void AddShield()
    {
        shields++;
        UpdateShieldVisual();
    }

    public void EnableDash()
    {
        canDash = true;
    }
}
