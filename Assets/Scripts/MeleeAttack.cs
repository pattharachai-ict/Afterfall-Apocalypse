using UnityEngine;
using System.Collections;

public class MeleeAttack2D : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 1f;

    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform batToRotate;  // <- assign `bat_head` here

    private float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownTimer <= 0f)
        {
            PerformAttack();
            cooldownTimer = attackCooldown;
        }
    }

    void PerformAttack()
    {
        Debug.Log("Player performed a melee attack.");
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
}



