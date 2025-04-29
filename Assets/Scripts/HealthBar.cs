using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public Slider slider;
    private Animator playerAnimator;
    private bool isDead = false;
    
    // Invincibility frame variables
    public float invincibilityDuration = 1.5f; // Duration of invincibility in seconds
    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        
        // Get the animator from the player
        playerAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        // If player is dead or currently invincible, ignore the damage
        if (isDead || isInvincible) return;
        
        health -= amount;
        slider.value = health;
        
        // Make the player invincible and start the invincibility coroutine
        StartCoroutine(InvincibilityFrames());

        if(health <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        
        // Optional: Visual feedback for invincibility
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Flash effect during invincibility
            float flashDuration = 0.1f;
            for (float i = 0; i < invincibilityDuration; i += flashDuration * 2)
            {
                spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 0.5f); // Reddish and transparent
                yield return new WaitForSeconds(flashDuration);
                spriteRenderer.color = Color.white; // Normal color
                yield return new WaitForSeconds(flashDuration);
            }
        }
        else
        {
            // If no sprite renderer is found, just wait the duration
            yield return new WaitForSeconds(invincibilityDuration);
        }
        
        isInvincible = false;
    }
    
    private void Die()
    {
        isDead = true;
        Debug.Log("Player died");
        
        // Disable player controller script
        var controller = GetComponent<PlayerController.Player2DPlatformController>();
        if (controller != null)
            controller.enabled = false;
        
        // Disable other scripts that should stop on death
        var weaponSwitching = GetComponent<WeaponSwitching>();
        if (weaponSwitching != null)
            weaponSwitching.enabled = false;
            
        // Trigger death animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool(PlayerController.AnimationStrings.isDead, true);
            
            // Play death sound if you have an AudioManager
            var audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.humandead);
                
            // You could reload level after animation
            StartCoroutine(GameOverAfterAnimation());
        }
        else
        {
            // If no animator, handle immediate game over
            Destroy(gameObject, 0.5f);
        }
    }
    
    private IEnumerator GameOverAfterAnimation()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(1.5f);
        
        // Load the Game Over scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }
}