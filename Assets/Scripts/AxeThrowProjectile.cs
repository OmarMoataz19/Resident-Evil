using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrowProjectile : MonoBehaviour
{
    public Transform Axe;
    public Transform Target;
    public float firingAngle = 30.0f;
    public float gravity = 9.8f;
    public float yOffset = 0.0f;
    public Transform Projectile;      
    private Transform myTransform;
    public bool didHit;
    
    void Awake()
    {
        myTransform = transform;      
    }
 
    void Start()
    {        
        firingAngle = 30.0f;
        gravity = 7.8f;
        yOffset = 0.0f;  
        transform.parent = null;
        StartCoroutine(SimulateProjectile());
        GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(0.1f);
       
        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
       
        // Calculate distance to target, plus y offset
        float target_Distance = (Vector3.Distance(Projectile.position,Target.position) * 1.2f);
        // target_Distance+= yOffset;

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / (Vx * 1f);
   
        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
        // Projectile.rotation = Quaternion.LookRotation( new Vector3(Target.position.x,Target.position.y+yOffset,Target.position.z) - Projectile.position);
        

        float elapse_time = 0;
 
        while ((elapse_time < flightDuration) && (Projectile.position.y>Target.position.y) )
        {   
            if(didHit){
                GetComponent<BoxCollider>().enabled = false;
                Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0);
            }else{
                Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            }
            Axe.Rotate(0.0f,4.0f,0.0f);
            elapse_time += Time.deltaTime;
            yield return null;
        }
        GetComponent<BoxCollider>().enabled = false;

    }  
}
