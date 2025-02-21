using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity = 10;

    private Rigidbody2D rb;

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    public void Fire(Vector3 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = direction * velocity;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rb.velocity);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}