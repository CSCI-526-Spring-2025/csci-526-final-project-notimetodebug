using UnityEngine;
using UnityEngine.UI;

public class UIHelp : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject helpUIPrefab;

    private GameObject spawnedHelpUI;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnHelpClicked);
    }

    private void OnHelpClicked()
    {
        if (spawnedHelpUI != null) return;

        spawnedHelpUI = Instantiate(helpUIPrefab);

        Transform mouseClick = spawnedHelpUI.transform.Find("HelpPanel/keys/MoveShot/mouseClick");
        if (mouseClick != null)
        {
            Animator animator = mouseClick.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("mouseClick"); 
            }
        }

        Button closeButton = spawnedHelpUI.transform.Find("HelpPanel/closeButton").GetComponent<Button>();
        closeButton.onClick.AddListener(CloseHelpUI);
        Time.timeScale = 0f; 
        
    }

    private void CloseHelpUI()
    {
        Time.timeScale = 1f;
        Destroy(spawnedHelpUI);
        spawnedHelpUI = null;
    }
}
