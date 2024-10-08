using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

//  This is your modified BonusBlock MM script
//  The block works in conjkunction with g_Coin.cs
//  BLock is animated on impact etc.  Working.
//  ** Enemies now damaged if block hits them.
//  ** Sounds now play if collecting coins (only one sound in this version, which is closer to SMB)

namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/g_BonusBlock")]
    public class g_BonusBlock : CorgiMonoBehaviour, Respawnable
    {
        public GameObject SpawnedObject;
        public int NumberOfAllowedHits = 3;
        public bool ResetOnDeath = false;
        public float SpawnSpeed = 0.5f;
        public Vector3 SpawnDestination;
        public bool AnimateSpawn = true;
        public AnimationCurve MovementCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1f, 1));
        public float BlockMoveUpDistance = 0.2f;  // How far the block moves up
        public float BlockMoveSpeed = 0.1f;      // How fast the block moves up and down
        public LayerMask EnemyLayer; // The layer to detect enemies on top of the block
        public AudioClip audioClip; // Assign this in the Inspector


        [Header("Enemy Damage Settings")]
        [Tooltip("The amount of damage dealt to enemies above the block.")]
        public float DamageAmount = 1f; // Damage amount (adjustable in the inspector)
        [Tooltip("The duration the enemy is invincible after taking damage.")]
        public float InvincibilityDuration = 0.5f; // Duration the enemy is invincible after taking damage
        [Tooltip("The force applied when the enemy is damaged.")]
        public float DamageForce = 0.5f; // The force applied when the enemy is damaged

        // private stuff
        protected Animator _animator;
        protected bool _hit = false;
        protected int _numberOfHitsLeft;
        protected BoxCollider2D _boxCollider2D;

        public virtual void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            _animator = this.gameObject.GetComponent<Animator>();
            _boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
            _numberOfHitsLeft = NumberOfAllowedHits;

            if (_numberOfHitsLeft > 0)
            {
                _animator.SetBool("Off", false);
            }
            else
            {
                _animator.SetBool("Off", true);
            }
        }

        protected virtual void Update()
        {
            UpdateAnimator();
            _hit = false;
        }

        protected virtual void UpdateAnimator()
        {
            _animator.SetBool("Hit", _hit);
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            CorgiController controller = collider.GetComponent<CorgiController>();
            if (controller == null)
            {
                return;
            }

            if (_numberOfHitsLeft == 0)
            {
                return;
            }

            if (collider.transform.position.y < transform.position.y)
            {
                _hit = true;
                _numberOfHitsLeft--;

                // Hurt enemies above the block
                HurtEnemiesAbove();

                // Move the block upwards and back down
                StartCoroutine(MoveBlockUpAndDown());

                // Spawn the object (coin, etc.)
                GameObject spawned = (GameObject)Instantiate(SpawnedObject);
                spawned.transform.position = transform.position;
                spawned.transform.rotation = Quaternion.identity;

                // Play the audio clip
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogWarning("AudioSource is missing on this GameObject.");
                    return; // Exit early if there's no AudioSource
                }

                if (audioClip == null)
                {
                    Debug.LogWarning("audioClip is not assigned!");
                    return; // Exit early if there's no audioClip
                }

                audioSource.PlayOneShot(audioClip); // Play the sound when the block is hit

                if (AnimateSpawn)
                {
                    // Call a coroutine to animate and then make the coin disappear
                    StartCoroutine(AnimateAndDisappear(spawned));
                }
                else
                {
                    spawned.transform.position = transform.position + SpawnDestination;
                }
            }

            if (_numberOfHitsLeft == 0)
            {
                _animator.SetBool("Off", true);
            }
        }

        private void HurtEnemiesAbove()
        {
            // Define the size of the detection area (adjust based on your block size)
            Vector2 detectionSize = new Vector2(_boxCollider2D.size.x, 0.5f);
            Vector2 detectionCenter = new Vector2(transform.position.x, transform.position.y + _boxCollider2D.size.y);

            // Detect enemies in the area above the block
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


        private IEnumerator AnimateAndDisappear(GameObject spawned)
        {
            // Animate the movement of the coin
            yield return StartCoroutine(MMMovement.MoveFromTo(spawned, transform.position,
                new Vector2(transform.position.x + SpawnDestination.x, transform.position.y + _boxCollider2D.size.y + SpawnDestination.y),
                SpawnSpeed, MovementCurve));

            // After the movement is complete, call the Disappear method on the coin
            g_Coin coin = spawned.GetComponent<g_Coin>();
            if (coin != null)
            {
                coin.Disappear();
            }
        }

        protected IEnumerator MoveBlockUpAndDown()
        {
            Vector3 originalPosition = transform.position;
            Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + BlockMoveUpDistance, originalPosition.z);

            // Move the block up
            float elapsedTime = 0f;
            while (elapsedTime < BlockMoveSpeed)
            {
                transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / BlockMoveSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Move the block back down
            elapsedTime = 0f;
            while (elapsedTime < BlockMoveSpeed)
            {
                transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / BlockMoveSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set the block back to its original position, just in case it's off by a fraction
            transform.position = originalPosition;
        }

        public virtual void OnPlayerRespawn(CheckPoint checkpoint, Character player)
        {
            if (ResetOnDeath)
            {
                Initialization();
            }
        }
    }
}
