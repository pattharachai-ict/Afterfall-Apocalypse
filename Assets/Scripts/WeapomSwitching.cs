using UnityEngine;
<<<<<<< HEAD
using UnityEngine.InputSystem;
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

public class WeaponSwitching : MonoBehaviour
{
    [Header("Weapon Scripts")]
    [SerializeField] private MeleeAttack2D meleeAttackScript;
    [SerializeField] private ShootScript shootScript;

    [Header("Weapon GameObjects")]
    [SerializeField] private GameObject meleeWeaponObj; // The visible melee weapon
    [SerializeField] private GameObject gunObj;         // The gun object with ShootScript

    private enum WeaponType { Melee, Ranged }
    private WeaponType currentWeapon = WeaponType.Ranged;

<<<<<<< HEAD
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
=======
    void Start()
    {
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        UpdateWeaponState();
    }

    void Update()
    {
<<<<<<< HEAD
        // Keep keyboard input for testing/backup
=======
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

<<<<<<< HEAD
    // Call this from Player Input's Attack callback
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Only trigger the active weapon's attack
        if (currentWeapon == WeaponType.Melee && meleeAttackScript != null)
        {
            meleeAttackScript.TryMeleeAttack();
        }
        else if (currentWeapon == WeaponType.Ranged && shootScript != null)
        {
            shootScript.TryShoot();
        }
    }

    // Call this from Player Input's Switch Weapon callback
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchWeapon();
        }
    }

   private void SwitchWeapon()
{
    // Cancel reload if switching from gun
    if (currentWeapon == WeaponType.Ranged && shootScript != null)
    {
        shootScript.CancelReloadIfActive();
    }

    // Toggle between Melee and Ranged
    currentWeapon = currentWeapon == WeaponType.Melee ? WeaponType.Ranged : WeaponType.Melee;

    // Update weapon state
    UpdateWeaponState();
    Debug.Log("Switched to: " + currentWeapon);

    if (currentWeapon == WeaponType.Melee)
        audioManager.PlaySFX(audioManager.meleeequip);
    else
        audioManager.PlaySFX(audioManager.gunequip);
}
=======
    private void SwitchWeapon()
    {
        currentWeapon = currentWeapon == WeaponType.Melee ? WeaponType.Ranged : WeaponType.Melee;
        UpdateWeaponState();
        Debug.Log("Switched to: " + currentWeapon);
    }

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    private void UpdateWeaponState()
    {
        bool isMelee = currentWeapon == WeaponType.Melee;

<<<<<<< HEAD
        // CRITICAL FIX: Completely disable the scripts, not just setting enabled flag
        // This prevents them from responding to other input methods
        if (meleeAttackScript != null) 
        {
            meleeAttackScript.enabled = isMelee;
            
            // If using keyboard input too, disable any direct method calls in Update()
            // May need to add this check to MeleeAttack2D script as well
        }
        
        if (shootScript != null) 
        {
            shootScript.enabled = !isMelee;
            
            // Prevent direct input checking in the ShootScript
            // You may need to modify ShootScript to check this too
        }
=======
        // Enable/disable weapon scripts
        if (meleeAttackScript != null) meleeAttackScript.enabled = isMelee;
        if (shootScript != null) shootScript.enabled = !isMelee;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

        // Show/hide weapon visuals
        if (meleeWeaponObj != null) meleeWeaponObj.SetActive(isMelee);
        if (gunObj != null) gunObj.SetActive(!isMelee);
    }
}
<<<<<<< HEAD
=======

>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
