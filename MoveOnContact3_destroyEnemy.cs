using UnityEngine;

public class MoveOnContact : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of movement
    private bool isContact = false;
    private Vector3 contactPosition;

    void Update()
    {
        if (isContact)
        {
            // Calculate direction based on the contact position relative to the center
            Vector3 direction = contactPosition.x < transform.position.x ? Vector3.right : Vector3.left;

            // Move the object in the determined direction
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Optionally stop moving after a certain distance or condition
            // For example, you can add logic to stop the movement after a certain time or distance
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Determine contact position
            contactPosition = other.transform.position; // Get the player's position
            isContact = true; // Set contact flag to true
            Debug.Log("Contact with Player detected at " + Time.time + " seconds.");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            // Destroy the enemy GameObject
            Destroy(other.gameObject);
            Debug.Log("Enemy destroyed at " + Time.time + " seconds.");
        }
    }
}
