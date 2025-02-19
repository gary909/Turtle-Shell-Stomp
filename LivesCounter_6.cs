using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System;

// For tracking the amount of player Lives and displaying it on the canvas:
/*
    V6 Lives counting UI text now working
    V5 Lives counting UI text moved to here from g_HealthBar V9 and GameManager_Edit.cs
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

    /*private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.OnLivesChanged == null)
            {
                GameManager.Instance.OnLivesChanged = delegate { };
            }

            GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Prevent duplicate subscriptions
            GameManager.Instance.OnLivesChanged += UpdateLivesUI;
            Debug.Log(":-) LivesCounter successfully subscribed to OnLivesChanged.");
        }
        else
        {
            Debug.LogError(":-( GameManager instance is NULL in LivesCounter.cs! UI will not update.");
        }
    }*/

    /*private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Prevent duplicate subscriptions
            GameManager.Instance.OnLivesChanged += UpdateLivesUI;
            Debug.Log(":-) LivesCounter successfully subscribed to OnLivesChanged.");
        }
        else
        {
            Debug.LogError(":-( GameManager instance is NULL in LivesCounter.cs! UI will not update.");
        }
    }*/

    /*private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();

        if (GameManager.Instance == null)
        {
            Debug.LogError(":-( GameManager instance is NULL in LivesCounter.cs! UI will not update.");
            return;
        }

        // Ensure OnLivesChanged is not NULL
        if (GameManager.Instance.OnLivesChanged == null)
        {
            GameManager.Instance.OnLivesChanged = delegate { };
        }

        GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Prevent duplicate subscriptions
        GameManager.Instance.OnLivesChanged += UpdateLivesUI;
        Debug.Log($":-) LivesCounter successfully subscribed to OnLivesChanged. Subscribers: {GameManager.Instance.OnLivesChanged.GetInvocationList().Length}");
    }*/

    private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();

        if (GameManager.Instance == null)
        {
            Debug.LogError(":-( GameManager instance is NULL in LivesCounter.cs! UI will not update.");
            return;
        }

        //  Subscribe properly without trying to assign
        GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Prevent duplicate subscriptions
        GameManager.Instance.OnLivesChanged += UpdateLivesUI;

        Debug.Log($":-) LivesCounter successfully subscribed to OnLivesChanged. Subscribers: REMOVED THIS AS CAUSING ERROR");
    }





    /*private void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
        GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Unsubscribe to prevent errors
    }*/

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= UpdateLivesUI;
            Debug.Log(":-( LivesCounter unsubscribed from OnLivesChanged.");
        }
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

    /*private void UpdateLivesUI()
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
    }*/

    private void UpdateLivesUI()
    {
        if (livesText != null && GameManager.Instance != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.CurrentLives;
            Debug.Log($":-) LivesCounter UI Updated: {GameManager.Instance.CurrentLives}");
        }
        else
        {
            Debug.LogError(":-( LivesCounter: livesText or GameManager.Instance is NULL! UI not updating.");
        }
    }



}
