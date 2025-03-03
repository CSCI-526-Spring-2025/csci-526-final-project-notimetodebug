using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera offset
    public float smoothSpeed = 5f; // Smooth follow speed

    [Header("Camera Zoom Settings")]
    public float baseZoomSize = 6f; // Default zoom level
    public float maxZoomOut = 9f;  // Maximum zoom level when airborne
    public float zoomSpeed = 1f; // Speed of zoom adjustment

    private Camera cam;
    private bool isAirborne = false; // Tracks if player is in the air

    void Start()
    {
        cam = GetComponent<Camera>(); 
        cam.orthographicSize = baseZoomSize;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Smoothly follow the player
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Adjust camera zoom based on airborne status
            AdjustZoom();
        }
    }

    void AdjustZoom()
    {
        float targetZoom = isAirborne ? maxZoomOut : baseZoomSize;
        
        // Smoothly transition to new zoom size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    public void SetAirborne(bool airborne)
    {
        isAirborne = airborne;
    }
}
