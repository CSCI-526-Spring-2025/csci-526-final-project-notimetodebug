using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EnvironmentInteractibles;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{

    public List<GameObject> doorControllersObjects;
    private List<IDoorController> doorControllers;

    private Animator animator;
    private bool isDoorOpen = true;

    private bool ShouldDoorOpen()
    {
        List<bool> controllerStates = doorControllers
            .Where(controller => controller != null)
            .Select(controller => controller.IsDoorOpen())
            .ToList();

        return !controllerStates.Contains(false);
    }

    private bool ShouldStateChange()
    {
        return ShouldDoorOpen() != isDoorOpen;
    }

    private void ChangeDoorState(bool isOpen)
    {
        isDoorOpen = !isOpen;
        animator.SetBool("Open", !isOpen);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Open", isDoorOpen);
        doorControllers = doorControllersObjects
            .Select(doorControllersObject => doorControllersObject?.GetComponent<IDoorController>())
            .Where(controller => controller != null)
            .ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldStateChange())
        {
            ChangeDoorState(isDoorOpen);
        }
    }
}
