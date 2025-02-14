using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class CoinCounter : MonoBehaviour, MMEventListener<CorgiEnginePointsEvent>
{
    public Text coinText; // Assign this in the Inspector
    private int coinCount = 0; // Store collected coins

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
