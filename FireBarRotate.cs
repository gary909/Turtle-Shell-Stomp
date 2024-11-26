using UnityEngine;

// Rotates the fireball/firebar sprites
// TODO: Check if update can be implemented betterer 

public class FireBarRotate : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation

    void Update()
    {
        // Rotate the FireBar GameObject around its center
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
