using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EnvironmentInteractibles;
using Unity.VisualScripting;
using UnityEngine;

public class TimedButton : MonoBehaviour, IItemController
{
    public float triggerTime = 2;

    private bool isButtonPressed = false;
    private float buttonReleaseTime = 0;

    private void TurnOnButton()
    {
        isButtonPressed = true;
        var triggerObject = gameObject.transform.Find("Trigger").gameObject;
        var buttonColor = Color.green;
        triggerObject.GetComponent<SpriteRenderer>().color = buttonColor;
    }

    private void TurnOffButton()
    {
        isButtonPressed = false;
        var triggerObject = gameObject.transform.Find("Trigger").gameObject;
        var buttonColor = Color.red;
        triggerObject.GetComponent<SpriteRenderer>().color = buttonColor;
    }

    [ContextMenu("Trigger Button")]
    public void TriggerButton()
    {
        TurnOnButton();
        buttonReleaseTime = Time.time + triggerTime;
    }

    public bool IsItemActive()
    {
        return isButtonPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerButton();

        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.OnAbsorbed();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isButtonPressed && Time.time > buttonReleaseTime)
        {
            TurnOffButton();
        }
    }
}
