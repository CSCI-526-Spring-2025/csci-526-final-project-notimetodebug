using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UIPlayerHP : MonoBehaviour
{
    public Slider healthSlider; 
    public TextMeshProUGUI hpText; 

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        UpdateHealth(maxHealth);
    }

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
        hpText.text = currentHealth + " / " + healthSlider.maxValue; 
    }
}
