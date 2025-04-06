using System;
using UnityEngine;


public class SniperGun : Gun
{
    [SerializeField] private LineRenderer laserRenderer;
    private CameraController cameraController;

    private Boolean isAiming = false;
    private int laserBounceTimes;
    [SerializeField] private float aimingZoom = 16f;

    protected override void Start()
    {
        base.Start();
        cameraController = FindObjectOfType<CameraController>();
        laserBounceTimes = bulletPrefab.GetComponent<Bullet>().bounceLeft;
    }

    public override Vector3 StartFire(Vector3 direction)
    {
        isAiming = true;
        laserRenderer.enabled = true;
        cameraController?.SetFixedZoom(aimingZoom);
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
        for (int i = 0; i <= laserBounceTimes; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(nextPosition + nextDirection * raycastFrontGap,
                nextDirection, float.PositiveInfinity, LayerMask.GetMask("Platform"));
            laserRenderer.positionCount++;
            laserRenderer.SetPosition(i + 1, hit.point);
            if (hit.collider.gameObject.GetComponent<Wall>() is not Mirror)
            {
                break;
            }

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
        cameraController?.ReleaseFixedZoom();
        return Fire(direction);
    }
    
    public override void OnUnequipped()
    {
        cameraController?.ReleaseFixedZoom();
        base.OnUnequipped();
    }
}