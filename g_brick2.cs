using UnityEngine;
using UnityEngine.Events;

// Detroy the brick object - with anim and sound
// This is working, along with brickParticles.cs for the anim
// Audio is is created as temp object then destroyed

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/Bonus Block")]
    public class g_Brick : CorgiMonoBehaviour
    {
        [SerializeField]
        private UnityEvent _hit;
        public AudioClip audioClip; // Assign this in the Inspector

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
    }
}
