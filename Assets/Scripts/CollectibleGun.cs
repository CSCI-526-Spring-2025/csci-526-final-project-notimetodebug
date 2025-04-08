using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CollectibleGun : MonoBehaviour
{
    [Header("Gun Properties")]
    public Gun gunPrefab; 
    
    [Header("Floating Animation")]
    public float bobHeight = 0.5f;
    public float bobSpeed = 3.0f;
    public float rotationSpeed = 10.0f;
    
    private Vector3 startPosition;
    private bool isCollected = false;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        if (!isCollected)
        {
            // Bobbing up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected)
            return;
        
        Player player = other.GetComponent<Player>();
        
        if (player != null)
        {
            isCollected = true;
            
            // Get the position of this collectible 
            Vector3 collectiblePosition = transform.position;
            
            
            if (player.guns.Count > 1)
            {
                Gun oldGun = player.guns[1];
                
                bool wasEquipped = (player.currentGunIndex == 1);
                
                
                if (wasEquipped)
                {
                    oldGun.OnUnequipped();
                }
                
                
                if (GunManager.Instance != null)
                {
                    GunManager.Instance.CreateCollectibleFromGun(oldGun, collectiblePosition);
                }
              
                oldGun.Destroy();
                player.guns.RemoveAt(1);
                
        
                if (wasEquipped)
                {
                    player.currentGunIndex = 0;
                    player.guns[0].OnEquipped();
                }
            }
            
   
            Gun newGun = Instantiate(gunPrefab, player.transform);
            
            if (newGun.transform != null)
            {
                newGun.transform.localPosition = Vector3.zero;
                newGun.transform.localRotation = Quaternion.identity;
            }
            
    
            player.PickUpGun(newGun);
            
  
            SetVisible(false);
            
            TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, $"Gun: {gunPrefab.name}");

         
            Destroy(gameObject, 0.1f);
        }
    }
    
    private void SetVisible(bool visible)
    {

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = visible;
        }
        
       
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in childRenderers)
        {
            r.enabled = visible;
        }
     
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = visible;
        }
    }
}