using Assets.Scripts.EnvironmentInteractibles;
using UnityEngine;

public class Creature : MonoBehaviour, IBulletIteractable, IDoorController
{
    public int HP;
    public int maxHP;

    protected virtual void Start()
    {
        HP = maxHP; 
    }

    public bool IsDoorOpen()
    {
        return HP <= 0;
    }

    public virtual void OnBulletCollision(Bullet bullet)
    {
        TakeDamage(bullet.damage, $"{bullet.shotBy}:Bullet");
        bullet.OnAbsorbed();
    }

    public virtual void TakeDamage(int damage, string source = "unknown")
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHP);

        if (HP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died!");

    }
}
