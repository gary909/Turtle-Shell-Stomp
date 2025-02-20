using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;

// V6 adds character glide ability check *(in conjuction with Health.cs - see Health_1.cs in repo for changes explanation)
// v5 Scripts FPS rate improved with this version (compared to v4)
// Not fully checked for any bugs just yet

public class g_HealthBar : MonoBehaviour, MMEventListener<HealthChangeEvent>
{
    public Image Heart_HealthLast;  // Last heart (1 heart left)
    public Image Heart_HealthMid;   // Middle heart (2 hearts left)
    public Image Heart_HealthFull;  // Full heart (3 hearts left)

    public Text LivesText;          // Reference to the Text object that displays the lives

    private Health PlayerHealth;    // Reference to the player's health component
    private int maxHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    // Coroutine to initialize the health bar and lives display after a slight delay
    private IEnumerator InitializeAfterDelay()
    {
        float timeout = 5f; // Max time to search for PlayerHealth
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
            UpdateLivesText();
            Debug.Log("Initialization complete, CurrentLives: " + GameManager.Instance.CurrentLives);
        }
    }


    // Method to initialize health bar based on the player's current health
    private void InitializeHealthBar()
    {
        float initialHealth = PlayerHealth.CurrentHealth;
        float scaledHealth = Mathf.Ceil(initialHealth / PlayerHealth.MaximumHealth * maxHealth);
        UpdateHealthBar(scaledHealth);
    }

    // Method to update health bar based on the player's health
    public void UpdateHealthBar(float currentHealth)
    {
        // Update visibility of hearts based on current health
        Heart_HealthFull.enabled = (currentHealth >= maxHealth);  // Full heart if health is max
        Heart_HealthMid.enabled = (currentHealth >= 2);           // Mid heart if health is 2 or more
        Heart_HealthLast.enabled = (currentHealth >= 1);          // Last heart if health is 1 or more

        // ******************************************************************Player Glide Ability Check**************************************
        // Call UpdateGlideAbility when health changes
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.UpdateGlideAbility(); // Synchronize the glide ability
            }
        }
        // ***********************************************************************************************************************************
    }

    // Method to update the lives text display
    private void UpdateLivesText()
    {
        if (GameManager.Instance != null)
        {
            LivesText.text = "Lives: " + GameManager.Instance.CurrentLives;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    // This method gets called whenever the HealthChangeEvent is triggered
    public void OnMMEvent(HealthChangeEvent healthChangeEvent)
    {
        // Only update the health bar if the event is related to the player's health
        if (healthChangeEvent.AffectedHealth == PlayerHealth)
        {
            float scaledHealth = Mathf.Ceil(healthChangeEvent.NewHealth / healthChangeEvent.AffectedHealth.MaximumHealth * maxHealth);
            UpdateHealthBar(scaledHealth);
            Debug.Log("Updating health bar in g_HealthBar.cs line 99. scaledHealth=");
            Debug.Log(scaledHealth);
        }
    }

    // Start listening to health events when the object is enabled
    private void OnEnable()
    {
        this.MMEventStartListening<HealthChangeEvent>();
    }

    // Stop listening to health events when the object is disabled
    private void OnDisable()
    {
        this.MMEventStopListening<HealthChangeEvent>();
    }

    // Example: Trigger this from the GameManager when lives change
    public void OnLivesChanged(int newLives)
    {
        LivesText.text = "Lives: " + newLives;
    }


    // Optional: If lives change dynamically, you can update the lives text every frame
    // void Update()
    // {
    //     UpdateLivesText();
    // }
}
