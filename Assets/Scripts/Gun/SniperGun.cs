using System;
using UnityEngine;


public class SniperGun : Gun
{
    Boolean isAiming = false;
    
    public override Vector3 StartFire(Vector3 direction)
    {
        isAiming = true;
        return Vector3.zero;
    }

    public override Vector3 KeepFire(Vector3 direction)
    {
        return Vector3.zero;
    }

    public override Vector3 StopFire(Vector3 direction)
    {
        isAiming = false;
        return Fire(direction);
    }
}