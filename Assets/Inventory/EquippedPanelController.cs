using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquippedPanelController : MonoBehaviour
{
    // Weapon Data.
    public Image WeaponImage;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI WeaponAmmo;

    // Granade Data.
    public Image GrenadeImage;
    public TextMeshProUGUI GrenadeName;

    public MainController mainController;

    public Sprite sprite;
    public void EquipWeapon(WeaponItem weaponItem)
    {
        WeaponImage.sprite = weaponItem.icon;
        WeaponName.text = weaponItem.itemName;

        var totalAmmo = 0;
        if (weaponItem.WeaponType == WeaponType.Pistol)
        {
            totalAmmo = 12;
        }
        else if (weaponItem.WeaponType == WeaponType.Revolver)
        {
            totalAmmo = 6;
        }
        else if (weaponItem.WeaponType == WeaponType.AssaultRifle)
        {
            totalAmmo = 30;
        }
        else if (weaponItem.WeaponType == WeaponType.ShotGun)
        {
            totalAmmo = 8;
        }

        WeaponAmmo.text = $"{weaponItem.Ammo}/{totalAmmo}";
        mainController.EquipWeapon(weaponItem);
    }

    public void EquipGrenade(GrenadeItem grenadeItem , int index)
    {
        GrenadeName.text = grenadeItem.itemName;
        GrenadeImage.sprite = grenadeItem.icon;
        mainController.EquipGrenade(grenadeItem , index);
    }
    public void RemoveGrenade ()
    {
        GrenadeName.text = "";
        GrenadeImage.sprite = sprite;
    }

}
