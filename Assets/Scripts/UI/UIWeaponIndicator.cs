using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponIndicator : MonoBehaviour
{
    public Image outerCircle; // active/inactive states
    public Image innerSphere; // powerful gun type

    [SerializeField] private Sprite activateStatusSprite;
    [SerializeField] private Sprite inactivateStatusSprite;

    private void Start()
    {
        innerSphere.enabled = false;
    }

    public void SetGun(Gun gun)
    {
        Sprite icon = gun.GetGunIcon();
        if (icon != null)
        {
            innerSphere.sprite = icon;
            innerSphere.enabled = true;
        }
    }

    public void UpdateWeaponIndicator(bool isEquipped)
    {
        outerCircle.sprite = isEquipped ? activateStatusSprite : inactivateStatusSprite;
    }
}