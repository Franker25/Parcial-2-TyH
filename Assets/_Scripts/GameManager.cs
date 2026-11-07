using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int killsToWin = 25;
    [SerializeField]
    private TextMeshProUGUI killsText;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Upgrade[] upgrades;
    [SerializeField]
    private int killsPerUpgrade = 5;
    [SerializeField]
    private WeaponRotation weapon;
    [SerializeField]
    private Player player;
    [SerializeField]
    private TextMeshProUGUI upgradeText;

    private int kills = 0;
    public bool HasWon = false;
    public float EnemySpeedBonus = 0f;
    private bool hasSecondAxe = false;
    void Start()
    {
        upgradeText.text = "";
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        UpdateText();
    }

    public void AddKill()
    {
        if (HasWon)
        {
            return;
        }

        kills++;
        UpdateText();

        if (kills >= killsToWin)
        {
            Win();
        }
        else if (kills % killsPerUpgrade == 0)
        {
            GiveRandomUpgrade();
        }
    }

    private void UpdateText()
    {
        killsText.text = "Enemies Killed: " + kills + " / " + killsToWin;
    }

    private void Win()
    {
        HasWon = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        if (HasWon)
        {
            return;
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GiveRandomUpgrade()
    {
        Upgrade chosen = upgrades[Random.Range(0, upgrades.Length)];

        while (chosen.type == UpgradeType.SecondAxe && hasSecondAxe)
        {
            chosen = upgrades[Random.Range(0, upgrades.Length)];
        }

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
            hasSecondAxe = true;
        }
        else if (chosen.type == UpgradeType.BiggerWeapon)
        {
            weapon.GrowWeapon(chosen.amount);
        }

        EnemySpeedBonus += 0.3f;
        upgradeText.text = "Upgrade: " + chosen.upgradeName;
    }



}
