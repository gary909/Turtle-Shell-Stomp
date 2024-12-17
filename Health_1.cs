// 17/12/24
// We're adding the player glide ability, which is checked using the players health.
// Add this function to the Corgi made Health.cs code.  
// This is only the function, not the full script. 

//Added to line 1064 in Health.cs

		// ***************************************************************TESTING THIS CODE **********************************************************************
		public void UpdateGlideAbility()
		{
			GameObject player = GameObject.FindWithTag("Player");
			if (player == null) return;

			// Get references to CharacterGlide and bubble animation
			CharacterGlide glideAbility = player.GetComponent<CharacterGlide>();
			Animator bubbleAnimator = player.GetComponentInChildren<Animator>(); // Assuming the bubble uses an Animator

			if (CurrentHealth >= MaximumHealth) // Health is 3
			{
				// Enable glide ability and bubble animation
				if (glideAbility != null) glideAbility.enabled = true; // Enable the CharacterGlide script
				if (bubbleAnimator != null) bubbleAnimator.SetBool("IsBubbleVisible", true); // Show the bubble animation
			}
			else
			{
				// Disable glide ability and bubble animation
				if (glideAbility != null) glideAbility.enabled = false; // Disable the CharacterGlide script
				if (bubbleAnimator != null) bubbleAnimator.SetBool("IsBubbleVisible", false); // Hide the bubble animation
			}
		}

		// ****************************************************************************************************************************************************

// Also, we call this fuction on line 1015 in the getHealth fuction:

		public virtual void GetHealth(float health, GameObject instigator)
		{
			// this function adds health to the character's Health and prevents it to go above MaxHealth.
			if (MasterHealth != null)
			{
				MasterHealth.SetHealth(Mathf.Min (CurrentHealth + health,MaximumHealth), instigator);	
			}
			else
			{
				SetHealth(Mathf.Min (CurrentHealth + health,MaximumHealth), instigator);	
			}
			UpdateHealthBar(true);
			UpdateGlideAbility(); // *********HERE'S WHERE THE FUCTION IS CALLED ON LINE 1015 in Health.cs********
			Debug.Log("UpdateHealthBar(true); health.cs line 1015"); // *********GARY CHANGE********
		}

// We also altered the g_HealthBar.cs script:

            public void UpdateHealthBar(float currentHealth)
    {
        // Update visibility of hearts based on current health
        Heart_HealthFull.enabled = (currentHealth >= maxHealth);  // Full heart if health is max
        Heart_HealthMid.enabled = (currentHealth >= 2);           // Mid heart if health is 2 or more
        Heart_HealthLast.enabled = (currentHealth >= 1);          // Last heart if health is 1 or more

        // ******************************************************************GARY TESTING**************************************
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
        // ******************************************************************GARY TESTING**************************************
    }