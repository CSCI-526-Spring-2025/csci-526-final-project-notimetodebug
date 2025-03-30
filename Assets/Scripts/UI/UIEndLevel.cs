using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndLevel : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI enemyKillText;
    public TextMeshProUGUI hpRemainingText;
    public TextMeshProUGUI fullHPBonusText;
    public TextMeshProUGUI allEnemiesKilledBonusText;
    public GameObject scoreBreakdownPanel; 
    public Button againButton;
    public Button menuButton;
    public Button nextLevelButton;

    public Image star1;
    public Image star2;
    public Image star3;
    
    public Color filledColor = Color.yellow;  
    public Color unfilledColor = Color.grey; 
    public Color superColor = Color.blue; 

    private int currentTotalScore = 0;

    private void Start()
    {
        againButton.onClick.AddListener(RestartLevel);
        nextLevelButton.onClick.AddListener(NextLevel);
        menuButton.onClick.AddListener(ReturnToMenu);
    }

    public void ShowEndLevelUI()
    {
        gameObject.SetActive(true); 
        if (LevelManager.Instance.GetCurrentLevel() >= LevelManager.Instance.GetLevels().Count - 1)
        {
            nextLevelButton.gameObject.SetActive(false);
        }
        else
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        StartCoroutine(AnimateScoreBreakdown());
    }

    private IEnumerator AnimateScoreBreakdown()
    {
        int maxPossibleScore = LevelManager.Instance.GetMaxPossibleScore(); 
        Dictionary<string, int> breakdown = LevelManager.Instance.GetScoreBreakdown();

        int regularScore = breakdown["Kills:"] + breakdown["Collectibles:"];
        int finalScore = breakdown["Final score:"];
        currentTotalScore = 0; 

        float t1 = maxPossibleScore * 0.42f;
        float t2 = maxPossibleScore * 0.71f;
        float t3 = maxPossibleScore * 0.98f;        

        // Hide all at start
        hpRemainingText.gameObject.SetActive(false);
        enemyKillText.gameObject.SetActive(false);
        fullHPBonusText.gameObject.SetActive(false);
        allEnemiesKilledBonusText.gameObject.SetActive(false);
        //finalScoreText.gameObject.SetActive(false);

        finalScoreText.gameObject.SetActive(true); 
        finalScoreText.text = "Final Score: 0";

        ResetStars();

        // Animate categories one by one
        if (breakdown["Kills:"] > 0)
        {
            yield return StartCoroutine(AnimateText(enemyKillText, "Enemy kills: ", breakdown["Kills:"]));
        } else {
            yield return StartCoroutine(AnimateText(enemyKillText, "Enemy kills: ", 0));
        }

        // if (breakdown["Collectibles:"] > 0)
        // {
        //     yield return StartCoroutine(AnimateText(collectibleText, "Collectibles: ", breakdown["Collectibles:"]));
        // } else {
        //     yield return StartCoroutine(AnimateText(collectibleText, "Collectibles: ", 0));
        // }

        int starsFilled = 0;
        if (regularScore >= t1)
        {
            yield return StartCoroutine(FillStar(star1));
            starsFilled++;
        }
        if (regularScore >= t2)
        {
            yield return StartCoroutine(FillStar(star2));
            starsFilled++;
        }
        if (regularScore >= t3)
        {
            yield return StartCoroutine(FillStar(star3));
            starsFilled++;
        }

        yield return new WaitForSeconds(0.5f);

        if (breakdown["High HP bonus:"] > 0)
        {
            yield return StartCoroutine(AnimateText(fullHPBonusText, "HIGH HP BONUS: ", breakdown["High HP bonus:"]));
        }

        if (breakdown["Kills bonus:"] > 0)
        {
            yield return StartCoroutine(AnimateText(allEnemiesKilledBonusText, "KILLER BONUS: ", breakdown["Kills bonus:"]));
        }

        // if (breakdown["Collectibles:"] > 0)
        // {
        //     yield return StartCoroutine(AnimateText(collectibleText, "Collectibles: ", breakdown["Collectibles:"]));
        // }

        if (finalScore > regularScore)
        {
            if (finalScore >= t1 && starsFilled < 1)
            {
                yield return StartCoroutine(FillStar(star1));
            }
            if (finalScore >= t2 && starsFilled < 2)
            {
                yield return StartCoroutine(FillStar(star2));
            }
            if (finalScore >= t3 && starsFilled < 3)
            {
                yield return StartCoroutine(FillStar(star3));
            }
        }

        if (finalScore > maxPossibleScore)
        {
            star1.color = superColor;
            star2.color = superColor;
            star3.color = superColor;
        }
        
    }


    private IEnumerator AnimateText(TextMeshProUGUI textComponent, string prefix, int targetValue)
    {
        textComponent.gameObject.SetActive(true);

        int currentValue = 0;
        float duration = 2f;
        float stepTime = duration / Mathf.Max(targetValue, 1); // prevent div by 0

        while (currentValue < targetValue)
        {
            currentValue += Mathf.CeilToInt(targetValue / 20f);
            currentValue = Mathf.Min(currentValue, targetValue);

            textComponent.text = prefix + currentValue;
            finalScoreText.text = "Final Score: " + (currentTotalScore + currentValue);

            yield return new WaitForSeconds(stepTime);
        }

        // Final update
        textComponent.text = prefix + targetValue;
        currentTotalScore += targetValue;
        finalScoreText.text = "Final Score: " + currentTotalScore;

        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator FillStar(Image star)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            star.color = Color.Lerp(unfilledColor, filledColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        star.color = filledColor;
    }

    private IEnumerator FillAllStars()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            star1.color = Color.Lerp(unfilledColor, superColor, elapsed / duration);
            star2.color = Color.Lerp(unfilledColor, superColor, elapsed / duration);
            star3.color = Color.Lerp(unfilledColor, superColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        star1.color = superColor;
        star2.color = superColor;
        star3.color = superColor;
    }

    private void ResetStars()
    {
        star1.color = unfilledColor;
        star2.color = unfilledColor;
        star3.color = unfilledColor;
    }

    private void RestartLevel()
    {   
        Debug.Log("restart button clicked.");
        gameObject.SetActive(false);
        LevelManager.Instance.LoadLevel();
    }

    private void NextLevel()
    {
        Debug.Log("Next Level button clicked.");
        gameObject.SetActive(false); 
        LevelManager.Instance.NextLevel();
    }

    private void ReturnToMenu()
    {
        Debug.Log("Return to menu clicked.");
        gameObject.SetActive(false); 
        UIMenu menu = FindObjectOfType<UIMenu>();
        if (menu != null)
        {
            menu.ShowMenu();
        }
    }
}
