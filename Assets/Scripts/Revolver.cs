using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Weapon
{
    private void Awake()
    {
        damage = 5;
        timeBetweenShooting = 1.0f;
        weaponRange = Range.Long;
       // reloadTime = 4f; 
        magazineSize = 6;
        bulletsLeft = magazineSize;
        firingMode = FiringMode.SingleShot;
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

