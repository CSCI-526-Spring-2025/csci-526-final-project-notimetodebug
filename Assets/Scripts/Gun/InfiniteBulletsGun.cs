using UnityEngine;

public class InfiniteBulletsGun : Gun
{
    public override Vector3 Fire(Vector3 direction, bool isSpecialBullet = false)
    {
        float currentTime = Time.time;
        if (bulletNumber <= 0 || currentTime - lastFireTime < 1 / fireRate)
        {
            return Vector3.zero;
        }

        GenerateBullet(direction, isSpecialBullet);

        lastFireTime = currentTime;

        return -recoilForce * direction;
    }
}