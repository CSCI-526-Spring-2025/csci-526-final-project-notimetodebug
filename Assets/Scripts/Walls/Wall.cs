using UnityEngine;

public abstract class Wall: MonoBehaviour, IBulletIteractable
{
    public abstract void OnBulletCollision(Bullet bullet);
}