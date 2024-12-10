using System.Collections;
using UnityEngine;

public class g_RevolvingPlatform : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public bool rotateClockwise = true;
    public float updateInterval = 0.02f; // Interval between rotations
    public Transform platform;          // The platform to move
    public Transform pivot;             // The center point around which the platform revolves

    private float currentAngle = 0f;

    void Start()
    {
        if (platform == null || pivot == null)
        {
            Debug.LogError("Platform or Pivot is not assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(RevolvePlatform());
    }

    IEnumerator RevolvePlatform()
    {
        while (true)
        {
            float direction = rotateClockwise ? -1f : 1f;

            // Update the angle
            currentAngle += direction * rotationSpeed * updateInterval;

            // Keep the angle within 0-360 degrees
            currentAngle %= 360f;

            // Calculate the new position for the platform
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * Vector3.Distance(platform.position, pivot.position);

            // Update platform's position
            platform.position = pivot.position + offset;

            // Ensure the platform remains level
            platform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
