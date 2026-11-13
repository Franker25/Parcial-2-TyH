using UnityEngine;

public enum UpgradeType
{
    WeaponSpeed,
    PlayerSpeed,
    SecondAxe,
    BiggerWeapon,
    Shield,
    ThirdAxe,
    SlowCrabs,
    Dash
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;
    public UpgradeType type;
    public float amount;
}
