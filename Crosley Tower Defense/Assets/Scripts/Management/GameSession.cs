using UnityEngine;

public class GameSession
{
    public static int FinalWave { get; set; }
    public static int TowerHeight { get; set; }
    public static int Currency { get; set; }
    public static string PlayerName { get; set; }
    public static bool IsDataReadyToBeEnteredToLeaderboard { get; set; } = false;

    public static void Reset()
    {
        FinalWave = 0;
        TowerHeight = 0;
        Currency = 0;
        PlayerName = string.Empty;
        IsDataReadyToBeEnteredToLeaderboard = false;
    }

}
