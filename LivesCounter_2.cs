using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

// For tracking the amount of player Lives and displaying it on the canvas:
/*
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
        // Update UI when the player dies or respawns
        if (engineEvent.EventType == CorgiEngineEventTypes.PlayerDeath ||
            engineEvent.EventType == CorgiEngineEventTypes.Respawn)
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


public class LivesCounter : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public Text livesText; // Assign this in the Inspector

    private void Start()
    {
        UpdateLivesUI(); // Update UI at game start
    }

    private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>(); // Listen for lives-related events
    }

    private void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        // Update UI when a life is lost or gained
        if (engineEvent.EventType == CorgiEngineEventTypes.PlayerDeath || engineEvent.EventType == CorgiEngineEventTypes.Respawn)
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
