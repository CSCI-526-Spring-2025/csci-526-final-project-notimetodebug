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
        recoilForce = gun.Fire(fireDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Creature creature))
        {
            Vector2 normal = collision.contacts[0].normal;

            Rigidbody2D creatureRb = creature.GetComponent<Rigidbody2D>();
            creatureRb.velocity += -normal * BounceBackVelocity;

            creature.HP -= Damage;
        }
    }

}
