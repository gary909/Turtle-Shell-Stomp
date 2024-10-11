using UnityEngine;
using UnityEngine.Events;

// Detroy the brick object - need to add anim

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/Bonus Block")]
    public class g_Brick : CorgiMonoBehaviour
    {
        [SerializeField]
        private UnityEvent _hit;

        /// <param name="collider">The collider that collides with the platform.</param>		
        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            CorgiController controller = collider.GetComponent<CorgiController>();
            if (controller == null)
            {
                return; // Exit if it's not the player
            }

            // Check if the collider's y position is less than the block's y position (hit from below)
            if (collider.transform.position.y < transform.position.y)
            {
                // Destroy the brick game object
                Debug.Log("Brick destroyed!"); // Log when the brick is destroyed
                //Destroy(gameObject);
                _hit?.Invoke();
            }
        }
    }
}