using UnityEngine;
using TMPro;

public class StatisticsManager : MonoBehaviour
{
    
    
public TMP_Text highestTileText;
public TMP_Text gamesPlayedText;
public TMP_Text totalCoinsText;
public TMP_Text totalMergesText;

public TMP_Text playTimeText;
public TMP_Text boardsUnlockedText;

public TMP_Text score4x4Text;
public TMP_Text score4x5Text;
public TMP_Text score5x5Text;
public TMP_Text score5x6Text;
    void OnEnable()
    {
        LoadStats();
    }

    public void LoadStats()
{
    highestTileText.text =
        "Highest Tile: " +
        PlayerPrefs.GetInt(
            "HighestTile",
            2
        );

    gamesPlayedText.text =
        "Games Played: " +
        PlayerPrefs.GetInt(
            "GamesPlayed",
            0
        );

    totalCoinsText.text =
        "Total Coins Earned: " +
        PlayerPrefs.GetInt(
            "TotalCoinsEarned",
            0
        );

    totalMergesText.text =
        "Total Merges: " +
        PlayerPrefs.GetInt(
            "TotalMerges",
            0
        );

    int seconds =
        PlayerPrefs.GetInt(
            "PlayTime",
            0
        );

    int hours =
        seconds / 3600;

    int minutes =
        (seconds % 3600) / 60;

    playTimeText.text =
        "Play Time: " +
        hours + "h " +
        minutes + "m";

    int boardsUnlocked = 1;

    if (PlayerPrefs.GetInt(
        "Board4x5Owned",
        0
    ) == 1)
        boardsUnlocked++;

    if (PlayerPrefs.GetInt(
        "Board5x5Owned",
        0
    ) == 1)
        boardsUnlocked++;

    if (PlayerPrefs.GetInt(
        "Board5x6Owned",
        0
    ) == 1)
        boardsUnlocked++;

    boardsUnlockedText.text =
        "Boards Unlocked: " +
        boardsUnlocked +
        "/4";

    score4x4Text.text =
        "4x4 Best: " +
        PlayerPrefs.GetInt(
            "HighScore_4x4",
            0
        );

    score4x5Text.text =
        "4x5 Best: " +
        PlayerPrefs.GetInt(
            "HighScore_4x5",
            0
        );

    score5x5Text.text =
        "5x5 Best: " +
        PlayerPrefs.GetInt(
            "HighScore_5x5",
            0
        );

    score5x6Text.text =
        "5x6 Best: " +
        PlayerPrefs.GetInt(
            "HighScore_5x6",
            0
        );
}
}