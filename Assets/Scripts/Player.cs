using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections;

public class Player : Creature
{
    private Rigidbody2D rb;

    private Vector3 fireDirection;

    [SerializeField] public int currentGunIndex;

    [SerializeField] public List<Gun> guns;

    private UIWeaponIndicator weaponIndicatorUI;
    private UIBullet bulletUI;

    private UIPlayerHP healthUI;
    private UILevelFail failUI;


    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        weaponIndicatorUI = FindObjectOfType<UIWeaponIndicator>();
        
        bulletUI = FindObjectOfType<UIBullet>();
        failUI = FindObjectOfType<UILevelFail>(true);

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

    public override void TakeDamage(int damage, string source = "unknown")
    {
        base.TakeDamage(damage);
        healthUI?.UpdateHealth(HP);
        healthUI?.BlinkHPBar();
        StartCoroutine(BlinkRed());

        TelemetryManagerRef.GetComponent<TelemetryManager>().Log(TelemetryManager.EventName.PLAYER_DAMAGED, source);
    }

    private IEnumerator BlinkRed()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Die()
    {
       // LevelManager.Instance.RespawnPlayer();
       LevelManager.Instance.ShowLevelFailUI();
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public void ResetToDefaultGun(){
        if (guns.Count > 1){
            for (int i = 1; i < guns.Count; i++){
                guns[i].Destroy();
                guns.RemoveAt(i);
            }
        }
        currentGunIndex = 0;
        guns[0].OnEquipped();
    }
}