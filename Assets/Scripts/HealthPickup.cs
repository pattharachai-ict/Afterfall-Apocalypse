using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25; // Amount of health to restore
    [SerializeField] private AudioClip pickupSound; // Optional fallback sound
    [SerializeField] private GameObject pickupEffect; // Optional visual effect

    private AudioManager audioManager; // ✅ Reference to AudioManager

    private void Start()
    {
        // ✅ Find and cache AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
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

                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}
