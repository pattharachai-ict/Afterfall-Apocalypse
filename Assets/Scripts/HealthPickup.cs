using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25; // Amount of health to restore
<<<<<<< HEAD
    [SerializeField] private AudioClip pickupSound; // Optional fallback sound
    [SerializeField] private GameObject pickupEffect; // Optional visual effect

    private AudioManager audioManager; // ✅ Reference to AudioManager

    private void Start()
    {
        // ✅ Find and cache AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

=======
    [SerializeField] private AudioClip pickupSound; // Optional sound effect
    [SerializeField] private GameObject pickupEffect; // Optional visual effect

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
<<<<<<< HEAD
            HealthBar healthBar = collision.GetComponent<HealthBar>();

            if (healthBar != null && healthBar.health < healthBar.maxHealth)
            {
                int healingToApply = Mathf.Min(healAmount, healthBar.maxHealth - healthBar.health);
                healthBar.health += healingToApply;
                healthBar.slider.value = healthBar.health;

                // ✅ Use AudioManager to play SFX
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.healthpackcollect);
                }
                else if (pickupSound != null) // fallback
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

=======
            // Get the health bar component from the player
            HealthBar healthBar = collision.GetComponent<HealthBar>();
            
            // If player has a health component and is not at max health
            if (healthBar != null && healthBar.health < healthBar.maxHealth)
            {
                // Heal the player (the TakeDamage method with negative value will heal)
                int healingToApply = Mathf.Min(healAmount, healthBar.maxHealth - healthBar.health);
                healthBar.health += healingToApply;
                
                // Update the health bar slider
                healthBar.slider.value = healthBar.health;
                
                // Play pickup sound if assigned
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }
                
                // Spawn effect if assigned
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }
<<<<<<< HEAD

=======
                
                // Destroy the health pickup
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
                Destroy(gameObject);
            }
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
