using UnityEngine;

public class Creature: MonoBehaviour, IBulletIteractable
{
    public int HP;
    public int maxHP;

    public void OnBulletCollision(Bullet bullet)
    {
        HP -= bullet.damage;
        bullet.OnAbsorbed();
    }
}