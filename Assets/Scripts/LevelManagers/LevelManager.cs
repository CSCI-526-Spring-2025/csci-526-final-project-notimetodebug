using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<GameObject> Levels = new List<GameObject>();
    [SerializeField] private int CurrentLevel = 0;

    [SerializeField] private GameObject uiLevelFailPrefab;
    private UILevelFail failUI;

    // Score variables
    private int currentScore = 0;
    private int enemyKillScore = 0;
    private int collectibleScore = 0;
    private int maxCollectibleScore = 0;
    private int maxEnemyKillScore = 0;
    private int maxPossibleScore = 0;
    private bool highHPBonus = false;
    private bool allEnemiesKilledBonus = false;
    private bool allCollectiblesCollectedBonus = false;

    // Bonus value is 10% of the max possible score
    private int bonusValue = 0;

    private Dictionary<int, int> bestScoreByLevel = new Dictionary<int, int>();
    private Dictionary<int, int> maxScoreByLevel = new Dictionary<int, int>();

    public GameObject CurrentLevelObj { get; private set; }
    public GameObject PlayerRef { get; private set; }

    public delegate void ScoreUpdated();
    public static event ScoreUpdated OnScoreUpdated;

    private bool isEndLevel = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerRef = GameObject.FindWithTag("Player");

        GameObject fail = Instantiate(uiLevelFailPrefab);
        fail.SetActive(false);
        failUI = fail.GetComponent<UILevelFail>();

        /*
        PlayerPrefs.DeleteKey("TutorialCompleted");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        */

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Debug tool
        {
            NextLevel();
        }
    }

    // may add parameters int levelIndex to load a specific level from main menu
    public void LoadLevel()
    {
        isEndLevel = false;
        if (CurrentLevelObj != null)
        {
            Destroy(CurrentLevelObj);
        }

        if (CurrentLevel >= Levels.Count || Levels[CurrentLevel] == null)
        {
            return;
        }

        // Initialize best score for current level if it doesn't exist
        if (!bestScoreByLevel.ContainsKey(CurrentLevel))
        {
            bestScoreByLevel[CurrentLevel] = 0;
        }


        CurrentLevelObj = Instantiate(Levels[CurrentLevel], Vector2.zero, Quaternion.identity);
        
        StartCoroutine(SetPlayerToLevelStart());

        TelemetryManager.Log(TelemetryManager.EventName.LEVEL_START, CurrentLevelObj.name);

        enemyKillScore = 0; 
        collectibleScore = 0;
        currentScore = 0;

        highHPBonus = false;
        allEnemiesKilledBonus = false;
        allCollectiblesCollectedBonus = false;

        bonusValue = (int)(maxPossibleScore * 0.1);
        Player player = FindObjectOfType<Player>();
        if (player != null){
            player.ResetToDefaultGun();
            UpdateWeaponIndicatorUI(player);
        }

        StartCoroutine(CountEnemiesNextFrame());
    }

    private IEnumerator CountEnemiesNextFrame(){
        yield return new WaitForEndOfFrame(); 

        maxEnemyKillScore = 0;
        maxCollectibleScore = 0;
        maxPossibleScore = 0;

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            int enemyScore = enemy.GetScoreValue();
            maxEnemyKillScore += enemyScore;
        }

        foreach (var star in FindObjectsOfType<CollectibleStar>())
        {
            maxCollectibleScore += star.points;
        }

        maxPossibleScore = maxEnemyKillScore + maxCollectibleScore;
        bonusValue = (int)(maxPossibleScore * 0.1);

        maxScoreByLevel[CurrentLevel] = maxPossibleScore;

        OnScoreUpdated?.Invoke();
    }

    private void UpdateWeaponIndicatorUI(Player player)
    {
        UIWeaponIndicator weaponIndicatorUI = FindObjectOfType<UIWeaponIndicator>();
        if (weaponIndicatorUI != null && player.guns.Count > 0)
        {
            weaponIndicatorUI.SetGun(player.guns[0]); 
            weaponIndicatorUI.UpdateWeaponIndicator(false);
        }
    }

    public void CheckHPBonus(){
        Player player = PlayerRef.GetComponent<Player>();
        if (player == null) return;
        if(player.HP >= 80){
            highHPBonus = true;
        }
    }

    public void CheckAllEnemiesKilledBonus(){
        if (FindObjectsOfType<Enemy>().Length == 0){
            allEnemiesKilledBonus = true;
        }
    }

    public void CheckAllCollectiblesCollectedBonus(){
        if (FindObjectsOfType<CollectibleStar>().Length == 0){
            allCollectiblesCollectedBonus = true;
        }
    }

    public void StopUpdatingScore()
    {
        isEndLevel = true;
    }

    public void AddEnemyKillScore(int score)
    {
        if (isEndLevel) return; 
        enemyKillScore += score;
        currentScore = GetCurrentScore();
        Debug.Log("Killed an enemy, increasing enemyKillScore by " + score + ", current enemyKillScore: " + enemyKillScore);
        
        OnScoreUpdated?.Invoke();
    }

    public void AddCollectibleScore(int score)
    {
        if (isEndLevel) return;
        collectibleScore += score;
        currentScore = GetCurrentScore();
        Debug.Log("Collected a star, increasing collectibleScore by " + score + ", current collectibleScore: " + collectibleScore);
        OnScoreUpdated?.Invoke();
    }

    private IEnumerator SetPlayerToLevelStart()
    {
        yield return new WaitForEndOfFrame();

        if (CurrentLevelObj == null)
        {
            Debug.LogError("CurrentLevelObj is null! The level was not instantiated properly.");
            yield break;
        }

        if (PlayerRef == null)
        {
            Debug.LogError("Player reference not found!");
            yield break;
        }

        Transform levelStart = CurrentLevelObj.transform.Find("LevelStart");

        if (levelStart != null)
        {
            PlayerRef.transform.position = levelStart.position;
        }
        else
        {
            Debug.LogWarning("LevelStart not found in current level, setting player position to (0,0)");
            PlayerRef.transform.position = Vector2.zero;
        }

        ResetPlayerState();
    }

    public void RespawnPlayer()
    {
        LoadLevel();
        ResetPlayerState();
    }

    private void ResetPlayerState()
    {
        Creature playerCreature = PlayerRef.GetComponent<Creature>();
        if (playerCreature != null)
        {
            playerCreature.HP = playerCreature.maxHP;
        }

        UIPlayerHP playerHPUI = FindObjectOfType<UIPlayerHP>();
        if (playerHPUI != null)
        {
            playerHPUI.UpdateHealth(playerCreature.HP);
        }

        Rigidbody2D rb = PlayerRef.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        Player player = PlayerRef.GetComponent<Player>();
        if (player != null && isTutorial())
        {
            if (player.guns.Count > 1)
            {
                player.currentGunIndex = 0;
                player.guns[0].OnEquipped();
                player.guns[1].Destroy();
                player.guns.RemoveAt(1);
            }
        }
    }

    public void NextLevel()
    {
        if (CurrentLevel < Levels.Count - 1)
        {
            if (!bestScoreByLevel.ContainsKey(CurrentLevel)){
                bestScoreByLevel[CurrentLevel] = 0;
            }
            else if (currentScore > bestScoreByLevel[CurrentLevel])
            {
                bestScoreByLevel[CurrentLevel] = currentScore;
            }
            
            CurrentLevel++;
            LoadLevel();
        }
    }

    public void ShowLevelFailUI()
    {
        if (failUI != null)
        {
            failUI.Show();
        }
        else
        {
            Debug.LogError("Level Fail UI is missing!");
        }
    }


    private void EndGame()
    {
        //make it actual terminate everything

    }

    public bool isTutorial()
    {
        return CurrentLevel == 0;
    }

    public int GetCurrentScore(){

        currentScore = enemyKillScore + collectibleScore + (highHPBonus ? bonusValue : 0) + (allEnemiesKilledBonus ? bonusValue : 0) + (allCollectiblesCollectedBonus ? bonusValue : 0);
        return currentScore;
    }

    public int GetCurrentLevel(){
        return CurrentLevel;
    }

    public int GetMaxPossibleScore(){
        return maxPossibleScore;
    }

    public Dictionary<string, int> GetScoreBreakdown(){
        currentScore = enemyKillScore + collectibleScore + (allEnemiesKilledBonus ? bonusValue : 0) + (highHPBonus ? bonusValue : 0) + (allCollectiblesCollectedBonus ? bonusValue : 0);
        Dictionary<string, int> breakdown = new Dictionary<string, int>(){
            {"Final score:", currentScore},
            {"Kills:", enemyKillScore},
            {"Collectibles:", collectibleScore},
            {"High HP bonus:", highHPBonus ? bonusValue : 0},
            {"Kills bonus:", allEnemiesKilledBonus ? bonusValue : 0},
            {"Collector bonus:", allCollectiblesCollectedBonus ? bonusValue : 0}
        };
        return breakdown;
    }

    public List<GameObject> GetLevels() => Levels;
    public Dictionary<int, int> GetBestScores() => bestScoreByLevel;
    public Dictionary<int, int> GetMaxScores() => maxScoreByLevel;

    public void LoadLevel(int index)
    {
        CurrentLevel = index;
        LoadLevel();
    }






}
