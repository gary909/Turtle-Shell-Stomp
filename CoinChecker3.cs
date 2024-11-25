using System.Collections;
using UnityEngine;

//  Checks if the above brick coin is still present.
//  If not present, it disables the gameObject that
//  spawns the coin jumping out of brick

//  V2 was working but 'update' was being called every frame,
//  This script hopefully improves that

public class CoinChecker : MonoBehaviour
{
    // Coin object that needs to be check wether is present
    public GameObject coinAboveBrick;
    // Reference to the HitCoinFromBelowBrick game object
    // This is the game object that gets disabled if the coin 
    public GameObject hitCoinFromBelowBrick;

    private bool isCoinChecked = false;

    void Start()
    {
        StartCoroutine(CheckCoinStatusPeriodically());
    }

    private IEnumerator CheckCoinStatusPeriodically()
    {
        while (!isCoinChecked)
        {
            CheckCoinStatus();
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    private void CheckCoinStatus()
    {
        // Check if the child object is not active
        if (coinAboveBrick != null && !coinAboveBrick.activeSelf)
        {
            // Disable the HitCoinFromBelowBrick game object
            if (hitCoinFromBelowBrick != null && hitCoinFromBelowBrick.activeSelf)
            {
                hitCoinFromBelowBrick.SetActive(false);
                isCoinChecked = true;
                //Debug.Log("HitCoinFromBelowBrick game object has been disabled.");
            }
        }
    }
}
