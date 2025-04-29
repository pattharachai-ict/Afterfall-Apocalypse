using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int meleeDamage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
{
    IDDamagable damageable = collision.GetComponent<IDDamagable>();
    if (damageable != null)
    {
        damageable.damage(meleeDamage);
    }
}
}