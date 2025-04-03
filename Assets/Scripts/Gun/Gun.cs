using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum GunType {
    Default,
    StrongGun,
    Shotgun,
    Sniper
}

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private Sprite gunIcon;

    [SerializeField] protected int bulletCapacity = 30;
    [SerializeField] protected int bulletNumber = 30;
    [SerializeField] protected float fireRate = 15;
    [SerializeField] protected float reloadPreparationTime = 1;
    [SerializeField] protected float reloadRate = 10;
    [SerializeField] protected float recoilForce = 1;
    [SerializeField] protected bool isFullAuto = true;

    public GameObject collectiblePrefab;
    protected float lastFireTime = 0;
    protected float lastReloadTime = 0;

    protected float bulletGenerateDistance = 1.2f;
    private UIBullet bulletUI;
    private UIWeaponIndicator weaponIndicatorUI;

    protected string ownerName;

    protected virtual void Start()
    {
        ownerName = transform.parent.name;
    }

    protected virtual void Update()
    {
        float currentTime = Time.time;
        if (bulletNumber < bulletCapacity
                        && currentTime - lastReloadTime >= 1 / reloadRate
                        && currentTime - lastFireTime >= reloadPreparationTime)
        {
            lastReloadTime = currentTime;
            ChangeBulletNumber(1);
        }
    }

    public virtual void SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    public virtual Vector3 Fire(Vector3 direction)
    {
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
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.shotBy = ownerName;
        bullet.Fire(direction);
    }

    public void OnEquipped()
    {
        gameObject.SetActive(true);
        bulletUI?.UpdateBulletUI(this);
        weaponIndicatorUI?.UpdateWeaponIndicator(true);
    }

    public virtual void OnUnequipped()
    {
        gameObject.SetActive(false);
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
        bulletNumber += change;
        bulletUI?.UpdateBulletUI(this);
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

}