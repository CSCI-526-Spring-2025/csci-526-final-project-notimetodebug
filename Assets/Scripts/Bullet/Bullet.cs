using System;
using UnityEditor;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public string shotBy;
    public bool isSpecialBullet = false;
    public string bulletName;

    [SerializeField] protected float initialVelocity = 10;
    protected Vector3 initialDirection;

    [field: SerializeField] public int damage { get; protected set; } = 5;
    [field: SerializeField] public int bounceLeft { get; protected set; } = 0;

    protected Rigidbody2D rb;

    protected virtual void Start()
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
}