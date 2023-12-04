using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{

    public float grenadeDelay = 3f;
    public float radius = 15f;
    public float force = 700f;
    public GameObject handGrenade;
    float countDownGrenade;
    bool hasExploded;
   
    // Start is called before the first frame update
    void Start()
    {
        countDownGrenade = grenadeDelay;
        hasExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        countDownGrenade -= Time.deltaTime;

        if (countDownGrenade < 0 && !hasExploded)
        {
            Explode(handGrenade);
            hasExploded=true;
            Debug.Log("explode");
        }



        
    }

    void Explode(GameObject explosionEffect) {
        // show effect
        Instantiate(handGrenade , transform.position , transform.rotation);
        
        // get nearrby Object
        Collider [] surrounding  = Physics.OverlapSphere(transform.position , radius);

        foreach (Collider collider in surrounding)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(force , transform.position , radius);
            }


        }


        // remove grenade
        Destroy(gameObject);
    
    }
}
