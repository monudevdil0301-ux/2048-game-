using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public TMP_Text bestScoreText;

    public Image fadePanel;
    public TMP_Text coinsText;
    public TMP_Text[] shopItemStatusTexts;  // Assign in Inspector - Size 4
public Button[] shopItemButtons;        // Assign in Inspector - Size 4
public int[] shopItemPrices = { 0, 45, 40, 60 };  // 4x4=owned, 4x5=45gems, 5x5=40gems, 5x6=60gems

public TMP_Text gemsText;
public Image coinsIcon;
public Image gemsIcon;
public GameObject shopPanel;
public GameObject statsPanel;

public TMP_Text gamesPlayedText;
public TMP_Text highestTileText;
public TMP_Text totalCoinsText;
public TMP_Text totalMergesText;
public GameObject achievementsPanel;


public GameObject settingsPanel;
public GameObject musicOnIcon;
public GameObject musicOffIcon;
public GameObject soundOnIcon;
public GameObject soundOffIcon;

bool musicEnabled;
bool soundEnabled;
public GameObject mainMenuPanel;



public void CloseShop()
{
    shopPanel.SetActive(false);
}

    void Start()
    {
        int best =
            PlayerPrefs.GetInt(
                "HighScore",
                0
            );

        bestScoreText.text =
            "BEST: " + best;
        int coins =
    PlayerPrefs.GetInt(
        "Coins",
        0
    );

int gems =
    PlayerPrefs.GetInt(
        "Gems",
        0
    );

coinsText.text = coins.ToString("N0");
gemsText.text = gems.ToString("N0");
musicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
soundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1;

// Initialize 4x4 as pre-owned
if (PlayerPrefs.GetInt("ShopItem_0_Owned", 0) == 0)
{
    PlayerPrefs.SetInt("ShopItem_0_Owned", 1);
    PlayerPrefs.SetInt("ShopItem_0_Selected", 1);
    PlayerPrefs.Save();

}

RefreshToggleUI();
InitializeShop();
GameDistribution.Instance.PreloadRewardedAd();
GameDistribution.OnRewardGame += GiveReward;
    }

   public void PlayGame()
{
    AudioManager.instance.PlayButton();

    GameDistribution.OnResumeGame += StartGameAfterAd;

    GameDistribution.Instance.ShowAd();
}
void StartGameAfterAd()
{
    GameDistribution.OnResumeGame -= StartGameAfterAd;

    PlayerPrefs.DeleteKey("SaveData");

    StartCoroutine(LoadGame());
}

    IEnumerator LoadGame()
    {
        float alpha = 0;

        Color color =
            fadePanel.color;

        while (alpha < 1)
        {
            alpha +=
                Time.deltaTime * 2f;

            color.a = alpha;

            fadePanel.color =
                color;

            yield return null;
        }

        SceneManager.LoadScene(
            "Game"
        );
    }
    public void OpenStats()
{
    statsPanel.SetActive(true);

    gamesPlayedText.text =
        "Games Played: " +
        PlayerPrefs.GetInt(
            "GamesPlayed",
            0
        );

    highestTileText.text =
        "Highest Tile: " +
        PlayerPrefs.GetInt(
            "HighestTile",
            2
        );

    totalCoinsText.text =
        "Coins Earned: " +
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
        
}

public void CloseStats()
{
    statsPanel.SetActive(false);
}
public void OpenAchievements()
{
    achievementsPanel.SetActive(true);

    // firstGameText.text =
    //     PlayerPrefs.GetInt(
    //         "GamesPlayed",
    //         0
    //     ) >= 1
    //     ? "✅ First Game"
    //     : "⬜ First Game";

    // tile128Text.text =
    //     PlayerPrefs.GetInt(
    //         "HighestTile",
    //         2
    //     ) >= 128
    //     ? "✅ Reach 128 Tile"
    //     : "⬜ Reach 128 Tile";

    // tile512Text.text =
    //     PlayerPrefs.GetInt(
    //         "HighestTile",
    //         2
    //     ) >= 512
    //     ? "✅ Reach 512 Tile"
    //     : "⬜ Reach 512 Tile";

    // coins1000Text.text =
    //     PlayerPrefs.GetInt(
    //         "TotalCoinsEarned",
    //         0
    //     ) >= 1000
    //     ? "✅ Earn 1000 Coins"
    //     : "⬜ Earn 1000 Coins";

    // games25Text.text =
    //     PlayerPrefs.GetInt(
    //         "GamesPlayed",
    //         0
    //     ) >= 25
    //     ? "✅ Play 25 Games"
    //     : "⬜ Play 25 Games";

    // firstGameRewardText.text =
    //     PlayerPrefs.GetInt(
    //         "Achievement_FirstGame",
    //         0
    //     ) == 1
    //     ? "CLAIMED"
    //     : "CLAIM";

    // tile128RewardText.text =
    //     PlayerPrefs.GetInt(
    //         "Achievement_128",
    //         0
    //     ) == 1
    //     ? "CLAIMED"
    //     : "CLAIM";

    // tile512RewardText.text =
    //     PlayerPrefs.GetInt(
    //         "Achievement_512",
    //         0
    //     ) == 1
    //     ? "CLAIMED"
    //     : "CLAIM";

    // coins1000RewardText.text =
    //     PlayerPrefs.GetInt(
    //         "Achievement_1000Coins",
    //         0
    //     ) == 1
    //     ? "CLAIMED"
    //     : "CLAIM";

    // games25RewardText.text =
    //     PlayerPrefs.GetInt(
    //         "Achievement_25Games",
    //         0
    //     ) == 1
    //     ? "CLAIMED"
    //     : "CLAIM";
}
public void CloseAchievements()
{
    achievementsPanel.SetActive(false);
}
public void ClaimAchievementReward(
    string achievementKey,
    int coinsReward,
    int gemsReward
)
{
    // Already claimed?
    if (PlayerPrefs.GetInt(
        achievementKey,
        0
    ) == 1)
    {
        return;
    }

    int coins =
        PlayerPrefs.GetInt(
            "Coins",
            0
        );

    int gems =
        PlayerPrefs.GetInt(
            "Gems",
            0
        );

    coins += coinsReward;
    gems += gemsReward;

    PlayerPrefs.SetInt(
        "Coins",
        coins
    );

    PlayerPrefs.SetInt(
        "Gems",
        gems
    );

    PlayerPrefs.SetInt(
        achievementKey,
        1
    );

    PlayerPrefs.Save();
    coinsText.text = coins.ToString("N0");
    gemsText.text = gems.ToString("N0");

    OpenAchievements();
}
public void OpenSettings()
{
    mainMenuPanel.SetActive(false);
    settingsPanel.SetActive(true);
}

public void CloseSettings()
{
    settingsPanel.SetActive(false);
    mainMenuPanel.SetActive(true);
}
void RefreshToggleUI()
{
    musicOnIcon.SetActive(musicEnabled);
    musicOffIcon.SetActive(!musicEnabled);
    soundOnIcon.SetActive(soundEnabled);
    soundOffIcon.SetActive(!soundEnabled);
}

public void ToggleMusic()
{
    musicEnabled = !musicEnabled;
    PlayerPrefs.SetInt("Music", musicEnabled ? 1 : 0);
    PlayerPrefs.Save();
    if (AudioManager.instance != null)
        AudioManager.instance.SetMusicEnabled(musicEnabled);
    RefreshToggleUI();
}

public void ToggleSound()
{
    soundEnabled = !soundEnabled;
    PlayerPrefs.SetInt("Sound", soundEnabled ? 1 : 0);
    PlayerPrefs.Save();
    if (AudioManager.instance != null)
        AudioManager.instance.SetSoundEnabled(soundEnabled);
    RefreshToggleUI();
}

void InitializeShop()
{
    // Connect button click events
    for (int i = 0; i < shopItemButtons.Length; i++)
    {
        int index = i;  // Local copy for closure
        shopItemButtons[i].onClick.AddListener(() => BuyShopItem(index));
    }
    UpdateShopItems();
}


public void OpenShop()
{
    shopPanel.SetActive(true);
    UpdateShopItems();  // Refresh when opening
}


void UpdateShopItems()
{
    for (int i = 0; i < shopItemStatusTexts.Length; i++)
    {
        bool isOwned =
            PlayerPrefs.GetInt($"ShopItem_{i}_Owned", 0) == 1;

        bool isSelected =
            PlayerPrefs.GetInt($"ShopItem_{i}_Selected", 0) == 1;

        if (isSelected)
            shopItemStatusTexts[i].text = "SELECTED";
        else if (isOwned)
            shopItemStatusTexts[i].text = "OWNED";
        else
            shopItemStatusTexts[i].text = "BUY";
    }
}


public void BuyShopItem(int itemIndex)
{
    int gems = PlayerPrefs.GetInt("Gems", 0);
    int cost = shopItemPrices[itemIndex];
    
    bool isOwned = PlayerPrefs.GetInt($"ShopItem_{itemIndex}_Owned", 0) == 1;
    
    if (isOwned)
    {
        // Already owned - just select it
        Debug.Log($"Item {itemIndex} already owned. Selecting...");
        // Deselect all others
        for (int i = 0; i < shopItemPrices.Length; i++)
        {
            PlayerPrefs.SetInt($"ShopItem_{i}_Selected", 0);
        }
        PlayerPrefs.SetInt($"ShopItem_{itemIndex}_Selected", 1);
    }
    else if (gems >= cost)
    {
        // Buy it with gems
        Debug.Log($"Buying item {itemIndex} for {cost} gems");
        gems -= cost;
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.SetInt($"ShopItem_{itemIndex}_Owned", 1);
        // Deselect all others
        for (int i = 0; i < shopItemPrices.Length; i++)
        {
            PlayerPrefs.SetInt($"ShopItem_{i}_Selected", 0);
        }
        PlayerPrefs.SetInt($"ShopItem_{itemIndex}_Selected", 1);
        gemsText.text = gems.ToString("N0");
    }
    else
    {
        Debug.Log($"Not enough gems! Need {cost}, have {gems}");
        return;  // Not enough gems
    }
    
    PlayerPrefs.Save();
    UpdateShopItems();  // Refresh all item texts
}
public void WatchRewardedAd()
{
    if (GameDistribution.Instance.IsRewardedVideoLoaded())
    {
        GameDistribution.Instance.ShowRewardedAd();
    }
    else
    {
        Debug.Log("Rewarded ad not loaded.");
    }
}
void GiveReward()
{
    int gems = PlayerPrefs.GetInt("Gems", 0);

    gems += 10;

    PlayerPrefs.SetInt("Gems", gems);
    PlayerPrefs.Save();

    gemsText.text = gems.ToString("N0");

    Debug.Log("Reward granted!");
    GameDistribution.Instance.PreloadRewardedAd();
}
}