using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public float moveSpeed = 2f;             // Speed of movement
    protected Rigidbody2D rb;
    protected bool movingRight = true;
    [SerializeField]  private Gun gun;
    public int Damage = 40;
    public int BounceBackVelocity = 10;
    [SerializeField] private int scoreValue = 20;

    [SerializeField] private GameObject hpBarPrefab; 
    private UIEnemyHP enemyHPBar; 
    private bool isDead = false;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun?.OnEquipped();
        
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
    }

    void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);

    }

    public virtual void Flip()
    {
        
        movingRight = !movingRight;
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
        if (HP <= 0)
        {
            Die();
        }
        if (enemyHPBar != null)
        {
            enemyHPBar.UpdateHealth(HP);
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Enemy died, adding score: " + scoreValue);
       // LevelManager.Instance.AddEnemyKillScore(scoreValue);
        LevelManager.Instance.AddEnemyKillScore(scoreValue, transform.position);

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
