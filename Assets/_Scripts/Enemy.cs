using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    private Rigidbody2D rb2d;
    private Transform playerTransform;
    
    public bool Stopped = false; // Es publico porque otros scripts tienen que manipularlo

    [SerializeField]
    private GameObject crabDeath;

    public event Action OnDie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager != null)
        {
            speed += gameManager.EnemySpeedBonus;
        }

        rb2d = GetComponent<Rigidbody2D>();
        Player player = FindAnyObjectByType<Player>(); //Tenemos el transform del Player
        if (player != null ) // Si est� 
        {
            playerTransform = player.transform;
        }
        else
        {
            Stopped = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Stopped || playerTransform == null) // las barras verticales significan OR, entonces si esta parado el enemigo o si el personaje no est� (por eso el Null)
        {
            rb2d.linearVelocity = Vector3.zero; //Paramos todo
            return; 
        }
        Vector3 directionToPLayer = playerTransform.position - transform.position; //Buscamos ir al player 
            rb2d.linearVelocity = directionToPLayer.normalized * speed; //Marcamos la velocidad
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.CompareTag("Weapon"))  //Tageamos al arma para detectarla
        {
            Instantiate(crabDeath, transform.position, Quaternion.identity); //Ponemos al Cangrejo Muerto
            Destroy(gameObject); //Se muere
            if (OnDie != null) //Llama al Evento de On Die
            {
                OnDie();
            }
        }
    }
}
