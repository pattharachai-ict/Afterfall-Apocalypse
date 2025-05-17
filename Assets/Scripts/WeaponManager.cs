using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Scripts")]
    [SerializeField] private ShootScript shootScript;
    [SerializeField] private MeleeAttack2D meleeScript;

    public enum WeaponType { Ranged, Melee }
    public WeaponType currentWeapon = WeaponType.Melee;

    private void Awake()
    {
        if (shootScript == null)
            shootScript = GetComponent<ShootScript>();
        if (meleeScript == null)
            meleeScript = GetComponent<MeleeAttack2D>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (currentWeapon)
            {
                case WeaponType.Ranged:
                    if (shootScript != null)
                        shootScript.TryShoot();  // Use method below
                    break;

                case WeaponType.Melee:
                    if (meleeScript != null)
                        meleeScript.TryMeleeAttack();  // Use method below
                    break;
            }
        }
    }

    // Optional method to switch weapon type
    public void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
    }
}
