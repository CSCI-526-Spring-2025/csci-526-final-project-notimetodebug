using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float moveSpeed = 2f;             // Speed of movement
    private Rigidbody2D rb;
    private bool movingRight = true;
    [SerializeField]  private Gun gun;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);

    }

    public void Flip()
    {
        movingRight = !movingRight;
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.Rotate(0f, 180f, 0f);
    }


    public void Shoot(Transform playerTransform)
    {
        Vector3 recoilForce = Vector3.zero;
        Vector3 fireDirection = (playerTransform.position - transform.position).normalized;
        gun.SetDirection(fireDirection);
        Debug.Log("Fire Direction: " + fireDirection);
        Debug.Log("Magnitude of normalized vector: " + fireDirection.magnitude);
        recoilForce = gun.Fire(fireDirection);
    }

    // For testing purposes
    //void OnDrawGizmos()
    //{
    //    // Draw a debug line to visualize ground check
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

    //    // Draw a debug line to visualize wall detection
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position, transform.position + (movingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
    //}
}
