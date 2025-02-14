using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

// For tracking the amount of player Lives and displaying it on the canvas

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
