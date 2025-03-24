using System;
using UnityEngine;


public class SniperGun : Gun
{
    private Boolean isAiming = false;
    public LineRenderer laserRenderer;
    private int laserBounceTimes;

    private void Start()
    {
        laserBounceTimes = bulletPrefab.GetComponent<Bullet>().bounceLeft;
        Debug.Log(bulletPrefab.GetComponent<Bullet>().bounceLeft);
    }

    public override Vector3 StartFire(Vector3 direction)
    {
        isAiming = true;
        laserRenderer.enabled = true;
        return Vector3.zero;
    }

    public override void SetDirection(Vector3 direction)
    {
        base.SetDirection(direction);
        if (isAiming)
        {
            UpdateLaser(direction);
        }
    }

    private void UpdateLaser(Vector3 direction)
    {
        const float raycastFrontGap = 0.05f;

        laserRenderer.positionCount = 1;
        laserRenderer.SetPosition(0, laserRenderer.transform.position);
        Vector3 nextDirection = direction;
        Vector3 nextPosition = laserRenderer.transform.position;
        for (int i = 0; i < laserBounceTimes; i++)
        {
            // Debug.Log(i + "origin " + (nextPosition + nextDirection * raycastFrontGap));

            RaycastHit2D hit = Physics2D.Raycast(nextPosition + nextDirection * raycastFrontGap,
                nextDirection, float.PositiveInfinity, LayerMask.GetMask("Platform"));
            laserRenderer.positionCount++;
            laserRenderer.SetPosition(i + 1, hit.point);
            // Debug.DrawRay(hit.point, hit.normal, Color.red);
            if (!hit.collider.gameObject.GetComponent<Wall>().isMirror)
            {
                break;
            }

            // Debug.Log(i + "normal " + (Vector3)hit.normal);
            // Debug.Log(i + "hit point " + (Vector3)hit.point);
            // Debug.Log(i + "d " + (-(Quaternion.AngleAxis(180, hit.normal) * nextDirection) + "nd " + nextDirection));

            nextPosition = hit.point;
            nextDirection = -(Quaternion.AngleAxis(180, hit.normal) * nextDirection);
        }
    }

    public override Vector3 KeepFire(Vector3 direction)
    {
        return Vector3.zero;
    }

    public override Vector3 StopFire(Vector3 direction)
    {
        isAiming = false;
        laserRenderer.enabled = false;
        return Fire(direction);
    }
}