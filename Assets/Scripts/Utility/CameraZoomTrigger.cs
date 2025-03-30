using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class CameraZoomTrigger : MonoBehaviour
    {

        public GameObject targetArea;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                SpriteRenderer spriteRenderer = targetArea.GetComponent<SpriteRenderer>();
                Camera.main.GetComponent<CameraController>().ShowArea(spriteRenderer.bounds);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Camera.main.GetComponent<CameraController>().ShowPlayer();
            }
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