using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grenade : MonoBehaviour
{

    public float grenadeDelay = 3f;
    public float radius = 50f;

    // grenade prices
    private float buyPrice = 15f;
    private float sellPrice = 10f;

    float countDownGrenade;
    bool hasExploded;
    bool thrown;

    // Start is called before the first frame update
    void Start()
    {
        countDownGrenade = grenadeDelay;
        hasExploded = false;
        thrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        countDownGrenade -= Time.deltaTime;

        if (thrown && countDownGrenade < 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;

        }

    }

    public abstract void Explode();

    public float GetBuyprice()
    {
        return this.buyPrice;
    }

    public float GetSellPrice()
    {
        return this.sellPrice;
    }

    public bool IsThrown()
    {
        return this.thrown;
    }

    public void ThrowGrenade()
    {
        this.thrown = true;
    }
    public void Release()
    {
        // // set the grenade parent to null
        // transform.parent = null;
        // // set the grenade to be kinematic
        // GetComponent<Rigidbody>().isKinematic = false;
        // // add force to the grenade
        // GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
    }
}
