using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

// For tracking the amount of player Lives and displaying it on the canvas:
/*
    V3 - When player runs out of lives, the game immediatly ends, rather than the UI lives jumping back up to the starting lives amount
        (Also changed GameManager lines 258 to 262)
    V2 - Lives UI text wasn't updating so this version fixes that.  Also changed 'GameManager.cs' lines 168, 169, 269
    V1 - Tracks amount of extra lives the player currently has.
*/


public class LivesCounter : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public Text livesText; // Assign this in the Inspector

    private void Start()
    {
        UpdateLivesUI(); // Ensure UI updates when the game starts
    }

    private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>(); // Listen for deaths and respawns
        GameManager.Instance.OnLivesChanged += UpdateLivesUI; // Subscribe to lives updates
    }

    private void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
        GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Unsubscribe to prevent errors
    }

    public void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        // Update UI when the player dies, respawns, or triggers game over
        if (engineEvent.EventType == CorgiEngineEventTypes.PlayerDeath ||
            engineEvent.EventType == CorgiEngineEventTypes.Respawn ||
            engineEvent.EventType == CorgiEngineEventTypes.GameOver)
        {
            UpdateLivesUI();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.CurrentLives;
        }
    }
}
