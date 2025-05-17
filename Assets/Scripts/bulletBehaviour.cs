using System.Text;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    [Header("Normal Bullet Stats")]
    [SerializeField] private float bulletSpeed = 17.5f;
<<<<<<< HEAD
    [SerializeField] private float NBdamage = 10f;  // Changed to 10 for normal bullets
=======
    [SerializeField] private float NBdamage = 1f;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    [Header("General Bullet Stats")]
    private float countDown = 2f;
    //[SerializeField] private LayerMask whatDestroyBullet;
    [Header("Physic Bullet Stats")]
    [SerializeField] private float physicBulletSpeed = 17.5f;
    [SerializeField] private float bulletGravityValue = 0.5f;
<<<<<<< HEAD
    [SerializeField] private float PBdamage = 15f; // Increased physics bullet damage to maintain higher relative damage
    
    [Header("Camera Settings")]
    [SerializeField] private bool destroyWhenOffscreen = true;
    [SerializeField] private float offscreenMargin = 2f; // Extra margin beyond camera view
=======
    [SerializeField] private float PBdamage = 3f; // Increased and made serializable to edit in inspector
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    
    public BulletType bulletType;
    private Rigidbody2D rb;
    private float damage;
<<<<<<< HEAD
    private Camera mainCamera;
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    public enum BulletType
    {
        Normal, Physics
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        mainCamera = Camera.main; // Get reference to the main camera
        
        // Auto-destroy bullet after countdown
        Destroy(gameObject, countDown);  // Uncommented this for bullet cleanup
=======
        
        // Auto-destroy bullet after countdown
        //******Destroy(gameObject, countDown);
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        
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
<<<<<<< HEAD
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
=======
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
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

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
<<<<<<< HEAD
        
        // Check if bullet is off-screen and destroy if needed
        if (destroyWhenOffscreen && IsOffScreen())
        {
            Destroy(gameObject);
            Debug.Log("Bullet destroyed - off camera view");
        }
    }
    
    // Check if the bullet is outside the camera's view
    private bool IsOffScreen()
    {
        if (mainCamera == null) return false;
        
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        
        // Add margin to viewport bounds (0-1 is on screen)
        float margin = offscreenMargin * 0.1f; // Convert to viewport space
        
        return viewportPosition.x < -margin || 
               viewportPosition.x > 1 + margin || 
               viewportPosition.y < -margin || 
               viewportPosition.y > 1 + margin;
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    }
}