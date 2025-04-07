using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCollectible : MonoBehaviour
{
    [SerializeField] private int healingAmount = 20;
    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] private GameObject pickupEffect;
    
    // Bobbing effect parameters
    [Header("Bobbing Effect")]
    [SerializeField] private float bobbingHeight = 0.2f;
    [SerializeField] private float bobbingSpeed = 2f;
    
    private Vector3 startPosition;
    
    private void Start()
    {
        startPosition = transform.position;
    }
    
    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        
        if (player != null)
        {
            HealPlayer(player);
            
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