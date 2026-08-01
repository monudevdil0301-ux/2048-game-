using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SaveData
{
    public int[] values;

    public int score;

    public int coins;

    public int gems;

    public bool reviveUsed;

    public bool victoryShown;

    
}

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;
    public RectTransform boardRect;
    public Transform tilesParent;
    public TMP_Text scoreText;
    public TMP_Text coinsText;
    public TMP_Text gemsText;
    public TMP_Text comboText;
    public Image coinsIcon;
    public Image gemsIcon;
    public CanvasGroup comboCanvasGroup;
    public GameObject gameOverPanel;
    
    public GameObject continuePanel;
    public GameObject victoryPanel;
    public GameObject bestScorePanel;
    public GameObject pausePanel;
    public ParticleSystem mergeParticlesPrefab;
    public bool adReviveUsed = false;

    public int boardWidth = 4;
    public int boardHeight = 4;

    Tile[,] tiles;

    int score;
    int coins;
    int gems;
int totalComboEventsCount = 0; 
   bool reviveUsed = false;
    bool gameFinished = false;
    bool victoryShown = false;

    int comboCount = 0;
    float sessionTime = 0f;
    float boardPixelSize;

    Coroutine comboRoutine;
Vector2 lastBoardSize;
    Vector2 startTouch;
    Vector2 endTouch;

    void Start()
    {
        
        LoadBoardSize();
        Debug.Log("Board Size Loaded: " + boardWidth + " x " + boardHeight);
        tiles = new Tile[boardWidth, boardHeight];
        coins = PlayerPrefs.GetInt("Coins", 0);
        gems = PlayerPrefs.GetInt("Gems", 0);

        victoryPanel.SetActive(false);
        bestScorePanel.SetActive(false);
        UpdateCurrencyUI();

        gameOverPanel.SetActive(false);
        
        continuePanel.SetActive(false);
        comboCanvasGroup.alpha = 0;
        gameFinished = false;
        reviveUsed = false;
        victoryShown = false;
        adReviveUsed = false;

        StartCoroutine(InitAfterLayout());
    }

    IEnumerator InitAfterLayout()
    {
        // Wait for Canvas layout to fully resolve
        yield return null;
        yield return null;

        boardPixelSize = boardRect.rect.width;

        Debug.Log("Board pixel size: " + boardPixelSize);

        if (boardPixelSize <= 0)
        {
            boardPixelSize = 850f;
            Debug.LogWarning("Board size was 0, using fallback 850");
        }

        LoadGame();

UpdateAllTileSizes();

UpdateScore();
    }

    void Update()
{
    sessionTime += Time.deltaTime;

    if (continuePanel.activeSelf) return;
    if (gameOverPanel.activeSelf) return;

    // Keyboard Controls
    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        MoveLeft();

    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        MoveRight();

    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        MoveUp();

    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        MoveDown();

    DetectSwipe();

    RectTransform boardRect =
        tilesParent.GetComponent<RectTransform>();

    Vector2 currentSize =
        boardRect.rect.size;

    if (currentSize != lastBoardSize)
    {
        lastBoardSize = currentSize;

        UpdateAllTileSizes();
    }
}

    void DetectSwipe()
    { 
        if (Input.GetMouseButtonDown(0))
            startTouch = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;
            Vector2 delta = endTouch - startTouch;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 50) MoveRight();
                else if (delta.x < -50) MoveLeft();
            }
            else
            {
                if (delta.y > 50) MoveUp();
                else if (delta.y < -50) MoveDown();
            }
        }
    }

    void MoveLeft()
    {
        bool moved = false;
        for (int y = 0; y < boardHeight; y++)
            for (int x = 0; x < boardWidth; x++)
                moved |= MoveTile(x, y, -1, 0);
                if (moved)
        AudioManager.instance.PlaySwipe();

        AfterMove(moved);
    }

    void MoveRight()
    {
        bool moved = false;
        for (int y = 0; y < boardHeight; y++)
            for (int x = boardWidth - 2; x >= 0; x--)
                moved |= MoveTile(x, y, 1, 0);
                if (moved)
        AudioManager.instance.PlaySwipe();

        AfterMove(moved);
    }

    void MoveUp()
    {
        bool moved = false;
        for (int x = 0; x < boardWidth; x++)
            for (int y = 1; y < boardHeight; y++)
                moved |= MoveTile(x, y, 0, -1);
                if (moved)
        AudioManager.instance.PlaySwipe();

        AfterMove(moved);
    }

    void MoveDown()
    {
        bool moved = false;
        for (int x = 0; x < boardWidth; x++)
            for (int y = boardHeight - 2; y >= 0; y--)
                moved |= MoveTile(x, y, 0, 1);
                if (moved)
        AudioManager.instance.PlaySwipe();

        AfterMove(moved);
    }

    bool MoveTile(int startX, int startY, int moveX, int moveY)
    {
        Tile tile = tiles[startX, startY];
        if (tile == null) return false;

        int x = startX;
        int y = startY;

        while (true)
        {
            int nextX = x + moveX;
            int nextY = y + moveY;

            if (nextX < 0 || nextX >= boardWidth ||
                nextY < 0 || nextY >= boardHeight)
                break;

            if (tiles[nextX, nextY] == null)
            {
                tiles[nextX, nextY] = tile;
                tiles[x, y] = null;
                x = nextX;
                y = nextY;
            }
            else
            {
                if (tiles[nextX, nextY].value == tile.value)
                {
                    int mergedValue = tile.value * 2;

                    AddMerge();
                    UpdateHighestTile(mergedValue);

                    if (mergedValue >= 2048 && !victoryShown)
                    {
                        victoryShown = true;
                        victoryPanel.SetActive(true);
                        CameraShake.instance.Shake(25f);
                    }

                    tiles[nextX, nextY].SetValue(mergedValue);
                    tiles[nextX, nextY].PunchAnimation();
                    if (mergedValue >= 512)
{
    tiles[nextX, nextY].Flash();
}

                    if (mergedValue >= 128)
                        CameraShake.instance.Shake(12f);

                    comboCount++;
score += mergedValue;
UpdateScore();

                  if (mergeParticlesPrefab)
{
    ParticleSystem ps =
        Instantiate(
            mergeParticlesPrefab,
            tilesParent);

    ps.transform.localPosition =
        GetPosition(nextX, nextY);

    var main = ps.main;

    float size = 1f;

    if (mergedValue >= 64)
        size = 1.2f;

    if (mergedValue >= 256)
        size = 1.5f;

    if (mergedValue >= 1024)
        size = 2f;

    ps.transform.localScale =
        Vector3.one * size;

    ps.Play();

    Destroy(ps.gameObject, 1f);
}

                    if (AudioManager.instance)
                        AudioManager.instance.PlayMerge();

                    Destroy(tile.gameObject);
                    tiles[x, y] = null;
                    return true;
                }

                break;
            }
        }

        if (x != startX || y != startY)
        {
            tile.MoveTo(GetPosition(x, y));
            return true;
        }

        return false;
    }

  void AfterMove(bool moved)
{
    if (moved)
    {
        SpawnTile();

        if (comboCount == 2)
        {
            totalComboEventsCount++;
            // Reward gem only on 1st, 4th, 7th, 10th... combos
            if (totalComboEventsCount % 4 == 1)
            {
                gems += 1;
            }
            ShowCombo(2);
        }
        else if (comboCount == 3)
        {
            totalComboEventsCount++;
            // Reward gem only on 1st, 4th, 7th, 10th... combos
            if (totalComboEventsCount % 4 == 1)
            {
                gems += 1;
            }
            ShowCombo(-1);
        }
        else  // ← KEY: Clear animation if NO combo occurred
        {
            if (comboRoutine != null)
                StopCoroutine(comboRoutine);

            comboCanvasGroup.alpha = 0;
        }

        // Save gems if a combo was triggered
        if (comboCount >= 2)
        {
            PlayerPrefs.SetInt("Gems", gems);
            UpdateCurrencyUI();
        }

        comboCount = 0;
    }
    else
    {
        comboCount = 0;
    }
    SaveGame();
    CheckGameOver();
}
    

  void SpawnTile()
{
    if (IsBoardFull()) return;

    int x, y;
    do
    {
        x = Random.Range(0, boardWidth);
        y = Random.Range(0, boardHeight);
    }
    while (tiles[x, y] != null);

    GameObject obj = Instantiate(tilePrefab, tilesParent);

    Tile tile = obj.GetComponent<Tile>();
    tile.SetValue(Random.value < 0.9f ? 2 : 4);
    AudioManager.instance.PlayPop();

   RectTransform rt =
    obj.GetComponent<RectTransform>();

float cellWidth = GetCellWidth();
float cellHeight = GetCellHeight();

rt.sizeDelta =
    new Vector2(
        cellWidth - 8f,
        cellHeight - 8f
    );
    rt.anchoredPosition =
        GetPosition(x, y);

    tiles[x, y] = tile;
}
    // ─── Responsive Layout Helpers ───────────────────────────────────────────

    /// <summary>
    /// Returns a uniform cell size in pixels that fits the board panel on any screen.
    /// Uses the smaller axis of the board panel so cells are always square and
    /// never overflow on non-square board configs (e.g. 4x5, 5x6).
    /// </summary>
 float GetCellWidth()
{
    return boardRect.rect.width / boardWidth;
}

float GetCellHeight()
{
    return boardRect.rect.height / boardHeight;
}
    /// <summary>
    /// Returns the local anchored position for a grid cell (x, y).
    /// Origin is the centre of the board panel.
    /// </summary>
   Vector2 GetPosition(int x, int y)
{
    float cellWidth =
        GetCellWidth();

    float cellHeight =
        GetCellHeight();

    float totalWidth =
        cellWidth * boardWidth;

    float totalHeight =
        cellHeight * boardHeight;

    float startX =
        -totalWidth / 2f +
        cellWidth / 2f;

    float startY =
        totalHeight / 2f -
        cellHeight / 2f;

    return new Vector2(
        startX + x * cellWidth,
        startY - y * cellHeight
    );
}

    // ─────────────────────────────────────────────────────────────────────────

    bool IsBoardFull()
    {
        for (int y = 0; y < boardHeight; y++)
            for (int x = 0; x < boardWidth; x++)
                if (tiles[x, y] == null)
                    return false;

        
        return true;
    }

   void CheckGameOver()
{
    if (!IsBoardFull()) return;

    for (int y = 0; y < boardHeight; y++)
    {
        for (int x = 0; x < boardWidth; x++)
        {
            if (x < boardWidth - 1 && tiles[x, y].value == tiles[x + 1, y].value)
                return;

            if (y < boardHeight - 1 && tiles[x, y].value == tiles[x, y + 1].value)
                return;
        }
    }

    if (gameOverCount < 99)
    {AudioManager.instance.PlayGameOver();
        continuePanel.SetActive(true);
        return;
    }

    ShowGameOver();
}

   void ShowGameOver()
{
    RewardPlayer();
    AddPlayTime();
    AddGamesPlayed();
 AudioManager.instance.PlayGameOver();
    gameOverPanel.SetActive(true);
}
   
    void RewardPlayer()
    {
        if (gameFinished) return;
        gameFinished = true;

        int earnedCoins = Mathf.Max(1, score / 50);
        coins += earnedCoins;
        AddCoinsEarned(earnedCoins);

        int best = PlayerPrefs.GetInt("HighScore", 0);

        if (score > best)
        {
            gems += 1;
            PlayerPrefs.SetInt("HighScore", score);
            bestScorePanel.SetActive(true);
            StartCoroutine(HideBestScorePopup());
        }

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.Save();
        UpdateCurrencyUI();
    }

    void UpdateScore()
    {
        StopCoroutine("AnimateScore");
        StartCoroutine(AnimateScore());
    }

    IEnumerator AnimateScore()
    {
        int displayedScore = 0;
        string cleanText = scoreText.text.Replace("SCORE: ", "");
        int.TryParse(cleanText, out displayedScore);

        float time = 0f;
        float duration = 0.2f;

        while (time < duration)
        {
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(displayedScore, score, time / duration));
            scoreText.text = "SCORE: " + currentScore;
            time += Time.deltaTime;
            yield return null;
        }

        scoreText.text = "SCORE: " + score;
    }

   void ShowCombo(int combo)
{
    if (combo == -1)
        comboText.text = "BLAST!";
    else
        comboText.text = "COMBO x" + combo;

    if (comboRoutine != null)
        StopCoroutine(comboRoutine);

    comboRoutine = StartCoroutine(ComboAnimation());
}
    IEnumerator ComboAnimation()
    {
        comboCanvasGroup.alpha = 1;
        comboText.transform.localScale = Vector3.one * 1.4f;

        float time = 0f;
        while (time < 0.25f)
        {
            comboText.transform.localScale = Vector3.Lerp(Vector3.one * 1.4f, Vector3.one, time / 0.25f);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        while (comboCanvasGroup.alpha > 0)
        {
            comboCanvasGroup.alpha -= Time.deltaTime * 4f;
            yield return null;
        }
    }

   void SaveGame()
{
    SaveData data = new SaveData();

    data.values = new int[boardWidth * boardHeight];

    int index = 0;

    for (int y = 0; y < boardHeight; y++)
    {
        for (int x = 0; x < boardWidth; x++)
        {
            data.values[index] =
                tiles[x, y] != null
                ? tiles[x, y].value
                : 0;

            index++;
        }
    }

    data.score = score;
    data.coins = coins;
    data.gems = gems;
    data.reviveUsed = reviveUsed;
    data.victoryShown = victoryShown;

    string json = JsonUtility.ToJson(data);

    PlayerPrefs.SetString("SaveData", json);
    PlayerPrefs.Save();
}
    void LoadGame()
    {
        tiles = new Tile[boardWidth, boardHeight];

        if (!PlayerPrefs.HasKey("SaveData"))
        {
            SpawnTile();
            SpawnTile();
            return;
        }

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);
if (data == null || data.values == null ||
    data.values.Length != boardWidth * boardHeight)
{
    // Reset game state
    score = 0;
    coins = PlayerPrefs.GetInt("Coins", 0);
    gems = PlayerPrefs.GetInt("Gems", 0);

    reviveUsed = false;
    victoryShown = false;

    UpdateCurrencyUI();
    UpdateScore();

    // Delete corrupted save
    PlayerPrefs.DeleteKey("SaveData");

    SpawnTile();
    SpawnTile();

    return;
}

        score = data.score;
        coins = data.coins;
gems = data.gems;
reviveUsed = data.reviveUsed;
victoryShown = data.victoryShown;



UpdateCurrencyUI();
UpdateScore();

        int index = 0;
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                int value = data.values[index];
                if (value != 0)
                {
                    GameObject obj = Instantiate(tilePrefab, tilesParent);
                    Tile tile = obj.GetComponent<Tile>();
                    tile.SetValue(value);

                    // Apply responsive size + position
                   RectTransform rt = obj.GetComponent<RectTransform>();

float cellWidth = GetCellWidth();
float cellHeight = GetCellHeight();

rt.sizeDelta =
    new Vector2(
        cellWidth - 8f,
        cellHeight - 8f
    );

rt.anchoredPosition =
    GetPosition(x, y);

                    tiles[x, y] = tile;
                }
                index++;
            }
        }
        UpdateAllTileSizes();
    }

    void UpdateCurrencyUI()
    {
        coinsText.text = coins.ToString("N0");
        gemsText.text  = gems.ToString("N0");
    }

  public void ContinueGame()
{
    gameOverCount++;
    
 
    if (gems < 15)
        return;

    gems-=15;

    PlayerPrefs.SetInt("Gems", gems);

    UpdateCurrencyUI();

    continuePanel.SetActive(false);

    RemoveRandomTiles(4);

    SpawnTile();
    SaveGame();
}

 public void ContinueWithAd()
{
    Debug.Log("Watch Ad button clicked");

    if (GameDistribution.Instance.IsRewardedVideoLoaded())
    {
        Debug.Log("Rewarded ad is ready");
        GameDistribution.Instance.ShowRewardedAd();
    }
    else
    {
        Debug.Log("Rewarded ad NOT ready. Preloading...");
        GameDistribution.Instance.PreloadRewardedAd();
    }
}

    public void RestartGame()
    {AudioManager.instance.PlayButton();
        gameOverCount = 0;

        PlayerPrefs.DeleteKey("SaveData");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void RemoveRandomTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int attempts = 0;
            while (attempts < 100)
            {
                int x = Random.Range(0, boardWidth);
                int y = Random.Range(0, boardHeight);
                if (tiles[x, y] != null)
                {
                    Destroy(tiles[x, y].gameObject);
                    tiles[x, y] = null;
                    break;
                }
                attempts++;
            }
        }
        SaveGame();
    }

    public void CloseVictory()
    {
        victoryPanel.SetActive(false);
    }

    IEnumerator HideBestScorePopup()
    {
        yield return new WaitForSeconds(3f);
        bestScorePanel.SetActive(false);
    }

    void AddGamesPlayed()
    {
        int games = PlayerPrefs.GetInt("GamesPlayed", 0);
        PlayerPrefs.SetInt("GamesPlayed", games + 1);
    }

    void AddCoinsEarned(int amount)
    {
        int total = PlayerPrefs.GetInt("TotalCoinsEarned", 0);
        PlayerPrefs.SetInt("TotalCoinsEarned", total + amount);
    }

    void AddMerge()
    {
        int merges = PlayerPrefs.GetInt("TotalMerges", 0);
        PlayerPrefs.SetInt("TotalMerges", merges + 1);
    }

    void UpdateHighestTile(int value)
    {
        int highest = PlayerPrefs.GetInt("HighestTile", 2);
        if (value > highest)
            PlayerPrefs.SetInt("HighestTile", value);
    }

    void AddPlayTime()
    {
        int totalSeconds = PlayerPrefs.GetInt("PlayTime", 0);
        PlayerPrefs.SetInt("PlayTime", totalSeconds + Mathf.RoundToInt(sessionTime));
    }

    // ─── UPDATED BOARD SIZE LOADING ───────────────────────────────────────────
    // Now reads from shop system instead of hardcoded string
    void LoadBoardSize()
    {
        int selectedBoardIndex = GetSelectedBoardIndex();
        
        switch (selectedBoardIndex)
        {
            case 0: // 4x4
                boardWidth = 4;
                boardHeight = 4;
                Debug.Log("Board: 4x4 selected");
                break;
            case 1: // 4x5
                boardWidth = 4;
                boardHeight = 5;
                Debug.Log("Board: 4x5 selected");
                break;
            case 2: // 5x5
                boardWidth = 5;
                boardHeight = 5;
                Debug.Log("Board: 5x5 selected");
                break;
            case 3: // 5x6
                boardWidth = 5;
                boardHeight = 6;
                Debug.Log("Board: 5x6 selected");
                break;
            default:
                boardWidth = 4;
                boardHeight = 4;
                break;
        }
    }

    // Helper method - reads which board is selected from shop system
   int GetSelectedBoardIndex()
{
    for (int i = 0; i < 4; i++)
    {
        int selected = PlayerPrefs.GetInt($"ShopItem_{i}_Selected", 0);

        Debug.Log($"Board {i} Selected = {selected}");

        if (selected == 1)
        {
            Debug.Log("Using board index: " + i);
            return i;
        }
    }

    Debug.Log("No board selected. Defaulting to 4x4");
    return 0;
}

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

   

    public void RestartFromPause()
    {
        Time.timeScale = 1f;
        RestartGame();
    }
    public void RestartFromContinue()
    {
        Time.timeScale = 1f;
        RestartGame();
    }

    private static int gameOverCount = 0;

    public void BackToMenu()
    {gameOverCount = 0;

        gameOverCount++;
        if (gameOverCount == 100)
        {
            Time.timeScale = 1f;
            gameOverCount = 0;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("First game over! Remaining tracks reset.");
        }
    }
    public void BackToMenuFromPause()
{gameOverCount = 0;

    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
}
void UpdateAllTileSizes()
{
    float cellWidth =
        boardRect
        .rect.width / boardWidth;

    float cellHeight =
        boardRect
        .rect.height / boardHeight;
       Debug.Log("Board Width: " + boardRect.rect.width);
Debug.Log("Board Height: " + boardRect.rect.height);

Debug.Log("Cell Width: " + cellWidth);
Debug.Log("Cell Height: " + cellHeight);

    for (int y = 0; y < boardHeight; y++)
    {
        for (int x = 0; x < boardWidth; x++)
        {
            if (tiles[x, y] == null)
                continue;

            RectTransform rt =
                tiles[x, y]
                .GetComponent<RectTransform>();

            rt.sizeDelta =
                new Vector2(
                    cellWidth - 8f,
                    cellHeight - 8f
                );

            rt.anchoredPosition =
                GetPosition(x, y);
        }
    }
}
void OnRectTransformDimensionsChange()
{
    if (tiles == null)
        return;

    UpdateAllTileSizes();
}
void OnApplicationPause(bool pause)
{
    if (pause)
        SaveGame();
}

void OnApplicationQuit()
{
    SaveGame();
}
void OnEnable()
{
    GameDistribution.OnRewardGame += RewardPlayerFromAd;
    GameDistribution.OnPauseGame += PauseGameForAd;
GameDistribution.OnResumeGame += ResumeGameAfterAd;
}

void OnDisable()
{
    GameDistribution.OnRewardGame -= RewardPlayerFromAd;
    GameDistribution.OnPauseGame -= PauseGameForAd;
GameDistribution.OnResumeGame -= ResumeGameAfterAd;
}

void RewardPlayerFromAd()
{
    gameOverCount++;

    continuePanel.SetActive(false);

    reviveUsed = true;

    RemoveRandomTiles(4);

    SpawnTile();

    SaveGame();

    GameDistribution.Instance.PreloadRewardedAd();
}void PauseGameForAd()
{
    Time.timeScale = 0f;
}

void ResumeGameAfterAd()
{
    Time.timeScale = 1f;
}
}
