using UnityEngine;
using TMPro;

public class BattleHero : MonoBehaviour
{
    public bool IsDead = false;

    [SerializeField]
    private int maxHealth = 60;
    [SerializeField]
    private TextMeshProUGUI healthText;

    private int health;

    void Start()
    {
        health = maxHealth;
        UpdateText();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            IsDead = true;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        healthText.text = "Hero HP: " + health + " / " + maxHealth;
    }
}
