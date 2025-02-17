using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

// For tracking the amount of player Lives and displaying it on the canvas:
/*
    V4 - trying to get lives UI text to update
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
        //UpdateLivesUI(); // Ensure UI updates when the game starts
        Invoke("UpdateLivesUI", 0.1f); // Delay UI update to wait for GameManager
    }

    private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>(); // Listen for deaths and respawns
        GameManager.Instance.OnLivesChanged += UpdateLivesUI; // Subscribe to lives updates

        Debug.Log("LivesCounter subscribed to OnLivesChanged event.");
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
            //engineEvent.EventType == CorgiEngineEventTypes.Respawn ||
            engineEvent.EventType == CorgiEngineEventTypes.GameOver)
        {
            UpdateLivesUI();
        }
    }

private void UpdateLivesUI()
{
    if (livesText != null)
    {
        int lives = GameManager.Instance.CurrentLives;
        livesText.text = "Lives: " + lives;
        Debug.Log("LivesCounter UI Updated: " + lives);
    }
    else
    {
        Debug.LogError("LivesCounter: LivesText reference is missing!");
    }
}

}
