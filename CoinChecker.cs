using UnityEngine;

//  Checks if the above brick coin is still present.  If not: logs it

public class CoinChecker : MonoBehaviour
{
    // Reference to the child object
    public GameObject coinAboveBrick;

    void Update()
    {
        // Check if the child object is not active
        if (coinAboveBrick != null && !coinAboveBrick.activeSelf)
        {
            Debug.Log("G_coin_aboveBrick is disabled.");
        }
    }
}
