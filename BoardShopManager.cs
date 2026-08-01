using UnityEngine;
using TMPro;

public class BoardShopManager : MonoBehaviour
{
    public TMP_Text board4x4Text;
    public TMP_Text board4x5Text;
    public TMP_Text board5x5Text;
    public TMP_Text board5x6Text;

    void Start()
    {
        UpdateBoardStatus();
    }

    public void Select4x4()
    {
        PlayerPrefs.SetString("SelectedBoard", "4x4");
        PlayerPrefs.Save();
        UpdateBoardStatus();
    }

    public void BuyOrSelect4x5()
    {
        bool owned = PlayerPrefs.GetInt("Board4x5Owned", 0) == 1;

        if (!owned)
        {
            int gems = PlayerPrefs.GetInt("Gems", 0);
            if (gems < 5) return;

            gems -= 5;
            PlayerPrefs.SetInt("Gems", gems);
            PlayerPrefs.SetInt("Board4x5Owned", 1);
            // FIXED: removed erroneous gems += 1 and duplicate SetInt
        }

        PlayerPrefs.SetString("SelectedBoard", "4x5");
        PlayerPrefs.Save();
        UpdateBoardStatus();
    }

    public void BuyOrSelect5x5()
    {
        bool owned = PlayerPrefs.GetInt("Board5x5Owned", 0) == 1;

        if (!owned)
        {
            int gems = PlayerPrefs.GetInt("Gems", 0);
            if (gems < 10) return;

            gems -= 10;
            PlayerPrefs.SetInt("Gems", gems);
            PlayerPrefs.SetInt("Board5x5Owned", 1);
            // FIXED: removed erroneous gems += 2 and duplicate SetInt
        }

        PlayerPrefs.SetString("SelectedBoard", "5x5");
        PlayerPrefs.Save();
        UpdateBoardStatus();
    }

    public void BuyOrSelect5x6()
    {
        bool owned = PlayerPrefs.GetInt("Board5x6Owned", 0) == 1;

        if (!owned)
        {
            int gems = PlayerPrefs.GetInt("Gems", 0);
            if (gems < 20) return;

            gems -= 20;
            PlayerPrefs.SetInt("Gems", gems);
            PlayerPrefs.SetInt("Board5x6Owned", 1);
            // FIXED: removed erroneous gems += 3 and duplicate SetInt
        }

        PlayerPrefs.SetString("SelectedBoard", "5x6");
        PlayerPrefs.Save();
        UpdateBoardStatus();
    }

    void UpdateBoardStatus()
    {
        string selectedBoard = PlayerPrefs.GetString("SelectedBoard", "4x4");

        board4x4Text.text = selectedBoard == "4x4" ? "SELECTED" : "OWNED";

        board4x5Text.text = GetBoardText("Board4x5Owned", "4x5", selectedBoard);
        board5x5Text.text = GetBoardText("Board5x5Owned", "5x5", selectedBoard);
        board5x6Text.text = GetBoardText("Board5x6Owned", "5x6", selectedBoard);

        PlayerPrefs.Save();
    }

    string GetBoardText(string boardKey, string boardName, string selectedBoard)
    {
        bool owned = PlayerPrefs.GetInt(boardKey, 0) == 1;
        if (!owned) return "LOCKED";
        if (selectedBoard == boardName) return "EQUIPPED";
        return "OWNED";
    }
}
