using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private bool levelComplete = false;

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
        LevelManager.Instance.AddHPRemainingScore();
        LevelManager.Instance.CheckAllEnemiesKilledBonus();

        Dictionary<string, int> breakdown = LevelManager.Instance.GetScoreBreakdown();
        Debug.Log("Score breakdown: " + breakdown);

        foreach (var item in breakdown)
        {
            Debug.Log(item.Key + " " + item.Value);
        }

        //TODO: add UI to display score breakdown


        yield return new WaitUntil(() => Input.anyKeyDown);

        LevelManager.Instance.NextLevel();
    }
}

