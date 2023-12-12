using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private void Awake()
    {
        damage = 1;
        timeBetweenShooting = 0.2f; 
        weaponRange = Range.Medium;
        reloadTime = 2.6f; 
        magazineSize = 30;
        bulletsLeft = magazineSize;
        firingMode = FiringMode.Automatic;
        // The Animator component and the attackPoint are set in the Inspector
        
    }

    protected override void HandleHit(RaycastHit hit)
    {
        if (IsTargetInRange(hit))
        {
            DealDamage(hit, damage);
        }
  
    }
}
