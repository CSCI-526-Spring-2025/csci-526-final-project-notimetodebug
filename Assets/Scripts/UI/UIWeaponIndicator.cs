using UnityEngine;
using UnityEngine.UI;

public class UIWeaponIndicator : MonoBehaviour
{
    public Image outerCircle; // active/inactive states
    public Image innerSphere; // powerful gun type

    [SerializeField] private Sprite activateStatusSprite; 
    [SerializeField] private Sprite inactivateStatusSprite; 

    private Sprite currentGunSprite = null; 

    public void UpdateWeaponIndicator(Gun gun, bool isEquipped)
    {
        if (gun == null) return;

        bool isUsingDefault = gun.IsUsingDefaultBullet();

        outerCircle.sprite = isEquipped ? activateStatusSprite : inactivateStatusSprite;

        if (!isUsingDefault && gun.GetGunIcon() != null)
        {
            innerSphere.enabled = true;
            innerSphere.sprite = gun.GetGunIcon();
            currentGunSprite = gun.GetGunIcon(); 
        }
        else if (isUsingDefault)
        {
            innerSphere.enabled = currentGunSprite != null;
        }
    }
}
