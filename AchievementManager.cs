using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    void Awake()
    {
        Instance = this;
    }
    

    public void Unlock(string id)
    {
        Debug.Log("Achievement Unlocked: " + id);
    }

    public void CheckAds(int ads)
    {
        if (ads >= 1) Unlock("Ad Curious");
        if (ads >= 5) Unlock("5 Ads Watched");
        if (ads >= 10) Unlock("10 Ads Watched");
        if (ads >= 20) Unlock("20 Ads Watched");
        if (ads >= 40) Unlock("40 Ads Watched");
        if (ads >= 60) Unlock("60 Ads Watched");
        if (ads >= 80) Unlock("80 Ads Watched");
        if (ads >= 100) Unlock("100 Ads Watched");
        if (ads >= 150) Unlock("150 Ads Watched");
        if (ads >= 200) Unlock("200 Ads Watched");
    }

    public void CheckGamesPlayed(int games)
    {
        if (games >= 25) Unlock("Play 25 Games");
        if (games >= 50) Unlock("Play 50 Games");
        if (games >= 100) Unlock("Play 100 Games");
        if (games >= 250) Unlock("Play 250 Games");
        if (games >= 500) Unlock("Play 500 Games");
        if (games >= 1000) Unlock("Play 1000 Games");
        if (games >= 2000) Unlock("Play 2000 Games");
    }

    public void CheckTile(int tile)
    {
        if (tile >= 128) Unlock("Reach 128 Tile");
        if (tile >= 512) Unlock("Reach 512 Tile");
    }

    public void CheckScore(int score)
    {
        if (score >= 500) Unlock("Score 500");
        if (score >= 1000) Unlock("Score 1K");
        if (score >= 5000) Unlock("Score 5K");
        if (score >= 10000) Unlock("Score 10K");
        if (score >= 25000) Unlock("Score 25K");
        if (score >= 50000) Unlock("Score 50K");
        if (score >= 100000) Unlock("Score 100K");
        if (score >= 250000) Unlock("Score 250K");
        if (score >= 500000) Unlock("Score 500K");
        if (score >= 1000000) Unlock("Score Millionaire");
    }

    public void CheckClassic(int tile)
    {
        if (tile >= 256) Unlock("256 on Classic");
        if (tile >= 512) Unlock("512 on Classic");
        if (tile >= 1024) Unlock("1024 on Classic");
        if (tile >= 2048) Unlock("2048 on Classic");
        if (tile >= 4096) Unlock("4096 on Classic");
        if (tile >= 8192) Unlock("8192 on Classic");
    }

    public void CheckBigBoard(int tile)
    {
        if (tile >= 512) Unlock("512 on Big Board");
        if (tile >= 1024) Unlock("1024 on Big Board");
        if (tile >= 2048) Unlock("2048 on Big Board");
        if (tile >= 4096) Unlock("4096 on Big Board");
        if (tile >= 8192) Unlock("8192 on Big Board");
    }

    public void CheckTall(int tile)
    {
        if (tile >= 512) Unlock("512 Tall");
        if (tile >= 1024) Unlock("1024 Tall");
        if (tile >= 2048) Unlock("2048 Tall");
        if (tile >= 4096) Unlock("4096 Tall");
        if (tile >= 8192) Unlock("8192 Tall");
    }

    public void CheckMega(int tile)
    {
        if (tile >= 1024) Unlock("1024 Mega");
        if (tile >= 2048) Unlock("2048 Mega");
        if (tile >= 4096) Unlock("4096 Mega");
        if (tile >= 8192) Unlock("8192 Mega");
        if (tile >= 16384) Unlock("16384 Mega");
    }
}