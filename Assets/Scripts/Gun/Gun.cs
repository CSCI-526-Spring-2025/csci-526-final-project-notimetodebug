using System;
using UnityEngine;
using UnityEngine.Serialization;


public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected int bulletCapacity = 30;
    [SerializeField] protected int bulletNumber = 30;
    [SerializeField] protected float fireRate = 15;
    [SerializeField] protected float reloadPreparationTime = 1;
    [SerializeField] protected float reloadRate = 10;
    [SerializeField] protected float recoilForce = 1;
    [SerializeField] protected bool isFullAuto = true;

    protected float lastFireTime = 0;
    protected float lastReloadTime = 0;

    private float bulletGenerateDistance = 1.2f;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        float currentTime = Time.time;
        if (bulletNumber < bulletCapacity
                        && currentTime - lastReloadTime >= 1 / reloadRate
                        && currentTime - lastFireTime >= reloadPreparationTime)
        {
            bulletNumber++;
            lastReloadTime = currentTime;
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
        bulletNumber--;

        return -recoilForce * direction;
    }

    public virtual Vector3 StartFire(Vector3 direction)
    {
        if (bulletNumber <= 0)
        {
            return Vector3.zero;
        }

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
        GameObject bullet = Instantiate(bulletPrefab,
            transform.position + bulletGenerateDistance * direction, transform.rotation);
        bullet.GetComponent<Bullet>().Fire(direction);
    }

    public void OnEquipped()
    {
        gameObject.SetActive(true);
    }

    public void OnUnequipped()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnPickedUp(Player player)
    {
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }
}