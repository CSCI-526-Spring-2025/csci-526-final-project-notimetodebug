using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Player player; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera offset
    public float smoothSpeed = 5f; // Smooth follow speed

    [Header("Camera Zoom Settings")] public float baseZoom = 6f; // Default zoom level
    public float flyingZoom = 9f; // Maximum zoom level when airborne
    public float zoomSpeed = 1f; // Speed of zoom adjustment
    private float fixedZoom = 1f;
    private bool isZoomFixed = false;

    private bool showArea = false;
    private Bounds areaBounds;

    private Camera cam;


    public void ShowArea(Bounds bounds)
    {
        showArea = true;
        areaBounds = bounds;
    }

    public void ShowPlayer()
    {
        showArea = false;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = baseZoom;
    }

    void LateUpdate()
    {
        if(showArea)
        {
            float xExtent = areaBounds.extents.x;
            float yExtent = areaBounds.extents.y;
            float centerX = areaBounds.center.x;
            float centerY = areaBounds.center.y;

            Vector3 targetPosition = new Vector3(centerX, centerY, transform.position.z);
            float targetZoom = Mathf.Max(xExtent, yExtent);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
            return;
        }

        if (player != null)
        {
            // Smoothly follow the player
            Vector3 targetPosition = player.GetPosition() + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Adjust camera zoom based on airborne status
            AdjustZoom();
        }
    }

    public void SetFixedZoom(float fixedZoom)
    {
        isZoomFixed = true;
        this.fixedZoom = fixedZoom;
    }

    public void ReleaseFixedZoom()
    {
        isZoomFixed = false;
    }

    public void AdjustZoom()
    {
        float targetZoom;
        if (isZoomFixed)
        {
            targetZoom = fixedZoom;
        }
        else
        {
            targetZoom = player.GetVelocity().y == 0 ? baseZoom : flyingZoom;
        }

        // Smoothly transition to new zoom size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}