// 17/12/24
// We're adding the player glide ability, which is checked using the players health.
// Add this function to the Corgi made Health.cs code.  
// This is only the function, not the full script. 

private void UpdateGlideAbility()
{
    GameObject player = GameObject.FindWithTag("Player");
    if (player == null) return;

    // Get references to CharacterGlide and bubble animation
    CharacterGlide glideAbility = player.GetComponent<CharacterGlide>();
    Animator bubbleAnimator = player.GetComponentInChildren<Animator>(); // Assuming the bubble uses an Animator

    if (CurrentHealth >= MaximumHealth) // Health is 3
    {
        // Enable glide ability and bubble animation
        if (glideAbility != null) glideAbility.AbilityAuthorized = true;
        if (bubbleAnimator != null) bubbleAnimator.SetBool("IsBubbleVisible", true); // Assuming you have a bool parameter in the Animator
    }
    else
    {
        // Disable glide ability and bubble animation
        if (glideAbility != null) glideAbility.AbilityAuthorized = false;
        if (bubbleAnimator != null) bubbleAnimator.SetBool("IsBubbleVisible", false);
    }
}
