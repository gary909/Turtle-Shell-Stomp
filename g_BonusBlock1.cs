using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

// WORKING BUT NOT ANIMATED //


namespace MoreMountains.CorgiEngine
{
    [AddComponentMenu("Corgi Engine/Environment/g_BonusBlock")]
    public class g_BonusBlock : CorgiMonoBehaviour, Respawnable
    {
        public GameObject SpawnedObject;
        public int NumberOfAllowedHits = 3;
        public bool ResetOnDeath = false;
        public float SpawnSpeed = 0.2f;
        public Vector3 SpawnDestination;
        public bool AnimateSpawn = true;
        public AnimationCurve MovementCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1f, 1));

        protected Animator _animator;
        protected bool _hit = false;
        protected Vector2 _newPosition;
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

                GameObject spawned = (GameObject)Instantiate(SpawnedObject);
                spawned.transform.position = transform.position;
                spawned.transform.rotation = Quaternion.identity;

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

        public virtual void OnPlayerRespawn(CheckPoint checkpoint, Character player)
        {
            if (ResetOnDeath)
            {
                Initialization();
            }
        }
    }
}
