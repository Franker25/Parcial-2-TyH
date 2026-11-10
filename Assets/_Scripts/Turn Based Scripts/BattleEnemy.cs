using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleEnemy : MonoBehaviour
{
    public string EnemyName = "Enemy";
    public bool IsDead = false;

    [SerializeField]
    private int maxHealth = 30;
    [SerializeField]
    private int attackDamage = 4;
    [SerializeField]
    private int weaknessDamage = 15;
    [SerializeField]
    private int normalDamage = 2;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Image enemyImage;
    [SerializeField]
    private BattleManager manager;

    private int health;
    private bool weaknessKnown = false;
    private ElementType weakness;

    void Start()
    {
        health = maxHealth;
        UpdateText();
    }

    public void SetWeakness(ElementType newWeakness)
    {
        weakness = newWeakness;
    }

    public void OnClicked()
    {
        manager.SelectEnemy(this);
    }

    public bool TakeHit(ElementType element)
    {
        bool hitWeakness = false;

        if (element == weakness)
        {
            hitWeakness = true;
            weaknessKnown = true;
            health -= weaknessDamage;
        }
        else
        {
            health -= normalDamage;
        }

        if (health <= 0)
        {
            health = 0;
            IsDead = true;
            selectButton.interactable = false;
            enemyImage.color = Color.gray;
        }

        UpdateText();
        return hitWeakness;
    }

    public int GetAttackDamage()
    {
        return attackDamage;
    }

    public void SetSelected(bool selected)
    {
        if (IsDead)
        {
            return;
        }

        if (selected)
        {
            enemyImage.color = Color.yellow;
        }
        else
        {
            enemyImage.color = Color.white;
        }
    }

    private void UpdateText()
    {
        string weaknessText = "???";

        if (weaknessKnown)
        {
            weaknessText = weakness.ToString();
        }

        infoText.text = EnemyName + "\nHP: " + health + " / " + maxHealth + "\nWeak: " + weaknessText;
    }
}
