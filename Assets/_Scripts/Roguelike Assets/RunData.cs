using System.Collections.Generic;

public static class RunData
{
    public static int Level = 1;
    public static List<int> VisitedRooms = new List<int>();
    public static List<Upgrade> ChosenUpgrades = new List<Upgrade>();

    public static void Reset()
    {
        Level = 1;
        VisitedRooms.Clear();
        ChosenUpgrades.Clear();
    }
}
