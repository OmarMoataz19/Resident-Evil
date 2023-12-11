using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrenade : Grenade
{
    public GameObject handGrenadeEffect;
    float force = 700f;
    float handGrenadeDamage = 4f;

    public override void Explode()
    {
        // show effect
        Instantiate(handGrenadeEffect, transform.position, transform.rotation);

        // get nearrby Object
        Collider[] surrounding = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in surrounding)
        {
            // enemy affected and decrement their damage

            // any rigid body affected
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        // remove grenade
        Destroy(gameObject);
    }

    public float GetHandGrenadeDamage()
    {
        return this.handGrenadeDamage;
    }
}
