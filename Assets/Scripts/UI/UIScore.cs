using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScore : MonoBehaviour
{
    public Slider scoreSlider;
    public Image star1;
    public Image star2;
    public Image star3;

    private LevelManager levelManager;
    private int maxScore;
    public bool shouldUpdateUIAfterAnimation = false;

    private Color originalFillColor; 

    private void Start()
    {
        levelManager = LevelManager.Instance;
    
    }

    public void InitializeScoreUI()
    {
        if (levelManager == null) return;

        maxScore = levelManager.GetMaxPossibleScore();
        scoreSlider.maxValue = maxScore;
        scoreSlider.value = 0;

        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;

        originalFillColor = scoreSlider.fillRect.GetComponent<Image>().color;

    }


    private void UpdateStars(int score)
    {
        star1.enabled = score >= maxScore * 0.42f;
        star2.enabled = score >= maxScore * 0.71f;
        star3.enabled = score >= maxScore * 0.98f;
    }

    public void AnimateScoreIncrease()
    {
        StopAllCoroutines();

        int oldScore = Mathf.RoundToInt(scoreSlider.value);
        int newScore = levelManager.GetCurrentScore();   

        StartCoroutine(AnimateScoreRoutine(oldScore, newScore));
    }

    private IEnumerator AnimateScoreRoutine(int oldScore, int newScore)
    {
        float duration = 0.5f;
        float t = 0f;

        Image fillImage = scoreSlider.fillRect.GetComponent<Image>();

        Color highlightColor = Color.Lerp(new Color(1f, 0.7f, 0.3f),originalFillColor, 0.7f); 

        // linearly
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            scoreSlider.value = Mathf.Lerp(oldScore, newScore, progress);

            UpdateStars(Mathf.RoundToInt(scoreSlider.value));

            yield return null;
        }
        scoreSlider.value = newScore;
        fillImage.color = highlightColor; 
        shouldUpdateUIAfterAnimation = true;
    }

    public void FinishAnimationAndRestore()
    {
        Image fillImage = scoreSlider.fillRect.GetComponent<Image>();
        fillImage.color = originalFillColor; 

        UpdateScoreUI(); 
        shouldUpdateUIAfterAnimation = false;
    }

    public void UpdateScoreUI()
    {
        if (levelManager == null) return;

        maxScore = levelManager.GetMaxPossibleScore();
        int currentScore = levelManager.GetCurrentScore();

        scoreSlider.maxValue = maxScore;
        scoreSlider.value = currentScore;
        UpdateStars(currentScore);
    }
}
