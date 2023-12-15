using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private void Awake()
    {
        damage = 2;
        timeBetweenShooting = 0.2f;
        weaponRange = Range.Medium;
        reloadTime = 4.1f; 
        magazineSize = 12;
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
