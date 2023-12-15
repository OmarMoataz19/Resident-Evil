using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private void Awake()
    {
        damage = 3; 
        timeBetweenShooting = 0.5f;
        weaponRange = Range.Short;
        reloadTime = 3.3f; 
        magazineSize = 8;
        bulletsLeft = magazineSize;
        firingMode = FiringMode.SingleShot;
        // The Animator component and the attackPoint are set in the Inspector

    }

    protected override void HandleHit(RaycastHit hit)
    {
        if (IsTargetInRange(hit))
        {
            // Shotgun could have a spread and hit multiple targets
            DealDamage(hit, damage);
        }
    }
}

