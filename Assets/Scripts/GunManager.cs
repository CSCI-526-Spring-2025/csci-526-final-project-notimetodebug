using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance { get; private set; }
    
    [System.Serializable]
    public class GunMapping
    {
        public string gunName;  
        public Gun gunPrefab;   
        public CollectibleGun collectiblePrefab;  
    }
    
    [SerializeField] private List<GunMapping> gunMappings = new List<GunMapping>();
    [SerializeField] private CollectibleGun defaultCollectiblePrefab; // Fallback collectible
    
    private Dictionary<string, GunMapping> gunToMappingMap = new Dictionary<string, GunMapping>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Build the lookup map
        foreach (var mapping in gunMappings)
        {
            gunToMappingMap[mapping.gunName] = mapping;
        }
    }
    
    public CollectibleGun CreateCollectibleFromGun(Gun gun, Vector3 position)
    {
        
        string gunBaseName = gun.name.Replace("(Clone)", "");
        
        if (gunToMappingMap.TryGetValue(gunBaseName, out GunMapping mapping))
        {
          
            CollectibleGun newCollectible = Instantiate(mapping.collectiblePrefab, position, Quaternion.identity);
            newCollectible.gunPrefab = mapping.gunPrefab; 
            return newCollectible;
        }
        
        Debug.LogWarning($"No mapping found for gun: {gunBaseName}, using default collectible");
        if (defaultCollectiblePrefab != null)
        {
            return Instantiate(defaultCollectiblePrefab, position, Quaternion.identity);
        }
        
        return null;
    }
    
    public Gun GetGunPrefabByName(string gunName)
    {
        if (gunToMappingMap.TryGetValue(gunName, out GunMapping mapping))
        {
            return mapping.gunPrefab;
        }
        return null;
    }
}