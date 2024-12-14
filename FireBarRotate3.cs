using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotates the fireball/firebar sprites
// Added clockwise toggle
// Added coroutine for betterer performance
// FPS now back at 300 (in combination with other improvements)

public class g_FireBarRotate : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public bool rotateClockwise = true;
    public float updateInterval = 0.02f; // Interval between rotations

    void Start()
    {
        StartCoroutine(RotateFireBar());
    }

    IEnumerator RotateFireBar()
    {
        while (true)
        {
            float direction = rotateClockwise ? -1f : 1f;
            transform.Rotate(0, 0, direction * rotationSpeed * updateInterval);
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
