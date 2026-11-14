using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleEnemy : MonoBehaviour
{
    public string EnemyName = "Enemy";
    public bool IsDead = false;
    public bool WeaknessHitThisTurn = false;

    [SerializeField]
    private int maxHealth = 30;
    [SerializeField]
    private int attackDamage = 4;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Image enemyImage;
    [SerializeField]
    private BattleManager manager;
    [SerializeField]
    private int blinkCount = 3;
    [SerializeField]
    private float blinkTime = 0.08f;

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

    public bool TakeHit(ElementData element)
    {
        bool hitWeakness = element.type == weakness;

        if (hitWeakness)
        {
            weaknessKnown = true;
            WeaknessHitThisTurn = true;
        }

        health -= element.GetDamage(hitWeakness);

        if (health <= 0)
        {
            health = 0;
            IsDead = true;
            selectButton.interactable = false;
            enemyImage.color = Color.gray;
        }

        Blink();
        UpdateText();
        return hitWeakness;
    }

    private void Blink()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            enemyImage.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            enemyImage.enabled = true;
            yield return new WaitForSeconds(blinkTime);
        }
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
