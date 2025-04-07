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
    [SerializeField] private GunCollectibleRegistry gunCollectibleRegistry;

    private UIWeaponIndicator weaponIndicatorUI;
    private UIBullet bulletUI;

    private UIPlayerHP healthUI;
    private UILevelFail failUI;

    private bool isFiring = false;
    private Coroutine blinkCoroutine;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    #region Telemetry Only

    private enum MotionState
    {
        Idle,
        Grounded,
        Flying
    }

    private MotionState prevMotionState = MotionState.Idle;

    private bool hasMotionStateChanged(out MotionState motionState)
    {
        Vector2 velocity = GetVelocity();
        Collider2D ground = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, LayerMask.GetMask("Platform"))
            .collider;

        bool isGrounded = ground != null
                          && ground.gameObject.TryGetComponent<Wall>(out Wall groundWall)
                          && groundWall is not Spikes;


        if (isGrounded && velocity == Vector2.zero)
        {
            if (prevMotionState != MotionState.Idle)
            {
                motionState = MotionState.Idle;
                prevMotionState = motionState;
                return true;
            }
            else
            {
                motionState = prevMotionState;
                return false;
            }
        }
        else if (isGrounded)
        {
            if (prevMotionState != MotionState.Grounded)
            {
                motionState = MotionState.Grounded;
                prevMotionState = motionState;
                return true;
            }
            else
            {
                motionState = prevMotionState;
                return false;
            }
        }
        else
        {
            if (prevMotionState != MotionState.Flying)
            {
                motionState = MotionState.Flying;
                prevMotionState = motionState;
                return true;
            }
            else
            {
                motionState = prevMotionState;
                return false;
            }
        }
    }

    #endregion

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

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
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.W))
        {
            if (!isFiring)
            {
                recoilForce = currentGun.StartFire(fireDirection);
                isFiring = true;
            }
            else
            {
                recoilForce = currentGun.KeepFire(fireDirection);
            }
        }
        else if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.W) && isFiring)
        {
            recoilForce = currentGun.StopFire(fireDirection);
            isFiring = false;
        }

        rb.AddForce(recoilForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeGun();
        }

        if (hasMotionStateChanged(out MotionState motionState))
        {
            TelemetryManager.Log(TelemetryManager.EventName.MOVEMENT_STATE_CHANGE, motionState.ToString());
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
        isFiring = false;
    }
    public void PickUpGun(Gun gun)
    {
        if (guns.Count > 1)
        {
            Gun oldSpecialGun = guns.ElementAt(1);
            oldSpecialGun.OnUnequipped();
            guns.RemoveAt(1);

            GameObject collectiblePrefab = gunCollectibleRegistry.GetCollectiblePrefab(oldSpecialGun);

            if (collectiblePrefab != null)
            {
                Vector3 dropPosition = transform.position + Vector3.right * 1.5f + Vector3.up * 0.5f;
                Instantiate(collectiblePrefab, dropPosition, Quaternion.identity);
            }

            oldSpecialGun.Destroy();
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
        base.TakeDamage(damage, source);
        healthUI?.UpdateHealth(HP);
        healthUI?.BlinkDamageHPBar();
        if (blinkCoroutine is not null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkRed());

        TelemetryManager.Log(TelemetryManager.EventName.PLAYER_DAMAGED, source);
    }

    public void Heal(int amount)
    {
        HP = Mathf.Min(HP + amount, maxHP);
        healthUI?.UpdateHealth(HP);
        healthUI?.BlinkHealHPBar();
    }

    private IEnumerator BlinkRed()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        blinkCoroutine = null;
    }

    protected override void Die()
    {
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

    public void ResetToDefaultGun()
    {
        if (guns.Count > 1)
        {
            for (int i = 1; i < guns.Count; i++)
            {
                guns[i].Destroy();
                guns.RemoveAt(i);
            }
        }

        currentGunIndex = 0;
        guns[0].OnEquipped();
        isFiring = false;
    }

    public void ResetState()
    {
        this.HP = maxHP;
        healthUI?.UpdateHealth(this.HP);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        ResetToDefaultGun();
    }
}