using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;


// public class CollectibleGun : MonoBehaviour
// {
//     [Header("Gun Properties")]
//     public Gun gunPrefab; 
    
//     [Header("Floating Animation")]
//     public float bobHeight = 0.5f;
//     public float bobSpeed = 3.0f;
//     public float rotationSpeed = 10.0f;
    
//     private Vector3 startPosition;
//     private bool isCollected = false;
    
//     void Start()
//     {
//         startPosition = transform.position;
//     }
    
//     void Update()
//     {

//         if (!isCollected)
//         {
//             // Bobbing up and down
//             float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
//             transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            
//             transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
//         }
//     }
    
//     void OnTriggerEnter2D(Collider2D other)
//     {

//         if (isCollected)
//             return;
        
//         Player player = other.GetComponent<Player>();
        
//         if (player != null)
//         {
//             isCollected = true;
            
//             // Instantiate the gun prefab as a child of the player
//             Gun newGun = Instantiate(gunPrefab, player.transform);
            
//             if (newGun.transform != null)
//             {
//                 newGun.transform.localPosition = Vector3.zero;
//                 newGun.transform.localRotation = Quaternion.identity;
//             }
            
//             player.PickUpGun(newGun);
            
//             Renderer renderer = GetComponent<Renderer>();
//             if (renderer != null)
//             {
//                 renderer.enabled = false;
//             }
            
//             // Disable any child renderers
//             Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
//             foreach (Renderer r in childRenderers)
//             {
//                 r.enabled = false;
//             }
            
//             // Disable the collider
//             Collider2D collider = GetComponent<Collider2D>();
//             if (collider != null)
//             {
//                 collider.enabled = false;
//             }

//             TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, $"Gun: {gunPrefab.name}");

//             // Destroy the collectible
//             Destroy(gameObject, 0.1f);
//         }
//     }
// }

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
    
    // For dropped guns, optionally store additional properties 
    // private int currentAmmo;
    
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
            
            // Instantiate the gun prefab as a child of the player
            Gun newGun = Instantiate(gunPrefab, player.transform);
            
            if (newGun.transform != null)
            {
                newGun.transform.localPosition = Vector3.zero;
                newGun.transform.localRotation = Quaternion.identity;
            }
            
            // Transfer any special properties to the gun if this was a dropped gun
            // newGun.currentAmmo = currentAmmo;
            
            player.PickUpGun(newGun);
            
            // Hide the collectible
            SetVisible(false);
            
            TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, $"Gun: {gunPrefab.name}");

            // Destroy the collectible
            Destroy(gameObject, 0.1f);
        }
    }
    
    private void SetVisible(bool visible)
    {
        // Hide or show all renderers
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = visible;
        }
        
        // Disable any child renderers
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in childRenderers)
        {
            r.enabled = visible;
        }
        
        // Disable the collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = visible;
        }
    }
    
    // Method to create a collectible from an existing gun (for gun manager)
    public static CollectibleGun CreateFromGun(Gun gun, Vector3 position)
    {
        // This would be called by a GunManager to create collectibles
        // from guns that have been dropped
        return null; // Implementation depends on your game structure
    }
}