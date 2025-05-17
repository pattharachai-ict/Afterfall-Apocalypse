using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using PlayerController;

public class ShootScript : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    private AudioManager audioManager;

    [Header("Ammo Settings")]
    public int currentClip;
    public int maxClipSize = 10;
    public int currentAmmo;
    public int maxAmmoSize = 30;

    [Header("Shooting Settings")]
    [SerializeField] private float shootCooldown = 0.1f;
    [SerializeField] private float reloadDuration = 2f;
    private float nextShootTime = 0f;
    private bool isReloading = false;
    private Coroutine reloadCoroutine;

    [Header("Bullet Settings")]
    [SerializeField] private bulletBehaviour.BulletType bulletTypeToUse = bulletBehaviour.BulletType.Normal;

    [Header("Damage Settings")]
    [SerializeField] private float bulletDamage = 10f;

    [Header("Input Handling")]
    [SerializeField] private bool managedByWeaponSwitcher = true;

    [SerializeField] private Player2DPlatformController playerController;
    private Vector2 direction;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentClip = 10;
        currentAmmo = 0;
        ValidateReferences();
        UpdateBulletDamage();
    }

    void OnEnable()
    {
        // Safety reset
        isReloading = false;
        reloadCoroutine = null;
    }

    void Update()
    {
        CalculateShootDirection();

        if (isReloading)
            return;

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartReload();
            return;
        }

#if UNITY_EDITOR
        if (!managedByWeaponSwitcher)
        {
            HandleGunShooting();
        }
#endif
    }

    private void CalculateShootDirection()
    {
        if (playerController != null)
            direction = playerController.IsFacingRight ? Vector2.right : Vector2.left;
        else
            direction = Vector2.right;
    }

    private void HandleGunShooting()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed && Time.time >= nextShootTime)
        {
            if (currentClip <= 0)
            {
                Debug.Log("Clip empty! Press 'R' to reload.");
                return;
            }

            Shoot();
            currentClip--;
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void Shoot()
    {
        if (bullet == null || bulletSpawnPoint == null) return;

        Quaternion bulletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletRotation);

        var bulletScript = newBullet.GetComponent<bulletBehaviour>();
        if (bulletScript != null)
        {
            bulletScript.bulletType = bulletTypeToUse;
        }

        audioManager.PlaySFX(audioManager.shoot);
        Debug.Log("Bullet fired in direction: " + direction);
    }

    private IEnumerator Reload()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo left to reload!");
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading...");
        audioManager.PlaySFX(audioManager.reload);

        yield return new WaitForSeconds(reloadDuration);

        int reloadAmount = Mathf.Min(maxClipSize - currentClip, currentAmmo);
        currentClip += reloadAmount;
        currentAmmo -= reloadAmount;

        isReloading = false;
        reloadCoroutine = null;
        Debug.Log("Reload complete.");
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentClip < maxClipSize && currentAmmo > 0)
        {
            StartReload();
        }
    }

    private void StartReload()
    {
        if (!isReloading)
        {
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    public void TryShoot()
    {
        if (Time.time >= nextShootTime && !isReloading && currentClip > 0)
        {
            Shoot();
            currentClip--;
            nextShootTime = Time.time + shootCooldown;
        }
    }

    public void CancelReloadIfActive()
    {
        if (isReloading && reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            isReloading = false;
            Debug.Log("Reload canceled due to weapon switch.");
        }
    }
public void AddAmmo(int ammoAmount)
{
    currentAmmo += ammoAmount;
    if (currentAmmo > maxAmmoSize)
    {
        currentAmmo = maxAmmoSize;
    }
}
    private void ValidateReferences()
    {
        if (bullet == null) Debug.LogError("Bullet prefab is not assigned!");
        if (bulletSpawnPoint == null) Debug.LogError("Bullet spawn point is not assigned!");
        if (gun == null) Debug.LogError("Gun GameObject is not assigned!");
    }

    private void UpdateBulletDamage()
    {
        var bulletScript = bullet?.GetComponent<bulletBehaviour>();
        if (bulletScript != null)
        {
            Debug.Log("Bullet prefab damage settings initialized");
        }
    }
}