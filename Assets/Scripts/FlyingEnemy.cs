using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float amplitude = 1f;     
    public float frequency = 1f;     

    private Vector3 startPos;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
    }

    void Update()
    {
        FlyPattern();
    }

    void FlyPattern()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime * (movingRight ? 1 : -1), newY, transform.position.z);
    }

    // Optional: if you want it to flip direction when hitting a wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("TurnPoint"))
        {
            Flip();
        }
    }
}