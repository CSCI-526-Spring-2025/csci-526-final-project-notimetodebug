using System;
using UnityEngine;

public abstract class Wall : MonoBehaviour, IBulletIteractable
{
    public bool isMirror { get; protected set; } = false;

    public virtual void OnBulletCollision(Bullet bullet)
    {
        if (isMirror)
        {
            bullet.OnBounce();
        }
        else
        {
            bullet.OnAbsorbed();
        }
    }
}