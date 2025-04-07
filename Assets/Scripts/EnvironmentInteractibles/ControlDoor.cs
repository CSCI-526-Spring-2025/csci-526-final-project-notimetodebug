using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EnvironmentInteractibles;
using UnityEngine;

public class ControlDoor : MonoBehaviour, IItemController
{
    public bool activativeOnTrigger;
    private bool isItemActive = false;

    public bool IsItemActive()
    {
        return isItemActive;
    }

    [ContextMenu("Trigger")]
    private void Trigger()
    {
        if (activativeOnTrigger)
        {
            isItemActive = true;
        }
        else
        {
            isItemActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Trigger();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isItemActive = !activativeOnTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
