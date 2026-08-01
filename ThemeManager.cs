using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class ThemeManager : MonoBehaviour
{
    public Image background;

    public TMP_Text darkThemeButtonText;
    public TMP_Text neonThemeButtonText;

public Color neonThemeColor;

    public Color defaultColor;

    public Color darkThemeColor;
    public GameObject purchasePopup;
    public TMP_Text goldThemeButtonText;
public TMP_Text cyberThemeButtonText;

public Color goldThemeColor;
public Color cyberThemeColor;

    void Start()
    {
        ApplySavedTheme();

        UpdateDarkThemeButton();
UpdateNeonThemeButton();
UpdateGoldThemeButton();
UpdateCyberThemeButton();
    }

    public void BuyDarkTheme()
    {
        bool owned =
            PlayerPrefs.GetInt(
                "DarkThemeOwned",
                0
            ) == 1;

        if (!owned)
        {
            int coins =
                PlayerPrefs.GetInt(
                    "Coins",
                    0
                );

            if (coins < 500)
                return;

            coins -= 500;

            PlayerPrefs.SetInt(
                "Coins",
                coins
            );

            PlayerPrefs.SetInt(
                "DarkThemeOwned",
                1
            );

            PlayerPrefs.Save();
            StartCoroutine(
    ShowPurchasePopup()
);
        }

        ApplyDarkTheme();

        UpdateDarkThemeButton();
    }
    public void BuyNeonTheme()
{
    bool owned =
        PlayerPrefs.GetInt(
            "NeonThemeOwned",
            0
        ) == 1;

    if (!owned)
    {
        int coins =
            PlayerPrefs.GetInt(
                "Coins",
                0
            );

        if (coins < 1000)
            return;

        coins -= 1000;

        PlayerPrefs.SetInt(
            "Coins",
            coins
        );

        PlayerPrefs.SetInt(
            "NeonThemeOwned",
            1
        );

        PlayerPrefs.Save();
        StartCoroutine(
    ShowPurchasePopup()
);
    }

    ApplyNeonTheme();

    UpdateNeonThemeButton();
}

    public void ApplyDarkTheme()
    {
        if (PlayerPrefs.GetInt(
            "DarkThemeOwned",
            0
        ) == 0)
            return;

        background.color =
            darkThemeColor;

        PlayerPrefs.SetString(
            "SelectedTheme",
            "Dark"
        );

        PlayerPrefs.Save();

        UpdateDarkThemeButton();
UpdateNeonThemeButton();
UpdateGoldThemeButton();
UpdateCyberThemeButton();
    }
    public void ApplyNeonTheme()
{
    if (PlayerPrefs.GetInt(
        "NeonThemeOwned",
        0
    ) == 0)
        return;

    background.color =
        neonThemeColor;

    PlayerPrefs.SetString(
        "SelectedTheme",
        "Neon"
    );

    PlayerPrefs.Save();

    UpdateDarkThemeButton();
    UpdateNeonThemeButton();
}

    void ApplySavedTheme()
{
    string theme =
        PlayerPrefs.GetString(
            "SelectedTheme",
            "Default"
        );

    switch (theme)
    {
        case "Dark":
            background.color =
                darkThemeColor;
            break;

        case "Neon":
            background.color =
                neonThemeColor;
            break;
            case "Gold":
    background.color =
        goldThemeColor;
    break;

case "Cyber":
    background.color =
        cyberThemeColor;
    break;

        default:
            background.color =
                defaultColor;
            break;
    }
}

    void UpdateDarkThemeButton()
    {
        bool owned =
            PlayerPrefs.GetInt(
                "DarkThemeOwned",
                0
            ) == 1;

        string selectedTheme =
            PlayerPrefs.GetString(
                "SelectedTheme",
                "Default"
            );

        if (!owned)
        {
            darkThemeButtonText.text =
                "Dark Theme\n500 Coins";
        }
        else if (selectedTheme == "Dark")
        {
            darkThemeButtonText.text =
                "EQUIPPED";
        }
        else
        {
            darkThemeButtonText.text =
                "EQUIP";
        }
    }
   void UpdateNeonThemeButton()
{
    bool owned =
        PlayerPrefs.GetInt(
            "NeonThemeOwned",
            0
        ) == 1;

    string selectedTheme =
        PlayerPrefs.GetString(
            "SelectedTheme",
            "Default"
        );

    if (!owned)
    {
        neonThemeButtonText.text =
            "Neon Theme\n1000 Coins";
    }
    else if (selectedTheme == "Neon")
    {
        neonThemeButtonText.text =
            "EQUIPPED";
    }
    else
    {
        neonThemeButtonText.text =
            "EQUIP";
    }
} 
IEnumerator ShowPurchasePopup()
{
    purchasePopup.SetActive(true);

    yield return new WaitForSeconds(2f);

    purchasePopup.SetActive(false);
}
public void BuyGoldTheme()
{
    bool owned =
        PlayerPrefs.GetInt(
            "GoldThemeOwned",
            0
        ) == 1;

    if (!owned)
    {
        int gems =
            PlayerPrefs.GetInt(
                "Gems",
                0
            );

        if (gems < 5)
            return;

        gems -= 5;

        PlayerPrefs.SetInt(
            "Gems",
            gems
        );

        PlayerPrefs.SetInt(
            "GoldThemeOwned",
            1
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ShowPurchasePopup()
        );
    }

    ApplyGoldTheme();
}
public void ApplyGoldTheme()
{
    if (PlayerPrefs.GetInt(
        "GoldThemeOwned",
        0
    ) == 0)
        return;

    background.color =
        goldThemeColor;

    PlayerPrefs.SetString(
        "SelectedTheme",
        "Gold"
    );

    PlayerPrefs.Save();

    UpdateDarkThemeButton();
    UpdateNeonThemeButton();
    UpdateGoldThemeButton();
    UpdateCyberThemeButton();
}
public void BuyCyberTheme()
{
    bool owned =
        PlayerPrefs.GetInt(
            "CyberThemeOwned",
            0
        ) == 1;

    if (!owned)
    {
        int gems =
            PlayerPrefs.GetInt(
                "Gems",
                0
            );

        if (gems < 10)
            return;

        gems -= 10;

        PlayerPrefs.SetInt(
            "Gems",
            gems
        );

        PlayerPrefs.SetInt(
            "CyberThemeOwned",
            1
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ShowPurchasePopup()
        );
    }

    ApplyCyberTheme();
}
public void ApplyCyberTheme()
{
    if (PlayerPrefs.GetInt(
        "CyberThemeOwned",
        0
    ) == 0)
        return;

    background.color =
        cyberThemeColor;

    PlayerPrefs.SetString(
        "SelectedTheme",
        "Cyber"
    );

    PlayerPrefs.Save();

    UpdateDarkThemeButton();
    UpdateNeonThemeButton();
    UpdateGoldThemeButton();
    UpdateCyberThemeButton();
}
void UpdateGoldThemeButton()
{
    bool owned =
        PlayerPrefs.GetInt(
            "GoldThemeOwned",
            0
        ) == 1;

    string selectedTheme =
        PlayerPrefs.GetString(
            "SelectedTheme",
            "Default"
        );

    if (!owned)
    {
        goldThemeButtonText.text =
            "Gold Theme\n5 Gems";
    }
    else if (selectedTheme == "Gold")
    {
        goldThemeButtonText.text =
            "EQUIPPED";
    }
    else
    {
        goldThemeButtonText.text =
            "EQUIP";
    }
}
void UpdateCyberThemeButton()
{
    bool owned =
        PlayerPrefs.GetInt(
            "CyberThemeOwned",
            0
        ) == 1;

    string selectedTheme =
        PlayerPrefs.GetString(
            "SelectedTheme",
            "Default"
        );

    if (!owned)
    {
        cyberThemeButtonText.text =
            "Cyber Theme\n10 Gems";
    }
    else if (selectedTheme == "Cyber")
    {
        cyberThemeButtonText.text =
            "EQUIPPED";
    }
    else
    {
        cyberThemeButtonText.text =
            "EQUIP";
    }
}


}