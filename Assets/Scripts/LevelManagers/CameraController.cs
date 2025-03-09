using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Player player; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera offset
    public float smoothSpeed = 5f; // Smooth follow speed

    [Header("Camera Zoom Settings")] public float baseZoom = 6f; // Default zoom level
    public float maxZoom = 9f; // Maximum zoom level when airborne
    public float zoomSpeed = 1f; // Speed of zoom adjustment

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = baseZoom;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Smoothly follow the player
            Vector3 targetPosition = player.GetPosition() + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Adjust camera zoom based on airborne status
            AdjustZoom();
        }
    }

    void AdjustZoom()
    {
        float targetZoom = player.GetVelocity().y == 0 ? baseZoom : maxZoom;

        // Smoothly transition to new zoom size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}