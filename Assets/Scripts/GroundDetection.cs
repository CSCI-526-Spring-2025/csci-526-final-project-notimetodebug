using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public Enemy enemy;
    private int groundContactCount = 0; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            groundContactCount++; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            groundContactCount--;

            if (groundContactCount <= 0) 
            {
                enemy.Flip();
            }
        }
    }
}
