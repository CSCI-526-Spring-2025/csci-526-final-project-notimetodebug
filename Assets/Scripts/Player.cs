using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : Creature
{
    private Rigidbody2D rb;

    private Vector3 fireDirection;

    [SerializeField] public int currentGunIndex;

    [SerializeField] public List<Gun> guns;

    private UIWeaponIndicator weaponIndicatorUI;
    private UIBullet bulletUI;

    private UIPlayerHP healthUI;

    // Start is called before the first frame update
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        weaponIndicatorUI = FindObjectOfType<UIWeaponIndicator>();
        
        bulletUI = FindObjectOfType<UIBullet>();

        // Find and link the UI HP bar
        healthUI = FindObjectOfType<UIPlayerHP>();
        healthUI?.SetMaxHealth(maxHP);
        healthUI?.UpdateHealth(HP);

        for (int i = 0; i < guns.Count; i++)
        {
            guns.ElementAt(i).SetBulletUI(bulletUI);
            if (i != currentGunIndex)
            {
                guns.ElementAt(i).OnUnequipped();
            }
            else
            {
                guns.ElementAt(i).OnEquipped();
            }
        }
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        fireDirection = (mousePos - transform.position).normalized;

        Gun currentGun = guns.ElementAt(currentGunIndex);

        currentGun.SetDirection(fireDirection);

        Vector3 recoilForce = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            recoilForce = currentGun.StartFire(fireDirection);
        }
        else if (Input.GetMouseButton(0))
        {
            recoilForce = currentGun.KeepFire(fireDirection);
        }
        else
        {
            recoilForce = currentGun.StopFire(fireDirection);
        }

        rb.AddForce(recoilForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeGun();
        }
    }

    private void ChangeGun()
    {
        Gun currentGun = guns.ElementAt(currentGunIndex);

        currentGun.OnUnequipped();
        currentGunIndex = (currentGunIndex + 1) % guns.Count;
        currentGun = guns.ElementAt(currentGunIndex);
        currentGun.SetDirection(fireDirection);
        currentGun.OnEquipped();
    }

    public void PickUpGun(Gun gun)
    {
        if (guns.Count > 1)
        {
            guns.ElementAt(1).OnUnequipped();
            guns.ElementAt(1).Destroy();
            guns.RemoveAt(1);
        }

        gun.SetBulletUI(bulletUI);
        gun.SetWeaponIndicatorUI(weaponIndicatorUI);
        guns.Add(gun);
        gun.OnPickedUp(this);

        if (currentGunIndex == guns.Count - 1)
        {
            gun.OnEquipped();
        }
        else
        {
            gun.OnUnequipped();
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthUI?.UpdateHealth(HP);
    }

    protected override void Die()
    {
        LevelManager.Instance.RespawnPlayer();
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}