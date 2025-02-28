using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera offset
    public float smoothSpeed = 5f; // Smooth follow speed

    [Header("Camera Zoom Settings")]
    public float zoomSize = 8f; // Default zoom level
    public float zoomSpeed = 2f; // Speed of zoom adjustment

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
        cam.orthographicSize = zoomSize;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Smoothly follow player
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Adjust camera size dynamically (optional)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
        }
    }
}
