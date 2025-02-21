using System;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int bulletCapacity = 30;
    public int bulletNumber = 30;
    public float fireRate = 15;
    public float reloadingTime = 2;
    public float recoilForce = 1;
    public bool isFullAuto = true;

    private float lastFireTime = 0;
    private float startReloadingTime = 0;
    private bool isReloading = false;

    private float bulletGenerateDistance = 1.2f;

    private void Update()
    {
        if (isReloading)
        {
            float currentTime = Time.time;
            if (currentTime - startReloadingTime >= reloadingTime)
            {
                isReloading = false;
                bulletNumber = bulletCapacity;
            }
        }
    }

    public void SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    public Vector3 Fire(Vector3 direction)
    {
        float currentTime = Time.time;
        if (!isReloading && bulletNumber > 0 && currentTime - lastFireTime >= 1 / fireRate)
        {
            GenerateBullet(direction);

            lastFireTime = currentTime;
            bulletNumber--;

            return -recoilForce * direction;
        }

        return Vector3.zero;
    }

    public Vector3 HoldFire(Vector3 direction)
    {
        return isFullAuto ? Fire(direction) : Vector3.zero;
    }

    public void GenerateBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab,
            transform.position + bulletGenerateDistance * direction, Quaternion.identity);
        bullet.GetComponent<Bullet>().Fire(direction);
    }

    public void Reload()
    {
        if (bulletNumber >= bulletCapacity)
        {
            return;
        }

        isReloading = true;
        startReloadingTime = Time.time;
    }
}