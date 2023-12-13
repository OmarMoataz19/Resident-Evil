using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrenade : Grenade
{
    public GameObject handGrenadeEffect;
    float force = 700f;
    int handGrenadeDamage = 4;
    public LeonAnimationController leonAnimationController;
    private bool alreadyTookDamage = false;
    public override void Explode()
    {
        // show effect
        Instantiate(handGrenadeEffect, transform.position, transform.rotation);

        // get nearrby Object
        Collider[] surrounding = Physics.OverlapSphere(transform.position, radius);

        alreadyTookDamage = false;
        foreach (Collider collider in surrounding)
        {
            // enemy affected and decrement their damage
            if (collider.gameObject.layer == 7 && collider.isTrigger)
            {
                //decrement health
                collider.gameObject.GetComponent<ZombieMain>().GetHit(handGrenadeDamage);
            }
            // any rigid body affected
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
            if(collider.gameObject.layer == 8 && !alreadyTookDamage)
            {
                leonAnimationController.dealDamage(handGrenadeDamage);
                alreadyTookDamage = true;
            }
        }
    }

    public float GetHandGrenadeDamage()
    {
        return this.handGrenadeDamage;
    }
}
