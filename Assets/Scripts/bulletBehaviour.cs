using System.Text;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    [Header("Normal Bullet Stats")]
    [SerializeField] private float bulletSpeed = 17.5f;
    [SerializeField] private float NBdamage = 1f;
    [Header("General Bullet Stats")]
    private float countDown = 2f;
    //[SerializeField] private LayerMask whatDestroyBullet;
    [Header("Physic Bullet Stats")]
    [SerializeField] private float physicBulletSpeed = 17.5f;
    [SerializeField] private float bulletGravityValue = 0.5f;
    [SerializeField] private float PBdamage = 3f; // Increased and made serializable to edit in inspector
    
    public BulletType bulletType;
    private Rigidbody2D rb;
    private float damage;

    public enum BulletType
    {
        Normal, Physics
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Auto-destroy bullet after countdown
        //******Destroy(gameObject, countDown);
        
        // Change RB stats based on bullet type
        setRBStat();
        
        // Set velocity based on bullet type
        InitializeBulletStats();
        
        Debug.Log("Bullet created with damage value: " + damage);
    }

    private void InitializeBulletStats()
    {
        if(bulletType == BulletType.Normal)
        {
            setStraightVelo();
            damage = NBdamage;
        }
        else if(bulletType == BulletType.Physics)
        {
            setPhysicVelo();
            damage = PBdamage;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Bullet hit: " + other.gameObject.name + " with tag: " + other.gameObject.tag);
    
    // Check if collision is with a valid tagged object
    if(other.gameObject.CompareTag("Enemy"))
    {
        // Try to get any component that implements IDDamagable
        IDDamagable damageable = other.gameObject.GetComponent<IDDamagable>();
        if (damageable != null)
        {
            Debug.Log("Applying damage: " + damage + " to damageable object");
            damageable.damage(damage);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No IDDamagable component found on object tagged as Enemy");
        }
    }
    else if(other.gameObject.CompareTag("Ground"))
    {
        Destroy(gameObject);
        Debug.Log("Hit the wall");
    }
}

// Similar change for OnCollisionEnter2D
private void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log("Bullet collision with: " + collision.gameObject.name);
    
    if(collision.gameObject.CompareTag("Enemy"))
    {
        IDDamagable damageable = collision.gameObject.GetComponent<IDDamagable>();
        if (damageable != null)
        {
            Debug.Log("Applying damage through collision: " + damage);
            damageable.damage(damage);
        }
        Destroy(gameObject);
    }
    else if(collision.gameObject.CompareTag("Ground"))
    {
        Destroy(gameObject);
    }
}

    private void setStraightVelo()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    private void setPhysicVelo()
    {
        rb.linearVelocity = transform.right * physicBulletSpeed;
    }

    private void setRBStat()
    {
        if(bulletType == BulletType.Normal)
        {
            rb.gravityScale = 0f;
        }
        else if(bulletType == BulletType.Physics)
        {
            rb.gravityScale = bulletGravityValue;
        }
    }

    private void FixedUpdate()
    {
        // Rotate bullet in direction of velocity for both bullet types
        if (rb.linearVelocity.sqrMagnitude > 0)
        {
            transform.right = rb.linearVelocity;
        }
    }
}