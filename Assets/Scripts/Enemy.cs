using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using PlayerController;


public class Enemy : MonoBehaviour, IDDamagable
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    private float lastFlashThreshold;
    private float flashThreshold;
    private Animator enemyAnimator;
    private bool isDead = false;

    [Header("Visuals")]
    public GameObject BloodEffect;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        lastFlashThreshold = maxHealth;
        flashThreshold = maxHealth / maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

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

        if (spriteRenderer != null && Mathf.Floor(previousHealth / flashThreshold) > Mathf.Floor(currentHealth / flashThreshold))
        {
            StartCoroutine(FlashRed());
            lastFlashThreshold -= flashThreshold;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Enemy died");

        // 🧩 Stop zombie movement
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // 🧩 Disable Collider so player shots don't hit corpse
        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        // 🧩 Play death animation
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool(AnimationStrings.isDead, true);
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // 🧩 Wait time (match your death animation length)
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
