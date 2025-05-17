using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int ammoValue = 5; // Amount of ammo to add
    [SerializeField] private AudioClip pickupSound; // Optional fallback sound
    [SerializeField] private GameObject pickupEffect; // Optional visual effect

    private AudioManager audioManager; // Reference to AudioManager

    private void Start()
    {
        // Find and cache AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found. Make sure there's a GameObject with 'Audio' tag and AudioManager component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
            // Find the ShootScript attached to the player's gun
            ShootScript shootScript = collision.GetComponent<ShootScript>();

            // If not found on player, look for it in the player's children
            if (shootScript == null)
            {
                shootScript = collision.GetComponentInChildren<ShootScript>();
            }

            if (shootScript != null)
            {
                // Add ammo to the player's reserves
                shootScript.AddAmmo(ammoValue);

                // Play armor collect sound effect
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.arrmorcollect);
                }
                else if (pickupSound != null) // fallback if no AudioManager
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                // Spawn visual effect if assigned
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                // Show a debug message
                Debug.Log("Picked up " + ammoValue + " ammo! Total ammo: " + shootScript.currentAmmo);

                // Destroy the pickup object
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("ShootScript not found on player or in children! Check your hierarchy setup.");
            }
        }
    }
}
