using UnityEngine;

public class WallRotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    void Update()
    {
        foreach (Transform child in transform) // Loop through all child objects
        {
            child.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Rotate each wall individually
        }
    }
}
