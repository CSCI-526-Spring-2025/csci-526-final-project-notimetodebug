using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float moveSpeed = 2f;             // Speed of movement
    private Rigidbody2D rb;
    private bool movingRight = true;
    [SerializeField]  private Gun gun;
    public int Damage = 40;
    public int BounceBackVelocity = 10;
    [SerializeField] private int scoreValue = 20;

    [SerializeField] private GameObject hpBarPrefab; 
    private UIEnemyHP enemyHPBar; 

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    if (hpBarPrefab != null)
    {
        GameObject hpBarObject = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        enemyHPBar = hpBarObject.GetComponent<UIEnemyHP>();
        enemyHPBar.Setup(this);
    }
    else
    {
        Debug.LogError("HP Bar Prefab not assigned in " + gameObject.name);
    }
   // LevelManager.Instance.RegisterEnemyScore(scoreValue);
    }

    void Update()
    {
        if (HP <= 0)
        {
            Die();
        }
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
        recoilForce = gun.Fire(fireDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Creature creature))
        {
            Vector2 normal = collision.contacts[0].normal;

            Rigidbody2D creatureRb = creature.GetComponent<Rigidbody2D>();
            creatureRb.velocity += -normal * BounceBackVelocity;

            creature.TakeDamage(Damage, "Enemy Touch");
        }
    }

    public override void TakeDamage(int damage, string source = "unknown")
    {
        base.TakeDamage(damage);
        if (enemyHPBar != null)
        {
            enemyHPBar.UpdateHealth(HP);
        }
    }

    protected override void Die()
    {
        LevelManager.Instance.AddScore(scoreValue);
        if (enemyHPBar != null)
        {
            Destroy(enemyHPBar.gameObject);
        }
        Destroy(gameObject);
    }
    public int GetScoreValue()
    {
        return scoreValue;
    }


}
