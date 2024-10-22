using UnityEngine;
using UnityEngine.UI;

public class g_HealthBar : MonoBehaviour
{
    public Image Heart_HealthLast;  // Last heart
    public Image Heart_HealthMid;   // Middle heart
    public Image Heart_HealthFull;  // Full heart
    
    private int health = 3;  // Max health (3 hearts)
    
    // Method to call when the player takes damage
    public void TakeDamage()
    {
        health--;
        UpdateHealthBar();
    }

    // Method to call when the player picks up health
    public void GainHealth()
    {
        health++;
        UpdateHealthBar();
    }

    // Updates the health bar based on the current health
    private void UpdateHealthBar()
    {
        // Clamp health value between 0 and 3
        health = Mathf.Clamp(health, 0, 3);

        // Show/Hide hearts based on health
        Heart_HealthFull.enabled = (health >= 3);  // Show full heart if health is 3
        Heart_HealthMid.enabled = (health >= 2);   // Show middle heart if health is 2 or more
        Heart_HealthLast.enabled = (health >= 1);  // Show last heart if health is 1 or more
    }
}
