using UnityEngine;
using System.Collections;

public class MeleeAttack2D : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float attackRange = 1f;
<<<<<<< HEAD
    [SerializeField] private float attackDamage = 8f;
=======
    [SerializeField] private float attackDamage = 1f;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform batToRotate;  // <- assign `bat_head` here

    private float cooldownTimer;

<<<<<<< HEAD
    // Declare the audioManager variable
    private AudioManager audioManager;

    void Awake()
    {
        // Initialize the audioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Prevent attacking if the script is disabled or the GameObject is inactive
        if (!enabled || !gameObject.activeInHierarchy)
            return;

        cooldownTimer -= Time.deltaTime;

        // IMPORTANT: Only check for direct mouse clicks when the weapon switching
        // is not handling our inputs (for debugging/testing only)
        #if UNITY_EDITOR
=======
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownTimer <= 0f)
        {
            PerformAttack();
            cooldownTimer = attackCooldown;
        }
<<<<<<< HEAD
        #endif
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    }

    void PerformAttack()
    {
        Debug.Log("Player performed a melee attack.");
<<<<<<< HEAD

        // Play melee attack sound
        audioManager.PlaySFX(audioManager.melee);  // Play the melee sound

=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        StartCoroutine(SwingBat());

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D obj in hitObjects)
        {
            IDDamagable target = obj.GetComponent<IDDamagable>();
            if (target != null)
            {
                target.damage(attackDamage);
                Debug.Log("Hit: " + obj.name);
            }
        }
    }

    private IEnumerator SwingBat()
    {
        float swingDuration = 0.2f;
        float elapsed = 0f;
        float startAngle = 90f;
        float endAngle = -90f;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            batToRotate.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        batToRotate.localRotation = Quaternion.identity;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
<<<<<<< HEAD
    
    // This method is called by the weapon switching system
    public void TryMeleeAttack()
    {
        if (cooldownTimer <= 0f)
        {
            PerformAttack();
            cooldownTimer = attackCooldown;
        }
    }

    // Kept for compatibility with any other scripts that might be using it
    public void ManualAttack()
    {
        TryMeleeAttack();
    }
}
=======
}



>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
