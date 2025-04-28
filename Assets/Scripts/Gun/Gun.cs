using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum GunType
{
    Rifle,
    Cannon,
    Shotgun,
    Sniper
}

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private Sprite gunIcon;
    [SerializeField] private string gunName;
    [SerializeField] [TextArea] private string gunDescription;

    public string GetGunName() => gunName;
    public string GetGunDescription() => gunDescription;


    [SerializeField] protected int bulletCapacity = 30;
    [SerializeField] protected int bulletNumber = 30;
    [SerializeField] protected float fireRate = 15;
    [SerializeField] protected float reloadPreparationTime = 1;
    [SerializeField] protected float reloadRate = 10;
    [SerializeField] protected float recoilForce = 1;
    [SerializeField] protected bool isFullAuto = true;
    [SerializeField] protected bool isInfiniteBullet = false;

    protected float lastFireTime = 0;
    protected float lastReloadTime = 0;
    protected bool isOverheat = false;
    protected bool isEquipped = false;

    protected float bulletGenerateDistance = 1.2f;
    private UIBullet bulletUI;
    private UIWeaponIndicator weaponIndicatorUI;

    private SpriteRenderer gunRenderer;

    protected string ownerName;

    protected virtual void Start()
    {
        ownerName = transform.parent.name;
        gunRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        float currentTime = Time.time;
        if (bulletNumber < bulletCapacity
            && (currentTime - lastReloadTime >= 1 / reloadRate ||
                isOverheat && currentTime - lastReloadTime >= 1 / (3 * reloadRate))
            && currentTime - lastFireTime >= reloadPreparationTime)
        {
            lastReloadTime = currentTime;
            ChangeBulletNumber(1);
        }

        gunRenderer.enabled = isEquipped;
    }

    public virtual void SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    public virtual Vector3 Fire(Vector3 direction)
    {
        if (isOverheat)
        {
            return Vector3.zero;
        }

        float currentTime = Time.time;
        if (bulletNumber <= 0 || currentTime - lastFireTime < 1 / fireRate)
        {
            return Vector3.zero;
        }

        GenerateBullet(direction);

        lastFireTime = currentTime;
        ChangeBulletNumber(-1);

        return -recoilForce * direction;
    }

    public virtual Vector3 StartFire(Vector3 direction)
    {
        return Fire(direction);
    }

    public virtual Vector3 KeepFire(Vector3 direction)
    {
        return isFullAuto ? Fire(direction) : Vector3.zero;
    }

    public virtual Vector3 StopFire(Vector3 direction)
    {
        return Vector3.zero;
    }

    public virtual void GenerateBullet(Vector3 direction)
    {
        GameObject bulletObj = Instantiate(bulletPrefab,
            transform.position + bulletGenerateDistance * direction, transform.rotation);
        LevelManager.Instance.DynamicallyAddGameObject(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.shotBy = ownerName;
        bullet.Fire(direction);
    }

    public void OnEquipped()
    {
        isEquipped = true;
        bulletUI?.UpdateBulletUI(this);
        weaponIndicatorUI?.UpdateWeaponIndicator(true);
    }

    public virtual void OnUnequipped()
    {
        isEquipped = false;
        weaponIndicatorUI?.UpdateWeaponIndicator(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnPickedUp(Player player)
    {
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        weaponIndicatorUI?.SetGun(this);
    }

    public void SetBulletUI(UIBullet bulletUI)
    {
        this.bulletUI = bulletUI;
    }

    public void SetWeaponIndicatorUI(UIWeaponIndicator weaponIndicatorUI)
    {
        this.weaponIndicatorUI = weaponIndicatorUI;
    }

    protected void ChangeBulletNumber(int change)
    {
        if (isInfiniteBullet)
        {
            return;
        }
        
        bulletNumber += change;

        if (!isOverheat && bulletNumber == 0)
        {
            isOverheat = true;
        }
        else if (isOverheat && bulletNumber == bulletCapacity)
        {
            isOverheat = false;
        }

        if (isEquipped)
        {
            bulletUI?.UpdateBulletUI(this);
        }
    }

    // UI
    public Sprite GetGunIcon()
    {
        return gunIcon;
    }

    public int GetRemainingBullets()
    {
        return bulletNumber;
    }

    public int GetBulletCapacity()
    {
        return bulletCapacity;
    }

    public bool IsOverheated()
    {
        return isOverheat;
    }

    public void ResetBullets()
    {
        bulletNumber = bulletCapacity;
        isOverheat = false;
    }
}