using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Wall
{
    public int Damage = 40;
    public int BounceBackVelocity = 10;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnBulletCollision(Bullet bullet)
    {
        bullet.OnAbsorbed();
    }
}