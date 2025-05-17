using System.Collections;
using UnityEngine;
using PlayerController;
using UnityEngine.SceneManagement;
<<<<<<< HEAD
using UnityEngine.UI;
=======

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class boss : MonoBehaviour, IDDamagable
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float walkStopRate = 0.05f;
    private bool canFlip = true;
<<<<<<< HEAD
    [Header("Walking Range")]
[SerializeField] private float leftBoundaryX = -0.51f;
[SerializeField] private float rightBoundaryX = 29.5f;
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    [Header("Detection")]
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    [Header("Health")]
    [SerializeField] private float maxHealth = 25f;
    private float currentHealth;
    private bool isDead = false;

    [Header("Visuals")]
    public GameObject BloodEffect;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

<<<<<<< HEAD
    [Header("UI")]
    [SerializeField] private FloatingHealthBar healthBar;

=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    // Components
    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;

    // Walking Direction
    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 WalkDirectionVector = Vector2.right;

    // Damage Flinch
    private bool justDamaged = false;
    private float damagedCooldownTime = 0.2f;
    private float damagedTimer = 0f;

    // Properties
    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                float newXScale = Mathf.Abs(transform.localScale.x);
                if (value == WalkableDirection.Left)
                    newXScale = -newXScale;

                transform.localScale = new Vector2(newXScale, transform.localScale.y);

                WalkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }

            _walkDirection = value;
        }
    }

<<<<<<< HEAD
    private float initialYPosition;

=======
    // UNITY FUNCTIONS
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
<<<<<<< HEAD

        if (healthBar == null)
            healthBar = GetComponentInChildren<FloatingHealthBar>();
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    }

    private void Start()
    {
        currentHealth = maxHealth;
<<<<<<< HEAD
        rb.gravityScale = 0;
        initialYPosition = transform.position.y;

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Update()
{
    if (isDead) return;

    HasTarget = attackZone.DetectedColliders.Count > 0;

    if (justDamaged)
    {
        damagedTimer -= Time.deltaTime;
        if (damagedTimer <= 0f)
            justDamaged = false;
    }

    // Flip if boss moves beyond its walking bounds
    if (transform.position.x < leftBoundaryX && WalkDirection == WalkableDirection.Left)
    {
        FlipDirection();
    }
    else if (transform.position.x > rightBoundaryX && WalkDirection == WalkableDirection.Right)
    {
        FlipDirection();
    }
}

=======
    }

    private void Update()
    {
        if (isDead) return;

        HasTarget = attackZone.DetectedColliders.Count > 0;

        // Check for cliff ahead
        if (cliffDetectionZone != null && cliffDetectionZone.DetectedColliders.Count == 0 && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }

        // Update damage flinch timer
        if (justDamaged)
        {
            damagedTimer -= Time.deltaTime;
            if (damagedTimer <= 0f)
                justDamaged = false;
        }
    }
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    private void FixedUpdate()
    {
        if (isDead) return;

        if (canFlip && !justDamaged && touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if (CanMove)
<<<<<<< HEAD
        {
            rb.linearVelocity = new Vector2(walkSpeed * WalkDirectionVector.x, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), 0);
        }
    }

=======
            rb.linearVelocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
    }

    // LOGIC FUNCTIONS
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Invalid WalkDirection state!");
        }
    }

<<<<<<< HEAD
=======
    // DAMAGE FUNCTIONS
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    public void damage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
<<<<<<< HEAD
=======

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        Debug.Log($"Boss took {damageAmount} damage. Health left: {currentHealth}");

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

<<<<<<< HEAD
        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        if (currentHealth <= 0)
        {
            Die();
        }

<<<<<<< HEAD
=======
        // Prevent flipping shortly after getting hit
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        justDamaged = true;
        damagedTimer = damagedCooldownTime;
    }

    private void Die()
<<<<<<< HEAD
    {
        isDead = true;
        Debug.Log("Boss is dead");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        if (attackZone != null && attackZone.gameObject != null)
            attackZone.gameObject.SetActive(false);

        if (animator != null)
        {
            animator.SetBool(AnimationStrings.isDead, true);
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }

        if (BloodEffect != null)
        {
            Instantiate(BloodEffect, transform.position, Quaternion.identity);
        }

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameClearScene");
        Destroy(gameObject);
    }
=======
{
    isDead = true;
    Debug.Log("Boss is dead");

    // Stop movement
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    // Disable collider
    Collider2D collider = GetComponent<Collider2D>();
    if (collider != null)
        collider.enabled = false;

    // Disable attack zone
    if (attackZone != null && attackZone.gameObject != null)
        attackZone.gameObject.SetActive(false);

    // Play death animation
    if (animator != null)
    {
        animator.SetBool(AnimationStrings.isDead, true);
        StartCoroutine(DestroyAfterAnimation());
    }
    else
    {
        Destroy(gameObject, 0.5f);
    }

    // Spawn blood effect
    if (BloodEffect != null)
    {
        Instantiate(BloodEffect, transform.position, Quaternion.identity);
    }
}

private IEnumerator DestroyAfterAnimation()
{
    // Wait for the death animation to complete (adjust the time to match your animation length)
    yield return new WaitForSeconds(1.5f); // Adjust this based on your animation length

    // Transition to the GameClearScene
    SceneManager.LoadScene("GameClearScene");

    // Optionally, destroy the boss object (not strictly needed as the scene transition happens)
    Destroy(gameObject);
}

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    public void ApplyKnockback(Vector2 force)
    {
        // Boss ignores knockback
    }
}
