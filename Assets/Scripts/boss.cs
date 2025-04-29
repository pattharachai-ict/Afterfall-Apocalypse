using System.Collections;
using UnityEngine;
using PlayerController;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class boss : MonoBehaviour, IDDamagable
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float walkStopRate = 0.05f;
    private bool canFlip = true;

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

    // UNITY FUNCTIONS
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        currentHealth = maxHealth;
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

    private void FixedUpdate()
    {
        if (isDead) return;

        if (canFlip && !justDamaged && touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if (CanMove)
            rb.linearVelocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
    }

    // LOGIC FUNCTIONS
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

    // DAMAGE FUNCTIONS
    public void damage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        Debug.Log($"Boss took {damageAmount} damage. Health left: {currentHealth}");

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        // Prevent flipping shortly after getting hit
        justDamaged = true;
        damagedTimer = damagedCooldownTime;
    }

    private void Die()
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
