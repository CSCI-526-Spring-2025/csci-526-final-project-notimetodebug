using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject challengeButtonPrefab; 
    public GameObject startPage;  
    public GameObject menuPanelPrefab; 
    
    private GameObject menuPanelInstance;
    private Transform buttonsParent; 

    [Header("Editor Mode Toggle")]
    public bool editorMode = true;

    private List<GameObject> levelPrefabs;
    private Dictionary<int, int> bestScores;
    private Dictionary<int, int> maxScores;

    [Header("Super Star Settings")]
    public Sprite superStarSprite;
    public Sprite normalStarSprite;



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
        maxScores = LevelManager.Instance.GetMaxScores();

        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            bool isLastLevel = (i == levelPrefabs.Count - 1);

            GameObject buttonGO = Instantiate(
            isLastLevel ? challengeButtonPrefab : levelButtonPrefab,
            buttonsParent
            );

            buttonGO.name = "LevelButton_" + i;

            TextMeshProUGUI text = buttonGO.transform.Find("Level#Text").GetComponent<TextMeshProUGUI>();
            string rawName = levelPrefabs[i].name;
            if (text != null && !isLastLevel)
            {
                text.text = FormatLevelName(rawName);
            }

           // int bestScore = bestScores.ContainsKey(i) ? bestScores[i] : 0;

            int bestScore = 0;
            int maxScore = 0;
            
            if (editorMode)
            {
                maxScore = 10;
                bestScore = 1000;  
            }
            else
            {
                bestScore = bestScores.ContainsKey(i) ? bestScores[i] : 0;
                maxScore = maxScores.ContainsKey(i) ? maxScores[i] : 1000;
            }

            Debug.Log($"Level {i} ({rawName}) - BestScore: {bestScore}");
            

            Transform starContainer = buttonGO.transform.Find("StarContainer");

            if (bestScore > maxScore)
            {
                SetStarColors(starContainer, 3, Color.yellow, useSuperStar: true);
            }
            else
            {
                int stars = CalculateStars(bestScore, maxScore);
                SetStarColors(starContainer, stars, Color.yellow, useSuperStar: false);
            }

            int index = i;
            Button btn = buttonGO.GetComponent<Button>();

            if (isLastLevel)
            {
                btn.onClick.AddListener(() =>
                {
                    if (AreAllPreviousLevelsCompleted())
                    {
                        HideMenu();
                        LevelManager.Instance.LoadLevel(index);
                    }
                    else
                    {
                        StartCoroutine(ShowHintTemporarily(buttonGO));
                    }
                });
            }
            else
            {
                btn.onClick.AddListener(() =>
                {
                    HideMenu();
                    LevelManager.Instance.LoadLevel(index);
                });
            }
        }
    }

    private bool AreAllPreviousLevelsCompleted()
    {
        if (editorMode)
        {
            return true;
        }
        for (int i = 0; i < levelPrefabs.Count - 1; i++)
        {
            if (!bestScores.ContainsKey(i) || bestScores[i] == 0)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator ShowHintTemporarily(GameObject challengeButton)
    {
        Transform text = challengeButton.transform.Find("Level#Text");
        Transform starContainer = challengeButton.transform.Find("StarContainer");
        Transform hint = challengeButton.transform.Find("hint");

        if (text != null && starContainer != null && hint != null)
        {
            text.gameObject.SetActive(false);
            starContainer.gameObject.SetActive(false);
            
            hint.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            text.gameObject.SetActive(true);
            starContainer.gameObject.SetActive(true);
            hint.gameObject.SetActive(false);
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

    private void SetStarColors(Transform starContainer, int filledCount, Color filledColor, bool useSuperStar = false)
    {
        for (int i = 0; i < 3; i++)
        {
            Image starImage = starContainer.GetChild(i).GetComponent<Image>();

            if (i < filledCount)
            {
                if (useSuperStar)
                {
                    starImage.sprite = superStarSprite;
                    starImage.color = Color.white; 
                }
                else
                {
                    starImage.sprite = normalStarSprite;
                    starImage.color = filledColor;
                }
            }
            else
            {
                starImage.sprite = normalStarSprite;
                starImage.color = Color.gray;
            }
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
