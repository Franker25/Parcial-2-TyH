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
    private ElementData[] elements;
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
    private AudioClip transformSound;
    [SerializeField]
    private float transformTime = 0.8f;
    [SerializeField]
    private float attackTime = 0.3f;
    [SerializeField]
    private float afterAttackTime = 0.6f;
    [SerializeField]
    [Range(0, 100)]
    private int counterChance = 20;
    [SerializeField]
    private float counterDelay = 0.5f;

    private BattleEnemy selectedEnemy;
    private bool playerTurn = true;
    private bool busy = false;

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

        for (int i = 0; i < elements.Length; i++)
        {
            availableElements.Add(i);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            int randomPosition = Random.Range(0, availableElements.Count);
            int chosenElement = availableElements[randomPosition];
            enemies[i].SetWeakness(elements[chosenElement].type);
            availableElements.RemoveAt(randomPosition);
        }
    }

    private ElementData GetElementData(ElementType type)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].type == type)
            {
                return elements[i];
            }
        }

        return null;
    }

    public void SelectEnemy(BattleEnemy enemy)
    {
        if (!playerTurn || busy)
        {
            return;
        }

        ChangeSelection(enemy);
    }

    private void ChangeSelection(BattleEnemy enemy)
    {
        if (selectedEnemy != null)
        {
            selectedEnemy.SetSelected(false);
        }

        selectedEnemy = enemy;
        selectedEnemy.SetSelected(true);
    }

    public void UseElement(int elementIndex)
    {
        if (!playerTurn || busy || selectedEnemy == null)
        {
            return;
        }

        ElementData data = GetElementData((ElementType)elementIndex);

        if (data == null)
        {
            return;
        }

        if (data.isWerewolfAttack)
        {
            StartCoroutine(WerewolfAttack(data));
        }
        else
        {
            ResolveHit(data);
        }
    }

    private IEnumerator WerewolfAttack(ElementData data)
    {
        busy = true;
        elementButtons.SetActive(false);
        messageText.text = "The hero transforms into a Werewolf!";
        PlaySound(transformSound);
        hero.SetWerewolf(true);

        yield return new WaitForSeconds(transformTime);

        messageText.text = data.elementName + " attack!";
        data.PlayAttack(audioSource);

        yield return new WaitForSeconds(attackTime);

        bool turnContinues = ResolveHit(data);

        yield return new WaitForSeconds(afterAttackTime);

        hero.SetWerewolf(false);
        busy = false;

        if (turnContinues)
        {
            elementButtons.SetActive(true);
        }
    }

    private bool ResolveHit(ElementData data)
    {
        BattleEnemy target = selectedEnemy;
        bool hitWeakness = target.TakeHit(data);
        data.PlayHit(audioSource, hitWeakness);

        if (AllEnemiesDead())
        {
            Win();
            return false;
        }

        if (target.IsDead)
        {
            SelectNextEnemy();
        }

        if (hitWeakness)
        {
            if (AllLivingEnemiesWeaknessHit())
            {
                messageText.text = "WEAKNESS! " + target.EnemyName + " is weak to " + data.elementName + "! But no more extra turns...";
                StartCoroutine(EnemyTurn());
                return false;
            }

            messageText.text = "WEAKNESS! " + target.EnemyName + " is weak to " + data.elementName + "! One more turn!";
            return true;
        }

        messageText.text = data.elementName + " did little to " + target.EnemyName + "...";
        StartCoroutine(EnemyTurn());
        return false;
    }

    private IEnumerator EnemyTurn()
    {
        playerTurn = false;
        elementButtons.SetActive(false);
        ResetWeaknessHits();

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

                if (Random.Range(0, 100) < counterChance)
                {
                    yield return StartCoroutine(Counterattack(enemies[i]));

                    if (AllEnemiesDead())
                    {
                        Win();
                        yield break;
                    }
                }
            }
        }

        ResetWeaknessHits();
        messageText.text = "Your turn! Pick an element.";
        elementButtons.SetActive(true);
        playerTurn = true;
    }

    private IEnumerator Counterattack(BattleEnemy attacker)
    {
        ElementData data = GetRandomWerewolfElement();

        if (data == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(counterDelay);

        messageText.text = "The beast awakens inside the Satyr!";
        PlaySound(transformSound);
        hero.SetWerewolf(true);

        yield return new WaitForSeconds(transformTime);

        messageText.text = "Counterattack: " + data.elementName + "!";
        data.PlayAttack(audioSource);

        yield return new WaitForSeconds(attackTime);

        bool hitWeakness = attacker.TakeHit(data);
        data.PlayHit(audioSource, hitWeakness);

        if (hitWeakness)
        {
            messageText.text = "WEAKNESS! " + attacker.EnemyName + " is weak to " + data.elementName + "!";
        }
        else
        {
            messageText.text = "The counterattack hits " + attacker.EnemyName + "!";
        }

        yield return new WaitForSeconds(afterAttackTime);

        hero.SetWerewolf(false);

        if (selectedEnemy != null && selectedEnemy.IsDead)
        {
            SelectNextEnemy();
        }
    }

    private ElementData GetRandomWerewolfElement()
    {
        List<ElementData> options = new List<ElementData>();

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].isWerewolfAttack)
            {
                options.Add(elements[i]);
            }
        }

        if (options.Count == 0)
        {
            return null;
        }

        return options[Random.Range(0, options.Count)];
    }

    private bool AllLivingEnemiesWeaknessHit()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead && !enemies[i].WeaknessHitThisTurn)
            {
                return false;
            }
        }

        return true;
    }

    private void ResetWeaknessHits()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].WeaknessHitThisTurn = false;
        }
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
                ChangeSelection(enemies[i]);
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
