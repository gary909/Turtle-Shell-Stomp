using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class HealthBar : MonoBehaviour, MMEventListener<HealthChangeEvent>
{
    public Image Heart_HealthLast;  // Last heart (1 heart left)
    public Image Heart_HealthMid;   // Middle heart (2 hearts left)
    public Image Heart_HealthFull;  // Full heart (3 hearts left)

    private int maxHealth = 3;

    // Method to update health bar based on the player's health
    public void UpdateHealthBar(float currentHealth)
    {
        // Update visibility of hearts based on current health
        Heart_HealthFull.enabled = (currentHealth >= maxHealth);  // Full heart if health is max
        Heart_HealthMid.enabled = (currentHealth >= 2);           // Mid heart if health is 2 or more
        Heart_HealthLast.enabled = (currentHealth >= 1);          // Last heart if health is 1 or more
    }

    // This method gets called whenever the HealthChangeEvent is triggered
    public void OnMMEvent(HealthChangeEvent healthChangeEvent)
    {
        if (healthChangeEvent.AffectedHealth != null)
        {
            // Convert the player's health to a scale from 0 to 3
            float scaledHealth = Mathf.Ceil(healthChangeEvent.NewHealth / healthChangeEvent.AffectedHealth.MaximumHealth * maxHealth);
            UpdateHealthBar(scaledHealth);
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
}
