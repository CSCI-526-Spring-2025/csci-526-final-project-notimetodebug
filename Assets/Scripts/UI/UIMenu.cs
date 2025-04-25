using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject startPage;  
    public GameObject menuPanelPrefab; 
    
    private GameObject menuPanelInstance;
    private Transform buttonsParent; 

    [Header("Editor Mode Toggle")]
    public bool editorMode = true;

    private List<GameObject> levelPrefabs;
    private Dictionary<int, int> bestScores;
    private Dictionary<int, int> maxScores;


    private void Start()
    {
       // menuPanel.SetActive(false); 
        startPage.SetActive(true);   

        Transform buttonsGroup = startPage.transform.Find("layout/buttons");

        if (buttonsGroup != null)
        {
            Button startButton = buttonsGroup.Find("start").GetComponent<Button>();
            Button menuButton = buttonsGroup.Find("menu").GetComponent<Button>();

            startButton.onClick.AddListener(() => {
                LevelManager.Instance.LoadLevel(0);
                startPage.SetActive(false);
            });

            menuButton.onClick.AddListener(() => {
                startPage.SetActive(false);
                ShowMenu();
            });
        }
        else
        {
            Debug.LogError("button missing in start page");
        }

       
    

    }

    void OnStartButtonClicked()
    {
        if (editorMode || PlayerPrefs.HasKey("TutorialCompleted"))
        {
            startPage.SetActive(false);
            ShowMenu();
        }
        else
        {
            Debug.Log("Tutorial not completed, loading tutorial.");
            LevelManager.Instance.LoadLevel(0);
            startPage.SetActive(false);
        }
    }

    private void PopulateLevelButtons()
    {
        ClearButtons();

        levelPrefabs = LevelManager.Instance.GetLevels();
        bestScores = LevelManager.Instance.GetBestScores();

        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            GameObject buttonGO = Instantiate(levelButtonPrefab, buttonsParent);
            buttonGO.name = "LevelButton_" + i;

            TextMeshProUGUI text = buttonGO.transform.Find("Level#Text").GetComponent<TextMeshProUGUI>();
            string rawName = levelPrefabs[i].name;
            text.text = FormatLevelName(rawName);

            int bestScore = bestScores.ContainsKey(i) ? bestScores[i] : 0;
            maxScores = LevelManager.Instance.GetMaxScores();

            Debug.Log($"Level {i} ({rawName}) - BestScore: {bestScore}");
            int maxScore = maxScores.ContainsKey(i) ? maxScores[i] : 1000;

            Transform starContainer = buttonGO.transform.Find("StarContainer");

            if (bestScore > maxScore)
            {
                // super star
                SetStarColors(starContainer, 3, Color.blue);
            }
            else
            {
                int stars = CalculateStars(bestScore, maxScore);
                SetStarColors(starContainer, stars, Color.yellow);
            }

            int index = i;
            buttonGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                HideMenu();
                LevelManager.Instance.LoadLevel(index);
            });
        }
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonsParent)
        {
            Destroy(child.gameObject);
        }
    }

    private string FormatLevelName(string raw)
    {
        if (raw.StartsWith("Level_"))
        {
            string number = raw.Substring("Level_".Length);
            return number;
        }
        return raw;
    }

    private int CalculateStars(int score, int maxScore)
    {
        float t1 = maxScore * 0.42f;
        float t2 = maxScore * 0.71f;
        float t3 = maxScore * 0.98f;

        if (score >= t3) return 3;
        if (score >= t2) return 2;
        if (score >= t1) return 1;
        return 0;
    }

    private void SetStarColors(Transform starContainer, int filledCount, Color filledColor)
    {
        for (int i = 0; i < 3; i++)
        {
            Image starImage = starContainer.GetChild(i).GetComponent<Image>();
            starImage.color = i < filledCount ? filledColor : Color.gray;
        }
    }


    public void ShowMenu()
    {
       // menuPanel.SetActive(true);
        if (startPage != null)
            startPage.SetActive(false);

        if (menuPanelInstance == null)
        {
            menuPanelInstance = Instantiate(menuPanelPrefab, transform);
            buttonsParent = menuPanelInstance.transform.Find("Panel/LevelButtons");

            if (buttonsParent == null)
            {
                Debug.LogError("Could not find LevelButtons inside menuPanel prefab.");
                return;
            }
        }

        menuPanelInstance.SetActive(true);
        PopulateLevelButtons();
    }

    public void HideMenu()
    {
        if (menuPanelInstance != null)
            menuPanelInstance.SetActive(false);
    }
}
