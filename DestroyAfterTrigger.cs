using UnityEngine;
using System.Collections;
using MoreMountains.CorgiEngine;

// Script destroys game object after user assigned delay

public class DestroyAfterTrigger : MonoBehaviour
{
    public float delay = 2f; // Time before the object is destroyed

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if it's the player hitting the block
        CorgiController controller = collider.GetComponent<CorgiController>();
        if (controller != null)
        {
            StartCoroutine(DestroyObjectCoroutine());
        }
    }

    private IEnumerator DestroyObjectCoroutine()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
