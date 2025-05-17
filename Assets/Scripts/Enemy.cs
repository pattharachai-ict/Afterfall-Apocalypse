using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using PlayerController;

<<<<<<< HEAD
public class Enemy : MonoBehaviour, IDDamagable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 20f;
=======

public class Enemy : MonoBehaviour, IDDamagable
{
    [SerializeField] private float maxHealth = 3f;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    private float currentHealth;
    private float lastFlashThreshold;
    private float flashThreshold;
    private Animator enemyAnimator;
    private bool isDead = false;

    [Header("Visuals")]
    public GameObject BloodEffect;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

<<<<<<< HEAD
    [Header("UI")]
    [SerializeField] private FloatingHealthBar healthBar;
    
    [Header("Drops")]
    [SerializeField] private GameObject ammoPickupPrefab;
    [SerializeField] private Vector2 dropForce = new Vector2(0, 2f);
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private AudioManager audioManager;
    
    // Add a static reference to a backup ammo prefab
    private static GameObject backupAmmoPrefab;

=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    void Start()
    {
        currentHealth = maxHealth;
        lastFlashThreshold = maxHealth;
<<<<<<< HEAD
        flashThreshold = maxHealth / 4;
=======
        flashThreshold = maxHealth / maxHealth;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

<<<<<<< HEAD
        enemyAnimator = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null && showDebugInfo)
        {
            Debug.LogWarning("AudioManager not found. Make sure the Audio tag is set correctly.");
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        // Try to find the ammo prefab if it's not assigned
        if (ammoPickupPrefab == null)
        {
            // Try to use the backup prefab if available
            if (backupAmmoPrefab != null)
            {
                ammoPickupPrefab = backupAmmoPrefab;
                if (showDebugInfo)
                {
                    Debug.Log("Using backup ammo prefab reference");
                }
            }
            else
            {
                // Try to find AmmoPickup in the scene
                AmmoPickup[] pickups = FindObjectsOfType<AmmoPickup>(true);
                if (pickups.Length > 0 && pickups[0] != null)
                {
                    ammoPickupPrefab = pickups[0].gameObject;
                    backupAmmoPrefab = ammoPickupPrefab; // Store for other instances
                    if (showDebugInfo)
                    {
                        Debug.Log("Found ammo prefab in scene");
                    }
                }
                else
                {
                    // Try to find by name in Resources folder if it exists
                    GameObject resourcePrefab = Resources.Load<GameObject>("AmmoPickup");
                    if (resourcePrefab != null)
                    {
                        ammoPickupPrefab = resourcePrefab;
                        backupAmmoPrefab = ammoPickupPrefab; // Store for other instances
                        if (showDebugInfo)
                        {
                            Debug.Log("Found ammo prefab in Resources folder");
                        }
                    }
                    else if (showDebugInfo)
                    {
                        Debug.LogWarning("AmmoPickup prefab not assigned and couldn't be found automatically. " +
                                         "Please assign it in the Inspector or add it to a Resources folder.");
                    }
                }
            }
        }
        else if (backupAmmoPrefab == null)
        {
            // Store the first valid prefab reference for other instances to use
            backupAmmoPrefab = ammoPickupPrefab;
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"Enemy initialized with {maxHealth} health");
        }
    }

    // Implement IDDamagable interface methods
    public void damage(float damageAmount)
    {
        if (isDead) return;

        // Debug log for bullet hits
        if (showDebugInfo)
        {
            Debug.Log($"Enemy damage method called with damage: {damageAmount}");
        }

        float previousHealth = currentHealth;
        currentHealth -= damageAmount;
        
        if (showDebugInfo)
        {
            Debug.Log($"Enemy took {damageAmount} damage. Health left: {currentHealth}/{maxHealth}");
        }

        // Instantiate blood effect if available
        if (BloodEffect != null)
        {
            Instantiate(BloodEffect, transform.position, quaternion.identity);
        }

        // Update health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Visual feedback for damage
=======
        // 🧩 NEW: Get Animator
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // 🔥 REMOVE auto destroy here (we move this logic to Die())
    }

    public void damage(float damageAmount)
    {
        if (isDead) return; // 🧩 Avoid multiple death triggers

        float previousHealth = currentHealth;
        currentHealth -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Health left: {currentHealth}");

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        if (spriteRenderer != null && Mathf.Floor(previousHealth / flashThreshold) > Mathf.Floor(currentHealth / flashThreshold))
        {
            StartCoroutine(FlashRed());
            lastFlashThreshold -= flashThreshold;
        }

<<<<<<< HEAD
        // Check for death
        if (currentHealth <= 0 && !isDead)
=======
        if (currentHealth <= 0)
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        {
            Die();
        }
    }

    private void Die()
    {
<<<<<<< HEAD
        if (isDead) return; // Prevent calling Die() multiple times
        
        isDead = true;
        
        if (showDebugInfo)
        {
            Debug.Log("Enemy died, attempting to drop ammo");
        }

        // Play death SFX
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.zombiedead);
        }

        // Stop movement
=======
        isDead = true;
        Debug.Log("Enemy died");

        // 🧩 Stop zombie movement
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

<<<<<<< HEAD
        // Disable collider
=======
        // 🧩 Disable Collider so player shots don't hit corpse
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

<<<<<<< HEAD
        // Drop ammo pickup
        DropAmmoPickup();

        // Play death animation if animator exists
=======
        // 🧩 Play death animation
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool(AnimationStrings.isDead, true);
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
<<<<<<< HEAD

        // Hide health bar
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void DropAmmoPickup()
    {
        // Make one final attempt to find the prefab if it's null
        if (ammoPickupPrefab == null && backupAmmoPrefab != null)
        {
            ammoPickupPrefab = backupAmmoPrefab;
        }
        
        // Check if we have a prefab reference
        if (ammoPickupPrefab != null)
        {
            if (showDebugInfo)
            {
                Debug.Log("Attempting to instantiate ammo pickup");
            }
            
            try
            {
                // Instantiate the ammo pickup at the enemy's position
                GameObject ammoPickup = Instantiate(
                    ammoPickupPrefab, 
                    transform.position, 
                    Quaternion.identity
                );
                
                if (showDebugInfo)
                {
                    Debug.Log($"Ammo pickup instantiated: {ammoPickup != null}");
                }
                
                // Apply a small upward force to make it pop out
                Rigidbody2D pickupRb = ammoPickup.GetComponent<Rigidbody2D>();
                if (pickupRb != null)
                {
                    // Apply random horizontal direction with upward force
                    float randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
                    pickupRb.AddForce(new Vector2(randomX + dropForce.x, dropForce.y), ForceMode2D.Impulse);
                    
                    if (showDebugInfo)
                    {
                        Debug.Log($"Applied force to ammo pickup: ({randomX + dropForce.x}, {dropForce.y})");
                    }
                }
                
                if (showDebugInfo)
                {
                    Debug.Log("Dropped ammo pickup successfully");
                }
            }
            catch (System.Exception e)
            {
                if (showDebugInfo)
                {
                    Debug.LogError($"Error instantiating ammo pickup: {e.Message}");
                }
            }
        }
        else
        {
            if (showDebugInfo)
            {
                Debug.LogError("Cannot drop ammo: ammoPickupPrefab is null! Assign it in the Inspector.");
            }
        }
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    }

    private IEnumerator DestroyAfterAnimation()
    {
<<<<<<< HEAD
=======
        // 🧩 Wait time (match your death animation length)
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
<<<<<<< HEAD
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    
    // Add extra debug checking for collisions with bullets
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (showDebugInfo)
        {
            Debug.Log($"Enemy collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebugInfo)
        {
            Debug.Log($"Enemy trigger entered by: {other.gameObject.name}, Tag: {other.gameObject.tag}");
        }
    }
}
=======
}
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
