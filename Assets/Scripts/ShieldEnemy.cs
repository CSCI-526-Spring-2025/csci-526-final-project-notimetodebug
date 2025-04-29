using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    [SerializeField] private Shield shield;

    protected override void Start()
    {
        base.Start();
        if (shield == null)
        {
            Debug.LogWarning("Shield not assigned to ShieldEnemy: " + gameObject.name);
        }
    }

    //public override void TakeDamage(int damage, string source = "unknown")
    //{

    //    if (shield != null)
    //    {
    //        shield.OnBulletCollision(new Bullet() { damage = damage, isSpecialBullet = true });
    //    }
    //    else
    //    {
    //        base.TakeDamage(damage, source);
    //    }
    //}
}