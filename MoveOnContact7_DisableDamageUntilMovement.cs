using UnityEngine;
using MoreMountains.CorgiEngine; // We need this to find the Damage on Touch script

/*
Turtle Shell stomp code
V7 We disable the attached 'Damage on Touch' script until the shell is moving.   this waz, the enemies walk
past the shell instead of it destroying them.  Script is imported at top via MM
V6 Shell is disabled if leaves camera view. Destroy enemies removed, now using MM destroyOnTouch script.
V5 Shell is destroyed if leaves camera view 
*/

public class MoveOnContact : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of movement
    private bool isContact = false;
    private Vector3 contactPosition;
    private DamageOnTouch damageOnTouch; // Reference to the shell's DamageOnTouch script

    void Start()
    {
        // Get the DamageOnTouch script attached to THIS object and disable it at start
        damageOnTouch = GetComponent<DamageOnTouch>();

        if (damageOnTouch != null)
        {
            damageOnTouch.enabled = false; // Only disable it for this object
        }
    }



    void Update()
    {
        if (isContact)
        {
            // Enable DamageOnTouch when the shell starts moving
            if (damageOnTouch != null && !damageOnTouch.enabled)
            {
                damageOnTouch.enabled = true;
            }

            // Calculate direction based on the contact position relative to the center
            Vector3 direction = contactPosition.x < transform.position.x ? Vector3.right : Vector3.left;

            // Move the object in the determined direction
            transform.position += direction * moveSpeed * Time.deltaTime;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Determine contact position
            contactPosition = other.transform.position; // Get the player's position
            isContact = true; // Set contact flag to true
            // Debug.Log("Contact with Player detected at " + Time.time + " seconds.");

            // Change the layer to "Default"
            gameObject.layer = LayerMask.NameToLayer("Default");

        }
        //else if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) // Muted while we use 'DamageOnTouch' MM Script
        //{
        // Destroy the enemy GameObject
        // Destroy(other.gameObject); // Muted while we use 'DamageOnTouch' MM Script
        // Debug.Log("Enemy destroyed at " + Time.time + " seconds.");
        //}
        else if (other.gameObject.layer == LayerMask.NameToLayer("Platforms")) // Shell is destroyed when hitting platform
        {
            // Destroy the redMartianShell GameObject
            // Destroy(gameObject);
            // Debug.Log("redMartianShell destroyed at " + Time.time + " seconds.");

            // Disable the object instead of destroying it
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        // Destroy the redMartianShell GameObject when it leaves the camera view
        // Destroy(gameObject);
        // Debug.Log("redMartianShell destroyed for leaving camera view at " + Time.time + " seconds.");

        // Disable the object instead of destroying it when it leaves the camera view
        gameObject.SetActive(false);
    }
}
