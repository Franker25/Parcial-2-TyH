using UnityEngine;

public enum UpgradeType
{
    WeaponSpeed,
    PlayerSpeed,
    SecondAxe,
    BiggerWeapon
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public UpgradeType type;
    public float amount;
}
