using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float amplitude = 1f;
    public float frequency = 1f;

    private float startY;
    private float elapsedTime = 0f;

    protected override void Start()
    {
        base.Start();
        startY = transform.position.y;
    }

    


    public override void Move()
    {
        elapsedTime += Time.deltaTime;
        float verticalOffset = Mathf.Sin(elapsedTime * frequency) * amplitude;
        float verticalVelocity = (verticalOffset - (rb.position.y - startY)) / Time.deltaTime;
        Debug.Log("Current Direction: " + (movingRight ? "Right" : "Left") + ", Velocity: " + rb.velocity.x);
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, verticalVelocity);

    }

    //public override void Flip()
    //{
    //    base.Flip(); // still flip movingRight and rotate
    //    Debug.Log("FlyingEnemy Flip called. Resetting elapsedTime!");
    //    elapsedTime = 0f; // <-- Reset elapsed time so sine wave doesn't jump weirdly
    //}


}
