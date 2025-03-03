using TMPro;
using UnityEngine;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialFollow tutorialFollow;

    private readonly string[] tutorialSteps = 
    {
        "Welcome to the tutorial! Left click to shoot and move around.",
        "Good job! Now defeat your first enemy.",
        "Awesome! Now collect your new gun.",
        "Switch guns by pressing Q. ",
        "Try your new gun out! Be aware of the bouncing bullets!",
        "You can utilize the walls to bounce the bullets to the enemy.",
        "Wonderful! You've completed the tutorial! Now enjoy the game!",
        " ",
        "You have finished our prototype! Thank you for playing!",
    };

    private int stepIndex = 0;

    void Start()
    {
        tutorialFollow.UpdateText(tutorialSteps[stepIndex]); // 初始化教程文本
    }

    public void NextStep()
    {
        if (stepIndex < tutorialSteps.Length - 1)
        {
            stepIndex++;
            tutorialFollow.UpdateText(tutorialSteps[stepIndex]); // 更新教程
        }
        else
        {
            tutorialFollow.UpdateText(""); // 清空教程文字
        }
    }
}
