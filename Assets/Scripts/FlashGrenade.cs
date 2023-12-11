using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashGrenade : Grenade
{
    public GameObject flashGrenadeEffect;
    float knockDownTime = 3.0f;


    public override void Explode()
    {
        // show effect
        Instantiate(flashGrenadeEffect, transform.position, transform.rotation);

        // get nearrby Object
        Collider[] surrounding = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in surrounding)
        {
            if (collider.gameObject.layer == 7 && collider.isTrigger)
            {
                //decrement health
                collider.gameObject.GetComponent<ZombieMain>().StunZombie();
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(700f, transform.position, radius);
            }


        }




        // remove grenade
       // Destroy(gameObject);
    }

    public float getKnockDownTime()
    {
        return knockDownTime;
    }


}
