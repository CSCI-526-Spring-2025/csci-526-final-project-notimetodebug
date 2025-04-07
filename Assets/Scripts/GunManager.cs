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
    
    private Dictionary<string, CollectibleGun> gunToCollectibleMap = new Dictionary<string, CollectibleGun>();
    
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
        }
        
        // Build the lookup map
        foreach (var mapping in gunMappings)
        {
            gunToCollectibleMap[mapping.gunName] = mapping.collectiblePrefab;
        }
    }
    
    // public CollectibleGun GetCollectiblePrefabForGunType(Gun gun)
    // {
    //     if (gunToCollectibleMap.TryGetValue(gun.name.Replace("(Clone)", ""), out CollectibleGun collectible))
    //     {
    //         return collectible;
    //     }
        
    //     Debug.LogWarning($"No collectible prefab found for gun: {gun.name}");
    //     return null;
    // }
    public CollectibleGun GetCollectiblePrefabForGunType(Gun gun)
    {
        // Use the gun name without the "(Clone)" suffix that Unity adds to instantiated objects
        if (gunToCollectibleMap.TryGetValue(gun.name.Replace("(Clone)", ""), out CollectibleGun collectible))
        {
            return collectible;
        }
        
        Debug.LogWarning($"No collectible prefab found for gun: {gun.name}");
        return null;
    }
}