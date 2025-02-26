using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            // Reflect the bullet. Sample code for later use

            //Vector2 normal = collision.GetContact(0).normal;
            //Vector2 incomingVector = collision.relativeVelocity;
            //Vector2 reflectedVector = Vector2.Reflect(incomingVector, normal);

            // Set bullet velocity to the reflected vector
            // Reduce number of remaining bounces by 1
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
