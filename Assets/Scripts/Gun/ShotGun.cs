using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class ShotGun : Gun
{
    public float fireAngle = 30;
    public float bulletNumberPerShot = 5;

    public override void GenerateBullet(Vector3 direction)
    {
        float angleGap = fireAngle / (bulletNumberPerShot - 1);
        for (int i = 0; i < bulletNumberPerShot; i++)
        {
            float directionOffsetDegree =  - fireAngle / 2 + i * angleGap;
            Debug.Log(directionOffsetDegree);
            Quaternion directionOffset = Quaternion.Euler(0, 0, directionOffsetDegree);
            Vector3 finalDirection = directionOffset * direction;

            GameObject bulletObj = Instantiate(bulletPrefab,
                transform.position + bulletGenerateDistance * finalDirection, transform.rotation * directionOffset);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.shotBy = ownerName;
            bullet.Fire(finalDirection);
        }
    }
}