using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int totalLevels = 3;
    [SerializeField]
    private int killsFirstLevel = 10;
    [SerializeField]
    private int extraKillsPerLevel = 5;
    [SerializeField]
    private int killsPerUpgrade = 5;
    [SerializeField]
    private float enemySpeedPerLevel = 1f;
    [SerializeField]
    private TextMeshProUGUI killsText;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject upgradePanel;
    [SerializeField]
    private GameObject levelClearedPanel;
    [SerializeField]
    private GameObject[] optionButtons;
    [SerializeField]
    private Upgrade[] upgrades;
    [SerializeField]
    private WeaponRotation weapon;
    [SerializeField]
    private Player player;

    private int kills = 0;
    private int killsToClear = 0;
    private bool choosing = false;
    private Upgrade[] currentOptions;

    public bool HasWon = false;
    public float EnemySpeedBonus = 0f;

    void Awake()
    {
        EnemySpeedBonus = (RunData.Level - 1) * enemySpeedPerLevel;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        upgradePanel.SetActive(false);
        levelClearedPanel.SetActive(false);

        for (int i = 0; i < RunData.ChosenUpgrades.Count; i++)
        {
            ApplyUpgrade(RunData.ChosenUpgrades[i]);
        }

        killsToClear = killsFirstLevel + (RunData.Level - 1) * extraKillsPerLevel;
        UpdateText();
    }

    public void AddKill()
    {
        if (HasWon || choosing)
        {
            return;
        }

        kills++;
        UpdateText();

        if (kills >= killsToClear)
        {
            LevelCleared();
        }
        else if (kills % killsPerUpgrade == 0)
        {
            ShowUpgradeChoice();
        }
    }

    private void LevelCleared()
    {
        if (RunData.Level >= totalLevels)
        {
            Win();
        }
        else
        {
            ShowLevelCleared();
        }
    }

    private void ShowLevelCleared()
    {
        choosing = true;
        Time.timeScale = 0f;
        levelClearedPanel.SetActive(true);
    }

    public void NextLevel()
    {
        levelClearedPanel.SetActive(false);
        RunData.Level++;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowUpgradeChoice()
    {
        choosing = true;
        Time.timeScale = 0f;

        List<Upgrade> available = new List<Upgrade>();
        for (int i = 0; i < upgrades.Length; i++)
        {
            bool alreadyHave = OnlyOnce(upgrades[i].type) && Owns(upgrades[i].type);
            if (!alreadyHave)
            {
                available.Add(upgrades[i]);
            }
        }

        int optionCount = Mathf.Min(optionButtons.Length, available.Count);
        currentOptions = new Upgrade[optionCount];

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < optionCount)
            {
                int pick = Random.Range(0, available.Count);
                currentOptions[i] = available[pick];
                available.RemoveAt(pick);

                optionButtons[i].SetActive(true);
                TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "<b><size=115%>" + currentOptions[i].upgradeName + "</size></b>\n\n" + currentOptions[i].description; ;
            }
            else
            {
                optionButtons[i].SetActive(false);
            }
        }

        upgradePanel.SetActive(true);
    }

    public void ChooseUpgrade(int index)
    {
        if (index >= currentOptions.Length)
        {
            return;
        }

        Upgrade chosen = currentOptions[index];
        RunData.ChosenUpgrades.Add(chosen);
        ApplyUpgrade(chosen);

        upgradePanel.SetActive(false);
        choosing = false;
        Time.timeScale = 1f;
    }

    private bool OnlyOnce(UpgradeType type)
    {
        return type == UpgradeType.SecondAxe || type == UpgradeType.ThirdAxe || type == UpgradeType.Dash;
    }

    private bool Owns(UpgradeType type)
    {
        for (int i = 0; i < RunData.ChosenUpgrades.Count; i++)
        {
            if (RunData.ChosenUpgrades[i].type == type)
            {
                return true;
            }
        }
        return false;
    }

    private void ApplyUpgrade(Upgrade chosen)
    {
        if (chosen.type == UpgradeType.WeaponSpeed)
        {
            weapon.AddRotationSpeed(chosen.amount);
        }
        else if (chosen.type == UpgradeType.PlayerSpeed)
        {
            player.AddSpeed(chosen.amount);
        }
        else if (chosen.type == UpgradeType.SecondAxe)
        {
            weapon.ActivateSecondAxe();
        }
        else if (chosen.type == UpgradeType.BiggerWeapon)
        {
            weapon.GrowWeapon(chosen.amount);
        }
        else if (chosen.type == UpgradeType.Shield)
        {
            player.AddShield();
        }
        else if (chosen.type == UpgradeType.ThirdAxe)
        {
            weapon.ActivateThirdAxe();
        }
        else if (chosen.type == UpgradeType.SlowCrabs)
        {
            EnemySpeedBonus -= chosen.amount;
        }
        else if (chosen.type == UpgradeType.Dash)
        {
            player.EnableDash();
        }
    }

    private void UpdateText()
    {
        killsText.text = "Level " + RunData.Level + " / " + totalLevels + "\nEnemies Killed: " + kills + " / " + killsToClear;
    }

    private void Win()
    {
        HasWon = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;

        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.StopMusic();
        }
    }

    public void GameOver()
    {
        if (HasWon)
        {
            return;
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.StopMusic();
        }
    }

    public void RestartGame()
    {
        RunData.Reset();
        Time.timeScale = 1f;

        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.Restart();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
