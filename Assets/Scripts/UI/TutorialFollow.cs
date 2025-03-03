using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialFollow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText; 
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0); 
    private Player player;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

    }

    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        if (player != null && tutorialText != null)
        {
            Vector3 worldPosition = player.transform.position + offset; 
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition); 
            tutorialText.transform.position = screenPosition; 
        }
    }

    public void UpdateText(string newText)
    {
        tutorialText.text = newText;
    }
}
