using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class MainController : MonoBehaviour
{
    private int hp;
    private int gold;
    public HealthController  healthController;
    private Weapon currentWeapon;
    public Weapon[] weapons;
    public GameObject[] weaponObjects;
    public Grenade currentGrenade;
    public Grenade[] grenades;
    public GameObject[] grenadeObjects;
    private int knifeDurability;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI gold2Text;
    public TextMeshProUGUI hp2Text;
    public TextMeshProUGUI knifeDurabilityText;
    public Inventory inventory;

    public CinemachineVirtualCamera aimCamera;
    private float pistolsShoulderOffset = 0.7f;
    private float pistolsShoulderOffsetZ = 1.09f;
    private float akShoulderOffset = 0.9f;
    private float akShoulderOffsetZ = 1.4f;

    private bool enableCheats = false;
    public Cheats cheats;
    void Start()
    {
        hp = 8;
        gold = 30;
        knifeDurability = 10;
    }
    void Update()
    {
        goldText.text = gold + "";
        hpText.text = hp + "";
        gold2Text.text = gold + "";
        hp2Text.text = hp + "";
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            enableCheats = !enableCheats;
        }
    }
    public void EquipWeapon(WeaponItem WeaponItem)
    {
        foreach (var weapon in weapons)
        {
            if (WeaponItem.itemName == "Pistol")
            {
                currentWeapon = weapons[0];
                weaponObjects[0].SetActive(true);
                weaponObjects[1].SetActive(false);
                weaponObjects[2].SetActive(false);
                weaponObjects[3].SetActive(false);
                break;
            }
            else if (WeaponItem.itemName == "AK-14")
            {
                currentWeapon = weapons[1];
                weaponObjects[0].SetActive(false);
                weaponObjects[1].SetActive(true);
                weaponObjects[2].SetActive(false);
                weaponObjects[3].SetActive(false);
                break;
            }
            else if (WeaponItem.itemName == "Revolver")
            {
                currentWeapon = weapons[2];
                weaponObjects[0].SetActive(false);
                weaponObjects[1].SetActive(false);
                weaponObjects[2].SetActive(true);
                weaponObjects[3].SetActive(false);
                break;
            }
            else if (WeaponItem.itemName == "Shotgun")
            {
                currentWeapon = weapons[3];
                weaponObjects[0].SetActive(false);
                weaponObjects[1].SetActive(false);
                weaponObjects[2].SetActive(false);
                weaponObjects[3].SetActive(true);
                break;
            }
        }
        currentWeapon.SetBulletsLeft(WeaponItem.Ammo); //bullets left
        fixCamera();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public void SetHp(int newHp)
    {
        if(cheats.isInvincible)
        {
            return;
        }
        healthController.SetHp(newHp);
        hp = newHp;
    }

    public Grenade GetCurrentGrenade()
    {
        return currentGrenade;
    }
    public void EquipGrenade(GrenadeItem grenadeItem , int index)
    {
        foreach (var grenade in grenades)
        {
            if (grenadeItem.itemName == "Hand Grenade")
            {
                currentGrenade = grenades[0];
                grenadeObjects[0].SetActive(true);
                grenadeObjects[1].SetActive(false);
                currentGrenade.equipIndex = index;
                break;
            }
            else if (grenadeItem.itemName == "Flash Grenade")
            {
                currentGrenade = grenades[1];
                grenadeObjects[0].SetActive(false);
                grenadeObjects[1].SetActive(true);
                currentGrenade.equipIndex = index;
                break;
            }
           
        } 
    }
    public void HideGrenade()
    {
        grenadeObjects[0].SetActive(false);
        grenadeObjects[1].SetActive(false);
        currentGrenade = null;
    }
    public void HideWeapons()
    {
        weaponObjects[0].SetActive(false);
        weaponObjects[1].SetActive(false);
        weaponObjects[2].SetActive(false);
        weaponObjects[3].SetActive(false);
    }
    public void ShowWeapon()
    {
        if(currentWeapon == weapons[0])
        {
            weaponObjects[0].SetActive(true);
        }
        else if(currentWeapon == weapons[1])
        {
            weaponObjects[1].SetActive(true);
        }
        else if(currentWeapon == weapons[2])
        {
            weaponObjects[2].SetActive(true);
        }
        else if(currentWeapon == weapons[3])
        {
            weaponObjects[3].SetActive(true);
        }
    }
    public int GetCurrentDurability()
    {
        return knifeDurability;
    }
    public void SetKnifeDurability (int value)
    {
        knifeDurability = value;
        knifeDurabilityText.text = value + "/10";
    }
    public int GetHp()
    {
        return hp;
    }
    public int GetGold()
    {
        return gold;
    }
    public void SetGold (int gold)
    {
        this.gold = gold;
    }
    public void fixCamera()
    {
        var thirdPersonFollow = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        if (thirdPersonFollow != null)
        {
            Vector3 currentOffset = thirdPersonFollow.ShoulderOffset;
            if(currentWeapon.name =="pistol" || currentWeapon.name =="revolver")
            {
                currentOffset.x = pistolsShoulderOffset;
                currentOffset.z = pistolsShoulderOffsetZ;
            }
            else
            {
                currentOffset.x = akShoulderOffset;
                currentOffset.z = akShoulderOffsetZ;
            }
            thirdPersonFollow.ShoulderOffset = currentOffset;
        }
    }
    public bool GetCheats()
    {
        return enableCheats;
    }
}
