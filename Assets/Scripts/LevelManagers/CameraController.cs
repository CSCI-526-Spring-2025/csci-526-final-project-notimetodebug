using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera offset
    public float smoothSpeed = 5f; // Smooth follow speed

    [Header("Camera Zoom Settings")]
    public float baseZoomSize = 5f; // Default zoom level
    public float maxZoomOut = 8f;  // Maximum zoom level when jumping
    public float minZoomIn = 4f;    // Minimum zoom level when grounded
    public float heightFactor = 0.5f; // How much the zoom changes based on height
    public float zoomSpeed = 2f; // Speed of zoom adjustment

    private Camera cam;
    private float initialY; // Store the player's initial Y position

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
        cam.orthographicSize = baseZoomSize;
        initialY = player.position.y; // Set the initial height reference
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Smoothly follow the player
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Adjust camera zoom based on jump height
            AdjustZoom();
        }
    }

    void AdjustZoom()
    {
        float playerHeight = player.position.y - initialY; // Get player's height difference
        float targetZoom = baseZoomSize + playerHeight * heightFactor; // Adjust zoom dynamically

        // Clamp zoom between min and max values
        targetZoom = Mathf.Clamp(targetZoom, minZoomIn, maxZoomOut);

        // Smoothly transition to new zoom size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
