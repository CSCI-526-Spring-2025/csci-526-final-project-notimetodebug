using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Levels = new List<GameObject>();
    [SerializeField] private int CurrentLevel = 0;
    [SerializeField] private GameObject EndMessage;

    private GameObject CurrentLevelObj;
    private GameObject PlayerRef;

    void Start()
    {
        PlayerRef = GameObject.FindWithTag("Player");
        LoadLevel();
    }

    // Switch to next level when pressing N
    // Debugging tool. Delete/Comment out when done testing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
    }

    private void LoadLevel()
    {
        if (CurrentLevelObj != null)
        {
            Destroy(CurrentLevelObj);
        }

        if (CurrentLevel >= Levels.Count || Levels[CurrentLevel] == null)
        {
            // Debug.LogError("Invalid level index or level prefab is null!");
            return;
        }

        CurrentLevelObj = Instantiate(Levels[CurrentLevel], Vector2.zero, Quaternion.identity);

        StartCoroutine(SetPlayerToLevelStart());
    }

    private IEnumerator SetPlayerToLevelStart()
    {
        yield return new WaitForEndOfFrame();

        Transform levelStart = CurrentLevelObj.transform.Find("LevelStart");
        if (levelStart != null)
        {
            PlayerRef.transform.position = levelStart.position;
        }
        else
        {
            // Debug.LogWarning("LevelStart not found in current level, setting player position to (0,0)");
            PlayerRef.transform.position = Vector2.zero;
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
        PlayerRef.SetActive(false);
        Instantiate(EndMessage, PlayerRef.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }
}
