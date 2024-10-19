using UnityEngine;

// No longer used

public class PlayerController : MonoBehaviour
{
    private BrickInteractionHandler brickHandler;

    private void Start()
    {
        // Get the BrickInteractionHandler component attached to the player
        brickHandler = GetComponent<BrickInteractionHandler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a brick
        if (collision.gameObject.CompareTag("Brick"))
        {
            // Ensure the BrickInteractionHandler component is available
            if (brickHandler != null)
            {
                // Call the method and pass the brick game object
                brickHandler.OnPlayerHitBrick(collision.gameObject);
            }
        }
    }

    // Example method to call when the player jumps
    public void Jump()
    {
        // Call the OnPlayerJump method from the BrickInteractionHandler
        if (brickHandler != null)
        {
            brickHandler.OnPlayerJump();
        }

        // Your existing jump logic
        // ...
    }

    // Example method to call when the player lands
    public void Land()
    {
        // Call the OnPlayerLand method from the BrickInteractionHandler
        if (brickHandler != null)
        {
            brickHandler.OnPlayerLand();
        }

        // Your existing landing logic
        // ...
    }
}
