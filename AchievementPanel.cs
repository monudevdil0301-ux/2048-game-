using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour
{
    public Transform content;
    public GameObject achievementPrefab;

    List<Achievement> achievements =
        new List<Achievement>();

    void Start()
    {
        CreateAchievements();
       achievements.Sort(CompareAchievements);
SpawnAchievements();
    }

    void CreateAchievements()
    {
        achievements.Add(new Achievement { title = "Ad Curious", coinReward = 50, completed = true });
        achievements.Add(new Achievement { title = "5 Ads Watched", coinReward = 150 });
        achievements.Add(new Achievement { title = "10 Ads Watched", coinReward = 300 });
        achievements.Add(new Achievement { title = "20 Ads Watched", gemReward = 1 });
        achievements.Add(new Achievement { title = "40 Ads Watched", coinReward = 500, gemReward = 1 });
        achievements.Add(new Achievement { title = "60 Ads Watched", gemReward = 1 });
        achievements.Add(new Achievement { title = "80 Ads Watched", coinReward = 800, gemReward = 1 });
        achievements.Add(new Achievement { title = "100 Ads Watched", gemReward = 1 });
        achievements.Add(new Achievement { title = "150 Ads Watched", coinReward = 1000, gemReward = 1 });
        achievements.Add(new Achievement { title = "200 Ads Watched", gemReward = 1 });

        achievements.Add(new Achievement { title = "First Game", coinReward = 50 });
        achievements.Add(new Achievement { title = "Reach 128 Tile", gemReward = 1 });
        achievements.Add(new Achievement { title = "Reach 512 Tile", gemReward = 2 });
        achievements.Add(new Achievement { title = "Earn 1000 Coins", gemReward = 2 });
        achievements.Add(new Achievement { title = "Play 25 Games", gemReward = 5 });

        achievements.Add(new Achievement { title = "256 on Classic", coinReward = 100 });
        achievements.Add(new Achievement { title = "512 on Classic", coinReward = 200 });
        achievements.Add(new Achievement { title = "1024 on Classic", gemReward = 1 });
        achievements.Add(new Achievement { title = "2048 on Classic", gemReward = 3 });
        achievements.Add(new Achievement { title = "4096 on Classic", gemReward = 5 });
        achievements.Add(new Achievement { title = "8192 on Classic", gemReward = 10 });

        achievements.Add(new Achievement { title = "First 5×5 Win", gemReward = 1 });
        achievements.Add(new Achievement { title = "512 on Big Board", coinReward = 100 });
        achievements.Add(new Achievement { title = "1024 on Big Board", coinReward = 200 });
        achievements.Add(new Achievement { title = "2048 on Big Board", gemReward = 2 });
        achievements.Add(new Achievement { title = "4096 on Big Board", gemReward = 4 });
        achievements.Add(new Achievement { title = "8192 on Big Board", gemReward = 8 });

        achievements.Add(new Achievement { title = "Tall Board Debut", coinReward = 50 });
        achievements.Add(new Achievement { title = "512 Tall", coinReward = 100 });
        achievements.Add(new Achievement { title = "1024 Tall", coinReward = 200 });
        achievements.Add(new Achievement { title = "2048 Tall", gemReward = 2 });
        achievements.Add(new Achievement { title = "4096 Tall", gemReward = 4 });
        achievements.Add(new Achievement { title = "8192 Tall", gemReward = 8 });

        achievements.Add(new Achievement { title = "Mega Board Debut", coinReward = 50 });
        achievements.Add(new Achievement { title = "1024 Mega", coinReward = 200 });
        achievements.Add(new Achievement { title = "2048 Mega", gemReward = 2 });
        achievements.Add(new Achievement { title = "4096 Mega", gemReward = 4 });
        achievements.Add(new Achievement { title = "8192 Mega", gemReward = 8 });
        achievements.Add(new Achievement { title = "16384 Mega", gemReward = 15 });

        achievements.Add(new Achievement { title = "Score 500", coinReward = 50 });
        achievements.Add(new Achievement { title = "Score 1K", coinReward = 100 });
        achievements.Add(new Achievement { title = "Score 5K", coinReward = 200 });
        achievements.Add(new Achievement { title = "Score 10K", gemReward = 1 });
        achievements.Add(new Achievement { title = "Score 25K", gemReward = 1 });
        achievements.Add(new Achievement { title = "Score 50K", gemReward = 2 });
        achievements.Add(new Achievement { title = "Score 100K", gemReward = 3 });
        achievements.Add(new Achievement { title = "Score 250K", gemReward = 5 });
        achievements.Add(new Achievement { title = "Score 500K", gemReward = 8 });
        achievements.Add(new Achievement { title = "Score Millionaire", gemReward = 15 });

        achievements.Add(new Achievement { title = "Play 50 Games", coinReward = 100 });
        achievements.Add(new Achievement { title = "Play 100 Games", coinReward = 200 });
        achievements.Add(new Achievement { title = "Play 250 Games", gemReward = 1 });
        achievements.Add(new Achievement { title = "Play 500 Games", gemReward = 2 });
        achievements.Add(new Achievement { title = "Play 1000 Games", gemReward = 5 });
        achievements.Add(new Achievement { title = "Play 2000 Games", gemReward = 10 });
    foreach (Achievement achievement in achievements)
{
    achievement.claimed =
        PlayerPrefs.GetInt(
            "AchievementClaimed_" + achievement.title,
            0
        ) == 1;
}
}

    void SpawnAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            GameObject obj =
                Instantiate(
                    achievementPrefab,
                    content);

            obj.GetComponent<AchievementUI>()
                .Setup(achievement);
        }
    }
    int CompareAchievements(
    Achievement a,
    Achievement b)
{
    int aPriority = GetPriority(a);
    int bPriority = GetPriority(b);

    return aPriority.CompareTo(bPriority);
}

int GetPriority(Achievement a)
{
    if (a.completed && !a.claimed)
        return 0; // claimable at top

    if (!a.completed)
        return 1; // locked in middle

    return 2; // claimed at bottom
}
}