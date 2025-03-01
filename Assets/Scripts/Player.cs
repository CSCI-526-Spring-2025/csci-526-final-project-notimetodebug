using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Creature
{
    private Rigidbody2D rb;

    private Vector3 fireDirection;

    [SerializeField] private int currentGunIndex;

    [SerializeField] private List<Gun> guns;

    private Transform levelStart;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < guns.Count; i++)
        {
            if (i != currentGunIndex)
            {
                guns.ElementAt(i).OnUnequipped();
            }
            else
            {
                guns.ElementAt(i).OnEquipped();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (levelStart == null)
        {
            levelStart = GameObject.Find("LevelStart").transform;
        }

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
            currentGun.OnUnequipped();
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns.ElementAt(currentGunIndex);
            currentGun.SetDirection(fireDirection);
            currentGun.OnEquipped();
        }
        if (HP <= 0){
           Respawn();
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
        gun.OnPickedUp(this);
        
        if (currentGunIndex == guns.Count - 1)
        {
            gun.OnEquipped();
        }
        else
        {
            gun.OnUnequipped();
        }
    }

    private void Respawn(){
        transform.position = levelStart.position;
        HP = maxHP;
    }

}