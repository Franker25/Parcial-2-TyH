using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private BattleEnemy[] enemies;
    [SerializeField]
    private BattleHero hero;
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private GameObject elementButtons;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private float enemyTurnDelay = 1f;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip criticalSound;

    private BattleEnemy selectedEnemy;
    private bool playerTurn = true;
    private int elementCount = 4;

    void Start()
    {
        AssignRandomWeaknesses();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        messageText.text = "Pick an enemy and an element. Find the weakness!";
        SelectNextEnemy();
    }

    private void AssignRandomWeaknesses()
    {
        List<int> availableElements = new List<int>();

        for (int i = 0; i < elementCount; i++)
        {
            availableElements.Add(i);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            int randomPosition = Random.Range(0, availableElements.Count);
            int chosenElement = availableElements[randomPosition];
            enemies[i].SetWeakness((ElementType)chosenElement);
            availableElements.RemoveAt(randomPosition);
        }
    }

    public void SelectEnemy(BattleEnemy enemy)
    {
        if (!playerTurn)
        {
            return;
        }

        if (selectedEnemy != null)
        {
            selectedEnemy.SetSelected(false);
        }

        selectedEnemy = enemy;
        selectedEnemy.SetSelected(true);
    }

    public void UseElement(int elementIndex)
    {
        if (!playerTurn || selectedEnemy == null)
        {
            return;
        }

        ElementType element = (ElementType)elementIndex;
        BattleEnemy target = selectedEnemy;
        bool hitWeakness = target.TakeHit(element);

        if (hitWeakness)
        {
            PlaySound(criticalSound);
        }
        else
        {
            PlaySound(hitSound);
        }

        if (AllEnemiesDead())
        {
            Win();
            return;
        }

        if (target.IsDead)
        {
            SelectNextEnemy();
        }

        if (hitWeakness)
        {
            messageText.text = "WEAKNESS! " + target.EnemyName + " is weak to " + element + "! One more turn!";
        }
        else
        {
            messageText.text = element + " did little to " + target.EnemyName + "...";
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        playerTurn = false;
        elementButtons.SetActive(false);

        yield return new WaitForSeconds(enemyTurnDelay);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead)
            {
                messageText.text = enemies[i].EnemyName + " attacks!";
                yield return new WaitForSeconds(enemyTurnDelay);
                hero.TakeDamage(enemies[i].GetAttackDamage());

                if (hero.IsDead)
                {
                    GameOver();
                    yield break;
                }
            }
        }

        messageText.text = "Your turn! Pick an element.";
        elementButtons.SetActive(true);
        playerTurn = true;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    private void SelectNextEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead)
            {
                SelectEnemy(enemies[i]);
                return;
            }
        }
    }

    private bool AllEnemiesDead()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead)
            {
                return false;
            }
        }

        return true;
    }

    private void Win()
    {
        playerTurn = false;
        elementButtons.SetActive(false);
        messageText.text = "";
        winPanel.SetActive(true);
    }

    private void GameOver()
    {
        playerTurn = false;
        elementButtons.SetActive(false);
        messageText.text = "";
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
