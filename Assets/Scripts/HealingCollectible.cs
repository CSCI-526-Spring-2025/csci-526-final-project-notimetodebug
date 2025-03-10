using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCollectible : MonoBehaviour
{
    [SerializeField] private int healingAmount = 20;
    [SerializeField] private bool destroyOnPickup = true;

    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        Player player = collision.GetComponent<Player>();
        
        if (player != null)
        {
            // Heal the player
            HealPlayer(player);
            

            // Spawn pickup effect if needed in future
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }
            
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void HealPlayer(Player player)
    {

        int currentHP = player.HP;
        int maxHP = player.maxHP;
        
        int newHP = Mathf.Min(currentHP + healingAmount, maxHP);
        
        player.HP = newHP;
        
        FindObjectOfType<UIPlayerHP>()?.UpdateHealth(newHP);
    }
}