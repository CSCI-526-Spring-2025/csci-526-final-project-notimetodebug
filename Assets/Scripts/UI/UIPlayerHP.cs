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

    public void BlinkDamageHPBar()
    {
        StartCoroutine(BlinkRed());
    }

    public void BlinkHealHPBar()
    {
        StartCoroutine(BlinkGreen());
    }

    private IEnumerator BlinkRed()
    {

        healthSlider.fillRect.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        healthSlider.fillRect.GetComponent<Image>().color = originalColor;
    }

    private IEnumerator BlinkGreen()
    {
        Color lightGreen = new Color(0.7f, 1f, 0.7f);
        healthSlider.fillRect.GetComponent<Image>().color = lightGreen;

        yield return new WaitForSeconds(0.1f);

        healthSlider.fillRect.GetComponent<Image>().color = originalColor;
    }

}
