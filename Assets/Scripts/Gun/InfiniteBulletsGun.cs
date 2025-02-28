using UnityEngine;

public class InfiniteBulletsGun : Gun
{
    public override Vector3 Fire(Vector3 direction)
    {
        float currentTime = Time.time;
        if (isReloading || bulletNumber <= 0 || currentTime - lastFireTime < 1 / fireRate)
        {
            return Vector3.zero;
        }

        GenerateBullet(direction);

        lastFireTime = currentTime;

        return -recoilForce * direction;
    }
}