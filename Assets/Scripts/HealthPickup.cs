using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25; // Amount of health to restore
    [SerializeField] private AudioClip pickupSound; // Optional sound effect
    [SerializeField] private GameObject pickupEffect; // Optional visual effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
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
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }
                
                // Destroy the health pickup
                Destroy(gameObject);
            }
        }
    }
}