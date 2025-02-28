using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float moveSpeed = 2f;             // Speed of movement
    public Transform groundCheck;            // Empty GameObject in front of enemy to check for ground
    public float groundCheckDistance = 0.5f;  // Distance to check for ground
    public float wallCheckDistance = 1.2f;   // Distance to check for walls

    private Rigidbody2D rb;
    private bool movingLeft = true;

    [Header("Shooting Settings")]
    public float shootRange = 5f;  // Detection range
    [SerializeField]  private Gun gun;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        CheckForPlayer();
        
    }

    void Move()
    {
        rb.velocity = new Vector2(movingLeft ? -moveSpeed : moveSpeed, rb.velocity.y);

        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
        bool isGroundAhead = groundHit.collider != null;
        //Debug.Log("Is ground ahead? " + isGroundAhead);


        Vector2 wallRayDirection = movingLeft ? Vector2.left : Vector2.right;
        RaycastHit2D[] wallHits = Physics2D.RaycastAll(transform.position, wallRayDirection, wallCheckDistance);

        //foreach (RaycastHit2D hit in wallHits)
        //{
        //    // Print the collider hit by the ray
        //    Debug.Log("Wall Ray hit collider: " + hit.collider.name);
        //}


        if ((!isGroundAhead || wallHits.Length > 1))
        {
            Flip();
        }
    }

    void Flip()
    {
        movingLeft = !movingLeft;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void CheckForPlayer()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootRange);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Shoot(collider.transform);
                break; // Stop checking after finding the player
            }
        }
    }

    void Shoot(Transform playerTransform)
    {
        Vector3 recoilForce = Vector3.zero;
        Vector3 fireDirection = (playerTransform.position - transform.position).normalized;
        gun.SetDirection(fireDirection);
        Debug.Log("Fire Direction: " + fireDirection);
        recoilForce = gun.KeepFire(fireDirection);
    }

    // For testing purposes
    void OnDrawGizmos()
    {
        // Draw a debug line to visualize ground check
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

        // Draw a debug line to visualize wall detection
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (movingLeft ? Vector3.left : Vector3.right) * wallCheckDistance);

        // Visualize shooting range in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
