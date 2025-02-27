using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Creature
{
    private Rigidbody2D rb;

    private Vector3 fireDirection;

    [SerializeField] private int currentGunIndex;

    [SerializeField] private List<Gun> guns;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < guns.Count; i++)
        {
            if (i != currentGunIndex)
            {
                guns.ElementAt(i).Deactivate();
            }
            else
            {
                guns.ElementAt(i).Activate();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        fireDirection = (mousePos - transform.position).normalized;

        Gun currentGun = guns.ElementAt(currentGunIndex);

        currentGun.SetDirection(fireDirection);

        Vector3 recoilForce = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            recoilForce = currentGun.StartFire(fireDirection);
        }
        else if (Input.GetMouseButton(0))
        {
            recoilForce = currentGun.KeepFire(fireDirection);
        }
        else
        {
            recoilForce = currentGun.StopFire(fireDirection);
        }

        rb.AddForce(recoilForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentGun.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentGun.Deactivate();
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns.ElementAt(currentGunIndex);
            currentGun.SetDirection(fireDirection);
            currentGun.Activate();
        }
    }

    public void PickUpGun(Gun gun)
    {
        if (guns.Count > 1)
        {
            guns.ElementAt(1).Destroy();
            guns.RemoveAt(1);
        }
        guns.Add(gun);
        
        if (currentGunIndex == guns.Count - 1)
        {
            gun.Activate();
        }
        else
        {
            gun.Deactivate();
        }
    }
}