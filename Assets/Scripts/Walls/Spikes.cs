using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Creature creature))
        {
            creature.HP -= 40;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
