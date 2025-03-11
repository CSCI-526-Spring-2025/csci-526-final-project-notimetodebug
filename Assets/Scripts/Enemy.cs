using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float moveSpeed = 2f;             // Speed of movement
    private Rigidbody2D rb;
    private bool movingRight = true;
    [SerializeField] private Gun gun;
    public int Damage = 40;
    public int BounceBackVelocity = 10;

    [SerializeField] private GameObject hpBarPrefab;
    private UIEnemyHP enemyHPBar;

    public float wallCheckDistance = 0.5f;  // Distance for detecting walls
    public float groundCheckDistance = 1f;  // Distance for detecting ground
    public Transform groundCheckPosition;  // Position to cast ground check ray
    public Transform wallCheckPosition;  // Position to cast ground check ray

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //if (hpBarPrefab != null)
        //{
        //    GameObject hpBarObject = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        //    enemyHPBar = hpBarObject.GetComponent<UIEnemyHP>();
        //    enemyHPBar.Setup(this);
        //}
        //else
        //{
        //    Debug.LogError("HP Bar Prefab not assigned in " + gameObject.name);
        //}


    }

    void Update()
    {
        //if (HP <= 0)
        //{
        //    Die();
        //}
        Move();
    }

    bool DetectWall()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        // Cast a ray in the movement direction and get all collisions
        RaycastHit2D[] hits = Physics2D.RaycastAll(wallCheckPosition.position, direction, wallCheckDistance);

        if (hits.length > 1) // ensure there's a second collision
        {
            raycasthit2d secondhit = hits[1]; // get the second detected object

            if (secondhit.collider != null && secondhit.collider.comparetag("wall"))
            {
                return true; // flip only if second object is a wall
            }
        }
        return false;
    }

    bool DetectGround()
    {
        // Cast a ray downward and get all collisions
        RaycastHit2D[] hits = Physics2D.RaycastAll(groundCheckPosition.position, Vector2.down, groundCheckDistance);

        if (hits.Length > 1) // Ensure there's a second collision
        {
            RaycastHit2D secondHit = hits[1]; // Get the second detected object

            if (secondHit.collider != null && secondHit.collider.CompareTag("Ground"))
            {
                return true; // Flip only if the second object is ground
            }
        }
        return false;
    }

    bool DetectGround()
    {
        if (groundCheckPosition == null)
        {
            Debug.LogError("groundCheckPosition is not assigned in " + gameObject.name);
            return false;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(groundCheckPosition.position, Vector2.down, groundCheckDistance);

        if (hits.Length > 1)
        {
            RaycastHit2D secondHit = hits[1];

            if (secondHit.collider != null && secondHit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
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
        recoilForce = gun.Fire(fireDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Creature creature))
        {
            Vector2 normal = collision.contacts[0].normal;

            Rigidbody2D creatureRb = creature.GetComponent<Rigidbody2D>();
            creatureRb.velocity += -normal * BounceBackVelocity;

            creature.TakeDamage(Damage);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (enemyHPBar != null)
        {
            enemyHPBar.UpdateHealth(HP);
        }
    }

    protected override void Die()
    {
        if (enemyHPBar != null)
        {
            Destroy(enemyHPBar.gameObject);
        }
        Destroy(gameObject);
    }

    // Debugging with Gizmos
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; // Ensure Gizmos are only drawn during play mode

        // Wall detection ray
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheckPosition.position, wallCheckPosition.position + (Vector3)direction * wallCheckDistance);

        // Ground detection ray
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckPosition.position, groundCheckPosition.position + Vector3.down * groundCheckDistance);
    }

}
