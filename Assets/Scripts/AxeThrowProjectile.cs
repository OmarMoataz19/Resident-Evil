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

        yield return new WaitForSeconds(0.1f);
       

        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
       
      
        float target_Distance = (Vector3.Distance(Projectile.position,Target.position) * 1.2f);
        

        
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
      
        float flightDuration = target_Distance / (Vx * 1f);
   
        
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
        
        

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
