using UnityEngine;
using UnityEngine.Events;

// Detroy the brick object - with anim and sound
// This is working, along with brickParticles.cs for the anim
// Audio is is created as temp object then destroyed
// Hurt Enemies from below added
// Added coin collect if above brick

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/Bonus Block")]
    public class g_Brick : CorgiMonoBehaviour
    {
        [SerializeField]
        private UnityEvent _hit;
        public AudioClip audioClip; // Assign this in the Inspector

        // Hurt enemies from below:
        public LayerMask EnemyLayer; // The layer to detect enemies above the brick
        public float DamageAmount = 1f; // Amount of damage dealt to enemies
        public float InvincibilityDuration = 0.5f; // Duration the enemy is invincible after taking damage


        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            CorgiController controller = collider.GetComponent<CorgiController>();
            if (controller == null)
            {
                return; // Exit if it's not the player
            }

            // Check if the block is hit from below)
            if (collider.transform.position.y < transform.position.y)
            {

                // Call the HurtEnemiesAbove method
                HurtEnemiesAbove();

                // Collect coin if present above the brick
                CollectCoinAbove();

                // Create a new temporary game object to play the audio
                GameObject tempAudioObject = new GameObject("TempAudio");
                AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
                tempAudioSource.clip = audioClip;
                tempAudioSource.Play();

                // Destroy the temporary audio object after the clip has finished playing
                Destroy(tempAudioObject, audioClip.length);

                _hit?.Invoke(); // Invoke the Unity Event (which may disable the main game object)
            }
        }

        private void HurtEnemiesAbove()
        {
            // Define the size of the detection area (adjust based on your brick size)
            Vector2 detectionSize = new Vector2(1.0f, 0.5f); // Adjust the width to match your brick's size
            Vector2 detectionCenter = new Vector2(transform.position.x, transform.position.y + 0.5f);

            // Detect enemies in the area above the brick
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(detectionCenter, detectionSize, 0f, EnemyLayer);

            // Apply damage or destroy the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null && enemyHealth.CanTakeDamageThisFrame())
                {
                    float damageAmount = DamageAmount;
                    GameObject instigator = this.gameObject;
                    float flickerDuration = 0.1f;
                    float invincibilityDuration = InvincibilityDuration;
                    Vector3 damageDirection = Vector3.up;

                    // Call the Damage method with all required parameters
                    enemyHealth.Damage(damageAmount, instigator, flickerDuration, invincibilityDuration, damageDirection);
                }
            }
        }
        private void CollectCoinAbove()
        {
            // Define the size of the detection area based on your brick size
            Vector2 detectionSize = new Vector2(1.0f, 0.5f); // Adjust the width to match your brick's size
            Vector2 detectionCenter = new Vector2(transform.position.x, transform.position.y + 0.5f);

            // Detect coins in the area above the brick
            Collider2D[] hitCoins = Physics2D.OverlapBoxAll(detectionCenter, detectionSize, 0f);

            foreach (Collider2D hit in hitCoins)
            {
                g_Coin coin = hit.GetComponent<g_Coin>();
                if (coin != null)
                {
                    coin.Disappear(); // Collect the coin
                }
            }
        }


    }
}
