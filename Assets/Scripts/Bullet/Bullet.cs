using System;
using UnityEditor;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IBulletIteractable
{
    public string shotBy;
    public bool isSpecialBullet = false;
    public string bulletName;

    [SerializeField] protected float initialVelocity = 10;
    protected Vector3 initialDirection;

    [field: SerializeField] public int damage { get; protected set; } = 5;
    [field: SerializeField] public int bounceLeft { get; protected set; } = 0;

    private Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = initialDirection * initialVelocity;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    protected virtual void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    public virtual void Fire(Vector3 direction)
    {
        initialDirection = direction;
        TelemetryManager.LogCumulative(TelemetryManager.EventName.PLAYER_SHOT_BULLET, bulletName);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<IBulletIteractable>(out IBulletIteractable bulletIteractable))
        {
            bulletIteractable.OnBulletCollision(this);
        }
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
        if (damage <= 0 || bullet.damage <= 0)
        {
            return;
        }

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