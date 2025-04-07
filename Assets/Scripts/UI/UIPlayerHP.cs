using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections;


public class UIPlayerHP : MonoBehaviour
{
    public Slider healthSlider; 
    public TextMeshProUGUI hpText;
    
    private Color originalColor;
    private void Start()
    {
        originalColor = healthSlider.fillRect.GetComponent<Image>().color;
    }

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

        float hpPercentage = currentHealth / healthSlider.maxValue;

     /*   if (hpPercentage < 0.2f){
            healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        } else if (hpPercentage < 0.5f){
            healthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        } else {
            healthSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
        */
    }

    public void BlinkHPBar()
    {
        StartCoroutine(BlinkHP());
    }

    private IEnumerator BlinkHP()
    {

        healthSlider.fillRect.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        healthSlider.fillRect.GetComponent<Image>().color = originalColor;
    }

}
