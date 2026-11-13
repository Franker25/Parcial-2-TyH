using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    private Rigidbody2D rb2d;
    private Transform playerTransform;
    private GameManager gameManager;
    private bool dead = false;

    public bool Stopped = false;

    [SerializeField]
    private GameObject crabDeath;

    public event Action OnDie;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        rb2d = GetComponent<Rigidbody2D>();
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Stopped = true;
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Stopped || playerTransform == null)
        {
            rb2d.linearVelocity = Vector3.zero;
            return;
        }

        float currentSpeed = speed;
        if (gameManager != null)
        {
            currentSpeed = Mathf.Max(0.5f, speed + gameManager.EnemySpeedBonus);
        }

        Vector3 directionToPLayer = playerTransform.position - transform.position;
        rb2d.linearVelocity = directionToPLayer.normalized * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (dead)
        {
            return;
        }
        dead = true;

        Instantiate(crabDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (OnDie != null)
        {
            OnDie();
        }
    }
}
