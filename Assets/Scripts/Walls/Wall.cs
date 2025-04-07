using System;
using UnityEngine;

public abstract class Wall : MonoBehaviour, IBulletIteractable
{
    public virtual void OnBulletCollision(Bullet bullet)
    {
        bullet.OnAbsorbed();
    }
}