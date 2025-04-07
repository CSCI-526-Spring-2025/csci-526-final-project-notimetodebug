using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public Slider scoreSlider;
    public Image star1;
    public Image star2;
    public Image star3;
    
    private LevelManager levelManager;
    private int maxScore;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        if (levelManager != null)
        {
            maxScore = levelManager.GetMaxPossibleScore();
            UpdateScoreUI();
        }
        else
        {
            Debug.LogError("UIScore: LevelManager is NULL!");
        }
    }

    private void UpdateStars(int score)
    {
        star1.enabled = score >= maxScore * 0.42f;
        star2.enabled = score >= maxScore * 0.71f;
        star3.enabled = score >= maxScore * 0.98f;
    }

    public void UpdateScoreUI()
    {
        if (levelManager == null) return;

        maxScore = levelManager.GetMaxPossibleScore();
        int currentScore = levelManager.GetCurrentScore();
        
        scoreSlider.maxValue = maxScore;
        scoreSlider.value = currentScore;

        Debug.Log($"UIScore: Updated Score UI, Max Score = {maxScore}, Current Score = {currentScore}");
        UpdateStars(currentScore);
    }

    private void OnEnable()
    {
        LevelManager.OnScoreUpdated += UpdateScoreUI;
    }

    private void OnDisable()
    {
        LevelManager.OnScoreUpdated -= UpdateScoreUI;
    }

}
