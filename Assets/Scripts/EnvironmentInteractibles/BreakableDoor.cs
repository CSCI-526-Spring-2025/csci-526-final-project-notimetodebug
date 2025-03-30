using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoor : Wall
{
    public int health = 100;

    private Animator animator;
    private BoxCollider2D boxCollider;

    public override void OnBulletCollision(Bullet bullet)
    {
        if (bullet.isSpecialBullet)
        {
            health -= bullet.damage;
            animator.SetTrigger("Shake");
            if (health <= 0)
            {
                animator.SetTrigger("Destroy");
                boxCollider.enabled = false;
            }
        } else
        {
            animator.SetTrigger("Shake");
        }

        bullet.OnAbsorbed();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
