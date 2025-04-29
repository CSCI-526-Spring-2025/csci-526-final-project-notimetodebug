using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject pauseUIPrefab; 

    private GameObject spawnedPauseUI;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnPauseClicked);
    }

    private void OnPauseClicked()
    {
        if (spawnedPauseUI != null) return;

        spawnedPauseUI = Instantiate(pauseUIPrefab);
        Button resumeButton = spawnedPauseUI.transform.Find("PausePanel/ResumeButton").GetComponent<Button>();
        Button againButton = spawnedPauseUI.transform.Find("PausePanel/ButtonContainer/AgainButton").GetComponent<Button>();
        Button menuButton = spawnedPauseUI.transform.Find("PausePanel/ButtonContainer/MenuButton").GetComponent<Button>();

        resumeButton.onClick.AddListener(OnResumeClicked);
        againButton.onClick.AddListener(OnAgainClicked);
        menuButton.onClick.AddListener(OnMenuClicked);

        Time.timeScale = 0f; 
    }

    private void OnResumeClicked()
    {
        Time.timeScale = 1f;
        Destroy(spawnedPauseUI);
        spawnedPauseUI = null;
    }

    private void OnAgainClicked()
    {
        Time.timeScale = 1f;
        Destroy(spawnedPauseUI);
        spawnedPauseUI = null;
        LevelManager.Instance.LoadLevel();
    }

    private void OnMenuClicked()
    {
        Time.timeScale = 1f;
        Destroy(spawnedPauseUI);
        spawnedPauseUI = null;

        UIMenu menu = FindObjectOfType<UIMenu>();
        if (menu != null)
        {
            menu.ShowMenu();
        }
    }
}
