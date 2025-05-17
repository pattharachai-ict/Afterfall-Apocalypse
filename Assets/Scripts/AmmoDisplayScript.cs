using UnityEngine;
using TMPro;

public class AmmoDisplayScript : MonoBehaviour
{
    [Header("References")]
    public ShootScript shootScript;
    public TMP_Text magAmmoText;
    public TMP_Text reserveAmmoText;

    void Update()
    {
        if (shootScript != null)
        {
            magAmmoText.text = $"Mag: {shootScript.currentClip}/{shootScript.maxClipSize}";
            reserveAmmoText.text = $"Ammo: {shootScript.currentAmmo}/{shootScript.maxAmmoSize}";
        }
    }
}
