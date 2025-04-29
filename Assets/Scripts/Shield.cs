using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Shield : MonoBehaviour, IBulletIteractable
{
    public int shieldHealth = 100;

    public virtual void OnBulletCollision(Bullet bullet)
    {
        bullet.OnAbsorbed();

        shieldHealth -= bullet.damage;

        if (shieldHealth <= 0)
        {
            DestroyShield();
        }
    }

    private void DestroyShield()
    {
        Destroy(gameObject);
    }
}