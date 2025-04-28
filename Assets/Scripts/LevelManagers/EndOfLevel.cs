using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private bool levelComplete = false;
    private UIEndLevel uiEndLevel;

    public GameObject uiEndLevelPrefab; 

    private void Start()
    {
        GameObject end = Instantiate(uiEndLevelPrefab);
        end.SetActive(false); 
        uiEndLevel = end.GetComponent<UIEndLevel>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelComplete) return;

        if (other.gameObject.CompareTag("Player")
        || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            levelComplete = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    private IEnumerator EndLevelSequence()
    {
        LevelManager.Instance.CheckHPBonus();
        LevelManager.Instance.CheckAllEnemiesKilledBonus();
        LevelManager.Instance.CheckAllCollectiblesCollectedBonus();

        LevelManager.Instance.StopUpdatingScore();

        int finalScore = LevelManager.Instance.GetCurrentScore();
        int levelIndex = LevelManager.Instance.GetCurrentLevel();
        Dictionary<int, int> bestScores = LevelManager.Instance.GetBestScores();
        
        if (!bestScores.ContainsKey(levelIndex) || finalScore > bestScores[levelIndex])
        {
            bestScores[levelIndex] = finalScore;
            Debug.Log($"Updated BestScore for level {levelIndex} to {finalScore}");
        }

        uiEndLevel.ShowEndLevelUI(); 

        yield break; 
    }
}

