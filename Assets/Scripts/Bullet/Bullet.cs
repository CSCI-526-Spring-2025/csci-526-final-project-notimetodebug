using System;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IBulletIteractable
{
    [SerializeField] protected float initialVelocity = 10;

    [field: SerializeField] public int damage { get; protected set; } = 5;
    [SerializeField] protected int bounceLeft = 0;

    private Rigidbody2D rb;

    protected void Start()
    {
    }

    protected virtual void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    public virtual void Fire(Vector3 direction)
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = direction * initialVelocity;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (damage <= 0)
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IBulletIteractable>(out IBulletIteractable bulletIteractable))
        {
            bulletIteractable.OnBulletCollision(this);
        }

        // if (other.gameObject.TryGetComponent<Creature>(out Creature creature))
        // {
        //     creature.HP -= damage;
        //     OnAbsorbed();
        // }

        // if (other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        // {
        //     if (bullet.damage > damage)
        //     {
        //         bullet.damage -= damage;
        //         OnAbsorbed();
        //     }
        //     else
        //     {
        //         damage -= bullet.damage;
        //         bullet.OnAbsorbed();
        //     }
        // }

        // if (other.gameObject.TryGetComponent(out Wall wall))
        // {
        //     if (wall is Mirror)
        //     {
        //         OnBounce();
        //     }
        //
        //     if (wall is Spikes)
        //     {
        //         OnAbsorbed();
        //     }
        //
        //     if (wall is Normal)
        //     {
        //         OnAbsorbed();
        //     }
        // }
    }

    public virtual void OnBounce()
    {
        if (bounceLeft <= 0)
        {
            OnAbsorbed();
        }
        else
        {
            bounceLeft--;
        }
    }

    public virtual void OnAbsorbed()
    {
        Destroy(gameObject);
    }

    public void OnBulletCollision(Bullet bullet)
    {
        int tempDamage = damage;
        damage = Math.Max(damage - bullet.damage, 0);
        bullet.damage = Math.Max(bullet.damage - tempDamage, 0);
        
        if (damage <= 0)
        {
            OnAbsorbed();
        }
        if (bullet.damage <= 0)
        {
            bullet.OnAbsorbed();
        }
    }
}