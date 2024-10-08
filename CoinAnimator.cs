using System.Collections;
using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [Tooltip("How far the coin moves up when it appears")]
    public float CoinMoveUpDistance = 1.5f;

    [Tooltip("How fast the coin moves up and disappears")]
    public float CoinMoveSpeed = 0.5f;

    [Tooltip("Delay before disabling the coin")]
    public float DisableDelay = 0.1f;

    private bool _hasMoved = false;

    public void AnimateCoin()
    {
        Debug.Log("CoinAnimator: AnimateCoin() called");
        if (!_hasMoved)
        {
            Debug.Log("Coin animation triggered");
            StartCoroutine(AnimateCoinAndDisappear());
            _hasMoved = true;
        }
        else
        {
            Debug.Log("Coin animation already triggered");
        }
    }

    private IEnumerator AnimateCoinAndDisappear()
    {
        Debug.Log("Coin animation started");

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + CoinMoveUpDistance, originalPosition.z);

        float elapsedTime = 0f;
        while (elapsedTime < CoinMoveSpeed)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / CoinMoveSpeed));
            elapsedTime += Time.deltaTime;
            Debug.Log("Coin moving up... " + transform.position);
            yield return null;
        }

        Debug.Log("Coin finished moving up");

        // Wait before disabling
        yield return new WaitForSeconds(DisableDelay);
        Debug.Log("Coin will now be disabled");

        // Disable the coin instead of destroying
        gameObject.SetActive(false);

        Debug.Log("Coin is now disabled");
    }
}
