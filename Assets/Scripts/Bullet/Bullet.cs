using System;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float initialVelocity = 10;

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

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}