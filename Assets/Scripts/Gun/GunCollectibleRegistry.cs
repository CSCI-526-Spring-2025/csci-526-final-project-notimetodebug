using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GunCollectibleRegistry", menuName = "Game/Gun Collectible Registry")]
public class GunCollectibleRegistry : ScriptableObject
{
    [System.Serializable]
    public class Mapping
    {
        public Gun gunPrefab;
        public GameObject collectiblePrefab;
    }

    public List<Mapping> mappings;

    private Dictionary<string, GameObject> gunToCollectible;

    public GameObject GetCollectiblePrefab(Gun gun)
    {
        if (gunToCollectible == null)
        {
            gunToCollectible = new Dictionary<string, GameObject>();
            foreach (var map in mappings)
            {
                if (map.gunPrefab != null && map.collectiblePrefab != null)
                {
                    gunToCollectible[map.gunPrefab.name] = map.collectiblePrefab;
                }
            }
        }

        gunToCollectible.TryGetValue(gun.name.Replace("(Clone)", "").Trim(), out var prefab);
        return prefab;
    }
}
