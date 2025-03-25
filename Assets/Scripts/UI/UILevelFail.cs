using UnityEngine;
using UnityEngine.UI;

public class UILevelFail : MonoBehaviour
{
    public Button againButton;
    public Button menuButton;

    private void Start()
    {
        againButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(ReturnToMenu);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void RestartLevel()
    {   
        Debug.Log("restart button clicked.");
        gameObject.SetActive(false);
        LevelManager.Instance.LoadLevel();
    }

    private void ReturnToMenu()
    {
        Debug.Log("Return to menu clicked.");
        // Implement main menu logic here
    }
}
