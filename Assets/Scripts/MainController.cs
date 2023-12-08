using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private int hp;
    private int gold;
    public HealthController  healthController;
    //knife durability..
    // inventory

    [SerializeField] private Weapon startingWeapon;
    private Weapon currentWeapon;
    void Start()
    {
        hp = 8;
        EquipWeapon(startingWeapon);
    }
    void Update()
    {
        //to be removed 
        //if key is pressed is 1 increase hp by 1 
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            healthController.SetHp(hp + 1);
            hp++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            healthController.SetHp(hp - 1);
            hp --;

        }
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
