
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 100; // Instant kill by default
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the health bar component and apply damage
            HealthBar healthBar = collision.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.TakeDamage(damageAmount);
            }
        }
    }
}