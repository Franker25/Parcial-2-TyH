using System.Collections.Generic;

public static class RunData
{
    public static int Level = 1;
    public static List<Upgrade> ChosenUpgrades = new List<Upgrade>();

    public static void Reset()
    {
        Level = 1;
        ChosenUpgrades.Clear();
    }
}
