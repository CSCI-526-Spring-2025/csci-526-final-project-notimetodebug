using System;
using UnityEngine;

public class BulletDamageTrigger : MonoBehaviour
{
    private Bullet bullet;

    private void Start()
    {
        bullet = gameObject.transform.parent.GetComponent<Bullet>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<IBulletIteractable>(out IBulletIteractable bulletIteractable))
        {
            Debug.Log("damaging");
            bulletIteractable.OnBulletCollision(bullet);
        }
    }
}