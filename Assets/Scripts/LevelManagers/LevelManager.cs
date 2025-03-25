using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //TODO: add collectible score logic when we have them
    public static LevelManager Instance { get; private set; }
    public GameObject TelemetryManagerRef;

    [SerializeField] private List<GameObject> Levels = new List<GameObject>();
    [SerializeField] private int CurrentLevel = 0;

    [SerializeField] private GameObject uiLevelFailPrefab;
    private UILevelFail failUI;

    private int currentScore = 0;
    private int maxPossibleScore = 100; // user HP
    private int maxEnemyKillScore = 0;
    private int enemyKillScore = 0;
    private int hpRemainingScore = 0;
    private bool perfectHPBonus = false;
    private bool allEnemiesKilled = false;
    // TODO: add score for collectibles when we have them

    // private int collectibleScore = 0;
    private Dictionary<int, int> scoreByLevel = new Dictionary<int, int>();
    private Dictionary<int, int> maxScoreByLevel = new Dictionary<int, int>();

    public GameObject CurrentLevelObj { get; private set; }
    public GameObject PlayerRef { get; private set; }

    // Static variables
    [SerializeField] private int perfectHPBonusValue = 50;
    [SerializeField] private int allEnemiesKilledBonusValue = 100;

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
        

        //TODO: add main menu, loaded when first opening the game
        // LoadMainMenu();
        LoadLevel();
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
        if (CurrentLevelObj != null)
        {
            Destroy(CurrentLevelObj);
        }

        if (CurrentLevel >= Levels.Count || Levels[CurrentLevel] == null)
        {
            return;
        }

        CurrentLevelObj = Instantiate(Levels[CurrentLevel], Vector2.zero, Quaternion.identity);
        
        StartCoroutine(SetPlayerToLevelStart());

        TelemetryManagerRef.GetComponent<TelemetryManager>().Log(TelemetryManager.EventName.LEVEL_START, CurrentLevelObj.name);
        
        maxEnemyKillScore = 0;
        maxPossibleScore = 100;
        enemyKillScore = 0; 
        hpRemainingScore = 0;

        foreach (var enemy in FindObjectsOfType<Enemy>()){
            int enemyScore = enemy.GetScoreValue(); 
            maxEnemyKillScore += enemyScore;
        }
        maxPossibleScore += maxEnemyKillScore;
        Debug.Log($"[DEBUG] Level {CurrentLevel}: Player HP = 100, Total Enemy Score = {maxEnemyKillScore}, Max Possible Score = {maxPossibleScore}");

        hpRemainingScore = GetHPScore(); // Just fetch the player's current HP
        currentScore = GetCurrentScore();

        // reset player gun when loading a new level
        Player player = FindObjectOfType<Player>();
        if (player != null){
            player.ResetToDefaultGun();
            UpdateWeaponIndicatorUI(player);
        }

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

    public void CheckPerfectHPBonus(){
        Player player = PlayerRef.GetComponent<Player>();
        if (player == null) return;
        hpRemainingScore = player.HP;
        if(hpRemainingScore == 100){
            perfectHPBonus = true;
        }
    }

    public void CheckAllEnemiesKilledBonus(){
        if (FindObjectsOfType<Enemy>().Length == 0){
            allEnemiesKilled = true;
        }
    }

    public void StopUpdatingScore()
    {
        isEndLevel = true;
    }

    public void AddScore(int score)
    {
        if (isEndLevel) return; 
        enemyKillScore += score;
        currentScore = GetCurrentScore();
        Debug.Log("Killed an enemy, increasing enemyKillScore by " + score + ", current enemyKillScore: " + enemyKillScore);
        Debug.Log($"Current Score: {GetCurrentScore()}, Max Score: {GetMaxPossibleScore()}");
        
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
        Debug.Log("Respawning Player...");
        LoadLevel();
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
            scoreByLevel[CurrentLevel] = currentScore;
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
    public int GetHPScore()
    {   
        if (PlayerRef == null)
        {
            return 0;
        }
        Player player = PlayerRef.GetComponent<Player>();
        if (player != null)
        {
            return player.HP; 
        }
        return 0;  
    }

    public int GetCurrentScore(){

        hpRemainingScore = GetHPScore();
        currentScore = enemyKillScore + hpRemainingScore;
        Debug.Log($"LevelManager: Updated Current Score = {currentScore}");
        return currentScore;
    }

    public int GetCurrentLevel(){
        return CurrentLevel;
    }

    public int GetMaxPossibleScore(){
        return maxPossibleScore;
    }

    public Dictionary<string, int> GetScoreBreakdown(){
        currentScore = enemyKillScore + hpRemainingScore + (allEnemiesKilled ? allEnemiesKilledBonusValue : 0) + (perfectHPBonus ? perfectHPBonusValue : 0);
        Dictionary<string, int> breakdown = new Dictionary<string, int>(){
            {"Final score:", currentScore},
            {"Enemy kill score:", enemyKillScore},
            {"HP Remaining score:", hpRemainingScore},
            {"All enemies killed bonus:", allEnemiesKilled ? allEnemiesKilledBonusValue : 0},
            {"Perfect HP bonus:", perfectHPBonus ? perfectHPBonusValue : 0}
        };
        return breakdown;
    }



}
