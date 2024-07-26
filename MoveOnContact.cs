using UnityEngine;

public class MoveOnContact : MonoBehaviour
{
    public float moveDistance = 5f; // Distance to move along the X axis
    public float moveSpeed = 2f; // Speed of movement

    private bool isContact = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isContact)
        {
            // Move the object along the X axis
            Vector3 targetPosition = new Vector3(startPosition.x + moveDistance, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Optionally stop moving when the target position is reached
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isContact = false; // Reset contact flag if you only want to move once
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = true;
            Debug.Log("Contact with Player detected at " + Time.time + " seconds.");
        }
    }
}
