using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingCollectible : MonoBehaviour
{
    [SerializeField] private int healingAmount = 20;
    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] private GameObject pickupEffect;
    
    [Header("Healing Text")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Color healTextColor = Color.green;
    [SerializeField] private float textFloatSpeed = 1.0f;
    [SerializeField] private float textDuration = 2.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.GetComponent<Player>();
        
        if (player != null)
        {

            HealPlayer(player);

            TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, "Healing");
            
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }
            
            ShowHealingText(player.transform.position);
            
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void HealPlayer(Player player)
    {
        player.Heal(healingAmount);
    }
    
    private void ShowHealingText(Vector3 position)
    {
        if (floatingTextPrefab != null)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();
            
            if (textMesh != null)
            {
                textMesh.text = "+" + healingAmount;
                textMesh.color = healTextColor;
                StartCoroutine(AnimateFloatingText(textObj));
            }
            else
            {
                TextMeshProUGUI textMeshUI = textObj.GetComponent<TextMeshProUGUI>();
                if (textMeshUI != null)
                {
                    textMeshUI.text = "+" + healingAmount;
                    textMeshUI.color = healTextColor;
                    StartCoroutine(AnimateFloatingText(textObj));
                }
                else
                {
                    Debug.LogWarning("Floating text prefab doesn't have TextMeshPro or TextMeshProUGUI component!");
                    Destroy(textObj);
                }
            }
        }
        else
        {
            Debug.LogWarning("Floating text prefab is not assigned!");
        }
    }
    
    private IEnumerator AnimateFloatingText(GameObject textObj)
    {
        float startTime = Time.time;
        Vector3 startPos = textObj.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 1.0f, 0); 
        
        while (Time.time < startTime + textDuration)
        {
            float t = (Time.time - startTime) / textDuration;
            
            // Move upward
            textObj.transform.position = Vector3.Lerp(startPos, targetPos, t * textFloatSpeed);
            
            // Fade out near the end
            if (t > 0.5f)
            {
                TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();
                if (textMesh != null)
                {
                    Color color = textMesh.color;
                    color.a = 1 - ((t - 0.5f) * 2);
                    textMesh.color = color;
                }
                else
                {
                    TextMeshProUGUI textMeshUI = textObj.GetComponent<TextMeshProUGUI>();
                    if (textMeshUI != null)
                    {
                        Color color = textMeshUI.color;
                        color.a = 1 - ((t - 0.5f) * 2);
                        textMeshUI.color = color;
                    }
                }
            }
            
            yield return null;
        }
        
        Destroy(textObj);
    }
}