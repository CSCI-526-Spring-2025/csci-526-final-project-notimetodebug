using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EnvironmentInteractibles;
using UnityEngine;

public class ControlDoor : MonoBehaviour, IDoorController
{
    public bool openOnTrigger;
    private bool isDoorOpen = false;

    public bool IsDoorOpen()
    {
        return isDoorOpen;
    }

    [ContextMenu("Trigger")]
    private void Trigger()
    {
        if (openOnTrigger)
        {
            isDoorOpen = true;
        }
        else
        {
            isDoorOpen = false;
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
        isDoorOpen = !openOnTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
