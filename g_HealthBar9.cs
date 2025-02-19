using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;

// V9 THE GREAT REMOVE. UI Lives text is removed from here. Will use LivesCounter.cs (v5) and GameManager.cs
// V8 Still testing, with update (at bottom) uncommented, it's semi-working.
// v7 work in progress to get lives to update properly
// V6 adds character glide ability check *(in conjuction with Health.cs - see Health_1.cs in repo for changes explanation)
// v5 Scripts FPS rate improved with this version (compared to v4)
// Not fully checked for any bugs just yet

// Scripts FPS rate improved with this version (compared to v4)
// Not fully checked for any bugs just yet

public class g_HealthBar : MonoBehaviour, MMEventListener<HealthChangeEvent>
{
    public Image Heart_HealthLast;
    public Image Heart_HealthMid;
    public Image Heart_HealthFull;
    public Text LivesText;  // No longer used for lives UI updates

    private Health PlayerHealth;
    private int maxHealth = 3;

    void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    private IEnumerator InitializeAfterDelay()
    {
        float timeout = 5f;
        float elapsedTime = 0f;

        while ((GameManager.Instance == null || PlayerHealth == null) && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerHealth = player.GetComponent<Health>();
                if (PlayerHealth != null)
                {
                    InitializeHealthBar();
                }
            }
            yield return null;
        }

        if (elapsedTime >= timeout)
        {
            Debug.LogWarning("Initialization timed out.");
        }
        else
        {
            Debug.Log("g_HealthBar.cs Initialization complete, CurrentLives: " + GameManager.Instance.CurrentLives);
        }
    }

    private void InitializeHealthBar()
    {
        float initialHealth = PlayerHealth.CurrentHealth;
        float scaledHealth = Mathf.Ceil(initialHealth / PlayerHealth.MaximumHealth * maxHealth);
        UpdateHealthBar(scaledHealth);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        Heart_HealthFull.enabled = (currentHealth >= maxHealth);
        Heart_HealthMid.enabled = (currentHealth >= 2);
        Heart_HealthLast.enabled = (currentHealth >= 1);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.UpdateGlideAbility();
            }
        }
    }

    public void OnMMEvent(HealthChangeEvent healthChangeEvent)
    {
        if (healthChangeEvent.AffectedHealth == PlayerHealth)
        {
            float scaledHealth = Mathf.Ceil(healthChangeEvent.NewHealth / healthChangeEvent.AffectedHealth.MaximumHealth * maxHealth);
            UpdateHealthBar(scaledHealth);
            Debug.Log("Updating health bar in g_HealthBar.cs line 99. scaledHealth=");
            Debug.Log(scaledHealth);
        }
    }

    private void OnEnable()
    {
        this.MMEventStartListening<HealthChangeEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<HealthChangeEvent>();
    }


    //Optional: If lives change dynamically, you can update the lives text every frame
    /* void Update() //   **********************************This allows lives to update ******************************************
    {
        UpdateLivesText();
    }*/
}

