using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private int hp;
    private int gold;
    //knife durability..
    // inventory

    [SerializeField] private Weapon startingWeapon;
    private Weapon currentWeapon;
    void Start()
    {
        EquipWeapon(startingWeapon);
    }
    void Update()
    {
        
    }
    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            // todo: Handle unequipping the current weapon 
        }

        currentWeapon = weapon;
        // todo: Handle equipping the new weapon (e.g., updating UI, setting weapon visible, etc.)
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
