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
    private bool playerInRange = false;

    [Header("Pickup Text")]
    public bool showPickupHint = true;
    public GameObject pickupHint;

    [Header("Gun Type")]
    public GunType gunType;

    void Start()
    {
        startPosition = transform.position;
        if (pickupHint != null)
        {
            pickupHint.SetActive(false);
        }
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

        if (playerInRange && !isCollected && Input.GetKeyDown(KeyCode.P))
        {
            CollectGun();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {

        if (isCollected)
            return;
        
        Player player = other.GetComponent<Player>();
        
        if (player != null)
        {
            playerInRange = true;
            if (pickupHint != null && showPickupHint)
            {
                pickupHint.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            playerInRange = false;
            if (pickupHint != null)
            {
                pickupHint.SetActive(false);
            }

        }
    }

    void CollectGun(){
        isCollected = true;

        Player player = FindObjectOfType<Player>();
        if (player == null || gunPrefab == null)
            return;

        // player.PickUpGun(gunPrefab);
        Gun newGun = Instantiate(gunPrefab, player.transform);
        newGun.transform.localPosition = Vector3.zero;
        newGun.transform.rotation = Quaternion.identity;

        player.PickUpGun(newGun);

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        if (pickupHint != null)
        {
            pickupHint.SetActive(false);
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        TelemetryManager.Log(TelemetryManager.EventName.COLLECTIBLE_PICKED_UP, $"Gun: {gunPrefab.name}");

        Destroy(gameObject, 0.1f);
    }
}