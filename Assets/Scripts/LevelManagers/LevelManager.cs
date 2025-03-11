using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public GameObject TelemetryManagerRef;

    [SerializeField] private List<GameObject> Levels = new List<GameObject>();
    [SerializeField] private int CurrentLevel = 0;
    [SerializeField] private GameObject EndMessage;

    public GameObject CurrentLevelObj { get; private set; }
    private GameObject PlayerRef;

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
        if (EndMessage != null)
        {
            EndMessage.SetActive(false);
        }
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Debug tool
        {
            NextLevel();
        }
    }

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
            CurrentLevel++;
            LoadLevel();
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (EndMessage != null)
        {
            EndMessage.transform.position = PlayerRef.transform.position + new Vector3(0, 2, 0);
            EndMessage.SetActive(true);
        }
    }

    public bool isTutorial()
    {
        return CurrentLevel == 0;
    }
}
