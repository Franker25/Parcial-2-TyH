using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHero : MonoBehaviour
{
    public bool IsDead = false;

    [SerializeField]
    private int maxHealth = 60;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Image heroImage;
    [SerializeField]
    private Sprite werewolfSprite;
    [SerializeField]
    private float werewolfScale = 1.5f;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private int blinkCount = 3;
    [SerializeField]
    private float blinkTime = 0.08f;

    private int health;
    private Sprite normalSprite;
    private Vector3 normalScale;

    void Start()
    {
        health = maxHealth;
        normalSprite = heroImage.sprite;
        normalScale = heroImage.rectTransform.localScale;
        UpdateText();
    }

    public void SetWerewolf(bool isWerewolf)
    {
        if (werewolfSprite == null)
        {
            return;
        }

        if (isWerewolf)
        {
            heroImage.sprite = werewolfSprite;
            heroImage.rectTransform.localScale = normalScale * werewolfScale;
        }
        else
        {
            heroImage.sprite = normalSprite;
            heroImage.rectTransform.localScale = normalScale;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            IsDead = true;
        }

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        Blink();
        UpdateText();
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
            heroImage.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            heroImage.enabled = true;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    private void UpdateText()
    {
        healthText.text = "Hero HP: " + health + " / " + maxHealth;
    }
}
