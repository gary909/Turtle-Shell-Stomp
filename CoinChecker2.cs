using UnityEngine;

//  Checks if the above brick coin is still present.
//  If not present, it disables the gameObject that
//  spawns the coin jumping out of brick

public class CoinChecker : MonoBehaviour
{
    // Coin object that needs to be check wether is present
    public GameObject coinAboveBrick;
    // Reference to the HitCoinFromBelowBrick game object
    // This is the game object that gets disabled if the coin 
    public GameObject hitCoinFromBelowBrick;

    void Update()
    {
        // Check if the child object is not active
        if (coinAboveBrick != null && !coinAboveBrick.activeSelf)
        {
            // Disable the HitCoinFromBelowBrick game object
            if (hitCoinFromBelowBrick != null && hitCoinFromBelowBrick.activeSelf)
            {
                hitCoinFromBelowBrick.SetActive(false);
                //Debug.Log("HitCoinFromBelowBrick game object has been disabled.");
            }
        }
    }
}
