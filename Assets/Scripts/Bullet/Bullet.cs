using System;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float initialVelocity = 10;

    [SerializeField] protected int damage = 5;
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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Creature creature = other.gameObject.GetComponent<Creature>();
            if (creature is not null)
            {
                creature.HP -= damage;
                OnAbsorbed();
            }
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