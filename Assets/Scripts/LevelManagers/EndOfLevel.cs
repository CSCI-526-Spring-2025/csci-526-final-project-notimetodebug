using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
        || (other.transform.parent && other.transform.parent.gameObject.CompareTag("Player")))
        {
            FindObjectOfType<LevelManager>().NextLevel();
        }
    }
}
