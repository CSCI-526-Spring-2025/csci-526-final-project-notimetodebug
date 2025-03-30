using System.Collections;
using UnityEngine;

namespace Assets.Scripts.EnvironmentInteractibles
{
    public class FlipButton : MonoBehaviour, IDoorController
    {
        private bool isButtonPressed = false;

        public bool IsDoorOpen() {
            return isButtonPressed;
        }

        [ContextMenu("Trigger Button")]
        public void TriggerButton()
        {
            var triggerObject = gameObject.transform.Find("Trigger").gameObject;
            var buttonColor = Color.green;
            triggerObject.GetComponent<SpriteRenderer>().color = buttonColor;
            isButtonPressed = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerButton();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}