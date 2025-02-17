using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

// A Script for keeping count of our lovely coins
// V2 - Once coins = 100, + 1up, coun count = 0

public class CoinCounter : MonoBehaviour, MMEventListener<CorgiEnginePointsEvent>
{
    public Text coinText; // Assign this in the Inspector
    private int coinCount = 0; // Store collected coins
    private const int CoinsForExtraLife = 100; // Coins needed for an extra life

    void OnEnable()
    {
        // Register for Corgi Engine's event system
        this.MMEventStartListening<CorgiEnginePointsEvent>();
    }

    void OnDisable()
    {
        // Unregister when the object is disabled
        this.MMEventStopListening<CorgiEnginePointsEvent>();
    }

    // This method is triggered when a points event occurs
    public void OnMMEvent(CorgiEnginePointsEvent pointsEvent)
    {
        if (pointsEvent.PointsMethod == PointsMethods.Add)
        {
            coinCount += pointsEvent.Points;

            // If player reaches 100 coins, grant an extra life and reset count
            if (coinCount >= CoinsForExtraLife)
            {
                GameManager.Instance.GainLives(1);
                coinCount = 0; // Reset coin count after extra life
            }

            UpdateCoinUI();
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount.ToString();
        }
    }
}
