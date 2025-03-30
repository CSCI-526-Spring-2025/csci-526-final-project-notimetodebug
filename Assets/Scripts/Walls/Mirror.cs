using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Wall
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnBulletCollision(Bullet bullet)
    {
        bullet.OnBounce();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
