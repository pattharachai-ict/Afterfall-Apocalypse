using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootScript : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject gun; // The gun GameObject
    [SerializeField] private GameObject bullet; // The bullet prefab
    [SerializeField] private Transform bulletSpawnPoint; // Bullet spawn location

    [Header("Ammo Settings")]
    public int currentClip;
    public int maxClipSize = 10;
    public int currentAmmo;
    public int maxAmmoSize = 30;

    [Header("Shooting Settings")]
    [SerializeField] private float shootCooldown = 1f; // Time between shots
    [SerializeField] private float reloadDuration = 2f; // Time to reload
    private float nextShootTime = 0f;
    private bool isReloading = false;

    private Vector2 worldPosition;
    private Vector2 direction;

    void Start()
    {
        // Initialize ammo
        currentClip = maxClipSize;
        currentAmmo = maxAmmoSize;

        // Check references at start
        ValidateReferences();
    }

    void Update()
    {
        HandleGunRotation();

        if (isReloading)
            return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartCoroutine(Reload());
            return;
        }

        HandleGunShooting();
    }

    private void HandleGunRotation()
    {
        if (gun != null)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (worldPosition - (Vector2)gun.transform.position).normalized;
            gun.transform.right = direction;

            Vector3 localScale = gun.transform.localScale;
            localScale.y = direction.y < 0 ? -Mathf.Abs(localScale.y) : Mathf.Abs(localScale.y);
            gun.transform.localScale = localScale;
        }
    }

    private void HandleGunShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextShootTime)
        {
            if (currentClip <= 0)
            {
                Debug.Log("Clip empty! Press 'R' to reload.");
                return;
            }

            Shoot();
            currentClip--;
        }
    }

    private void Shoot()
    {
        if (bullet == null)
        {
            Debug.LogWarning("Cannot shoot: bullet prefab is null or was destroyed.");
            return;
        }

        if (bulletSpawnPoint == null)
        {
            Debug.LogWarning("Cannot shoot: bulletSpawnPoint is null.");
            return;
        }

        Instantiate(bullet, bulletSpawnPoint.position, gun != null ? gun.transform.rotation : Quaternion.identity);
        Debug.Log("Bullet fired!");
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

        yield return new WaitForSeconds(reloadDuration);

        int reloadAmount = maxClipSize - currentClip;
        reloadAmount = Mathf.Min(reloadAmount, currentAmmo);

        currentClip += reloadAmount;
        currentAmmo -= reloadAmount;

        isReloading = false;
        Debug.Log("Reload complete.");
    }

    public void AddAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize;
        }
    }

    // Debug helper to check all references
    private void ValidateReferences()
    {
        if (bullet == null)
            Debug.LogError("Bullet prefab is not assigned!");
        if (bulletSpawnPoint == null)
            Debug.LogError("Bullet spawn point is not assigned!");
        if (gun == null)
            Debug.LogError("Gun GameObject is not assigned!");
    }
}
