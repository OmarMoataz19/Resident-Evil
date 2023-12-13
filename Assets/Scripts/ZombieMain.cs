using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ZombieMain : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Animator ZombieAnimator;
    LeonAnimationController LeonAnimatorScript;
    RigBuilder rigBuilder;

    public GameObject knockedParticle;

    public GameObject currentTarget;
    public GameObject Axe;
    public bool hasWeaopn; 
    public int health;
    public bool isStunned;
    public bool isGrappling;
    public bool grappleBroken;


    // Start is called before the first frame update
    void Start()
    {
        health = 5;

        if(hasWeaopn){
            Axe.SetActive(true);
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        ZombieAnimator = GetComponent<Animator>();
        LeonAnimatorScript = currentTarget.GetComponent<LeonAnimationController>();
        // rigBuilder = GetComponent<RigBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
    
    if (ZombieAnimator.GetCurrentAnimatorStateInfo(2).IsName("Zombie Grab") && 
    ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Punch")){
        Debug.Log("ERROR");
    }

    if (!ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk")){
        agent.velocity = Vector3.zero;
        // agent.updateRotation  = true;
    
    }else{
        agent.updateRotation  = true;
        isStunned = false;
        knockedParticle.SetActive(false);
        isGrappling = false;
        GetComponent<CapsuleCollider>().enabled = true; 
    }
 
    if(!LeonAnimatorScript.isGrappled){
        ZombieAnimator.SetTrigger("waitingOnGrappleDone");
    }

        agent.destination =   currentTarget.transform.position;
         if(!isStunned && !LeonAnimatorScript.isInvincible &&
         agent.remainingDistance<1f && agent.remainingDistance!=0 && ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk")){
            if(LeonAnimatorScript.isGrappled){
                ZombieAnimator.ResetTrigger("waitingOnGrappleDone");
                ZombieAnimator.Play("Zombie Idle 0");
            }
            else{    
                if(hasWeaopn){
                    ZombieAnimator.SetTrigger("AxeSwipe");
                }
                else{
                    if(Random.Range(1,4) == 1 && !ZombieAnimator.GetBool("Punch") && !ZombieAnimator.GetBool("isPunch")){
                        ZombieAnimator.SetBool("isGrab",true);
                        ZombieAnimator.SetTrigger("Grab");
                    }
                    else if(!ZombieAnimator.GetBool("Grab")  && !ZombieAnimator.GetBool("isGrab") ){
                        ZombieAnimator.SetBool("isPunch",true);
                        ZombieAnimator.SetTrigger("Punch");
                    }
                }
            }
        }
        else if (!isStunned && !LeonAnimatorScript.isGrappled && agent.remainingDistance>5f && hasWeaopn 
        && (Time.frameCount % 120 == 0) && (Random.Range(1,5) ==1)
        && ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk"))
        {
            ZombieAnimator.Play("Throw Axe2");
            hasWeaopn = false;
        }

        if (Input.GetKeyDown(KeyCode.J)){
            GetHit(1);
        }

        if (Input.GetKeyDown(KeyCode.P)){
            StunZombie();
        }

        if (Input.GetKeyDown(KeyCode.U)){
            ZombieAnimator.Play("Zombie Death");
            agent.enabled = false;
            GetComponent<ZombieMain>().enabled = false;
        } 

        if (Input.GetKeyDown(KeyCode.T)){
            ZombieAnimator.Play("Zombie Grab");
        } 
        
        if (Input.GetKeyDown(KeyCode.Y)){
            ZombieAnimator.Play("Zombie Grapple Release");
        } 


        if (Input.GetKeyDown(KeyCode.B)){
            // rigBuilder.enabled = false;
            killWhileStunned();
        } 

        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Grapple")){
            if(!isGrappling){
                GetComponent<CapsuleCollider>().enabled = false;
                isGrappling = true;
                LeonAnimatorScript.holdingZombieScipt = this;
                LeonAnimatorScript.startGrapple();
                StartCoroutine(SetGrapplingFalse());
                agent.updateRotation  = false;
            }

            Vector3 relativePos = new Vector3(currentTarget.transform.position.x - transform.position.x,0,currentTarget.transform.position.z - transform.position.z);
            Quaternion targetrotation = Quaternion.LookRotation(relativePos);
            Vector3 targetPos = transform.position + (transform.forward * 0.2f);
            targetPos.y = currentTarget.transform.position.y;

            float rotationspeed = 0.015f;
            float movementspeed = 0.025f;

            currentTarget.transform.rotation = Quaternion.Lerp(currentTarget.transform.rotation, targetrotation, Time.time * rotationspeed);
            currentTarget.transform.position = Vector3.Lerp(currentTarget.transform.position, targetPos, Time.time * movementspeed);
            // Physics.SyncTransforms();
        }

        // moving the player away from the zombie when he breaks the grapple
        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Grapple Release")){
            currentTarget.transform.position = currentTarget.transform.position + transform.forward * 0.5f * Time.deltaTime;
        }

        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Neck Bite")){
            if(ZombieAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<0.3f){
                currentTarget.transform.position = currentTarget.transform.position + currentTarget.transform.forward * 0.3f * Time.deltaTime;
            }
        }

        // checking if player is still close to zombie after grab action
        if (ZombieAnimator.GetCurrentAnimatorStateInfo(2).IsName("Zombie Grab")){
            if(ZombieAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime>0.7f && 
            ZombieAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime<0.8f){
                checkForGrabHit();
            }
        }

        
        // checking if player is still close to axe swipe action
        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Axe hit")){
            if(ZombieAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.4f && 
            ZombieAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<0.5f){
                chechForAxeSwipeHit();
            }
        }

        // checking if player is still close to hand swipe action
        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Punch")){
            if(ZombieAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.4f && 
            ZombieAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<0.5f){
                chechForAxeSwipeHit();
            }
        }        


    }

    public void ThrowAxe(){
            Axe.GetComponent<AxeThrowProjectile>().enabled = true;
    }

    public void GetHit(int damage){
        print(damage);
        if(damage>= health){
            ZombieAnimator.Play("Zombie Death 2");
            agent.enabled = false;
            generateGold();
            GetComponent<ZombieMain>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }else{
            ZombieAnimator.Play("Zombie Reaction hit");
            health = health - damage;
        }
    }

    public void StunZombie(){
        isStunned = true;
        StartCoroutine(SetKnockedParticle());
        agent.updateRotation  = false;
        ZombieAnimator.Play("Zombie Stun");
        StartCoroutine(SetStunFalse());
    }

    public void checkForGrabHit(){
        if(agent.remainingDistance<1f && !LeonAnimatorScript.isGrappled && !LeonAnimatorScript.isInvincible){
            Vector3 directionToTarget = transform.position - currentTarget.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if(Mathf.Abs(angle)>130f && Mathf.Abs(angle)<230f){
                ZombieAnimator.SetTrigger("Grapple");
            }
        }
    }

    public void chechForAxeSwipeHit(){
        if(agent.remainingDistance<1f && !LeonAnimatorScript.isGrappled && !LeonAnimatorScript.isInvincible){
            Vector3 directionToTarget = transform.position - currentTarget.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if(Mathf.Abs(angle)>150f && Mathf.Abs(angle)<210f){
                LeonAnimatorScript.setSideHitTrigger(hasWeaopn);
            }
         }
    }


    IEnumerator SetStunFalse(){
        yield return new WaitForSeconds(5); //wait 5 seconds
        ZombieAnimator.SetTrigger("UnStun");
    }

    IEnumerator SetGrapplingFalse(){
        grappleBroken = false;
        yield return new WaitForSeconds(4); //wait 4 seconds
        if(!grappleBroken){
            ZombieAnimator.SetTrigger("Bite");
            LeonAnimatorScript.getBit();
        }
        grappleBroken = false;
    }

    public void distractedByGrenade(){
        if(LeonAnimatorScript.actualGrenade){
            LeonAnimatorScript.actualGrenade = false;
            // rigBuilder.enabled = true;
        }
        ZombieAnimator.SetTrigger("UnGrapple");
    }

    public void killWhileStunned(){
        if(isStunned){
            GetComponent<CapsuleCollider>().enabled = false;
            generateGold();
            knockedParticle.SetActive(false);
            ZombieAnimator.SetTrigger("Death 2");
            agent.enabled = false;
            GetComponent<ZombieMain>().enabled = false;
        }
    }

    IEnumerator SetKnockedParticle(){
        yield return new WaitForSeconds(1); //wait 5 seconds
        knockedParticle.SetActive(true);
    }

    public void generateGold(){
        Debug.Log(Random.Range(5,51));
    }



    
}
