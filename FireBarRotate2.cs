using UnityEngine;

// Rotates the fireball/firebar sprites
// Added clockwise toggle
// TODO: Check if update can be implemented betterer 

public class FireBarRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public bool rotateClockwise = true; // Toggle for rotation direction

    void Update()
    {
        // Determine the rotation direction
        float direction = rotateClockwise ? -1f : 1f;

        // Rotate the FireBar GameObject around its center
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }
}
