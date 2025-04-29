using UnityEngine;

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

    void Start()
    {
        UpdateWeaponState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        currentWeapon = currentWeapon == WeaponType.Melee ? WeaponType.Ranged : WeaponType.Melee;
        UpdateWeaponState();
        Debug.Log("Switched to: " + currentWeapon);
    }

    private void UpdateWeaponState()
    {
        bool isMelee = currentWeapon == WeaponType.Melee;

        // Enable/disable weapon scripts
        if (meleeAttackScript != null) meleeAttackScript.enabled = isMelee;
        if (shootScript != null) shootScript.enabled = !isMelee;

        // Show/hide weapon visuals
        if (meleeWeaponObj != null) meleeWeaponObj.SetActive(isMelee);
        if (gunObj != null) gunObj.SetActive(!isMelee);
    }
}

