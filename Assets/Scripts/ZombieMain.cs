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
    public GameObject coinsOnDeath;


    public GameObject currentTarget;
    public GameObject Axe;
    public bool hasWeaopn; 
    public int health;
    public bool isStunned;
    public bool isGrappling;
    public bool grappleBroken;
    public bool stabToKill;
    public bool isDead;


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
    float getDistanceFromLeon(){
        return Vector3.Distance(transform.position,currentTarget.transform.position);
    }


    bool closeFromLeon(float f, bool withY){
        if(withY){
            float diff = (transform.position.y-currentTarget.transform.position.y);
            return getDistanceFromLeon()<=f &&  diff < 0.5 && diff> -0.5;
        }
        return getDistanceFromLeon()<=f;
    }

    // Update is called once per frame
    void Update()
    {
    if (ZombieAnimator.GetCurrentAnimatorStateInfo(2).IsName("Zombie Grab") && 
    ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Punch")){
        Debug.Log("ERROR");
    }

    if (!ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk") || ZombieAnimator.GetCurrentAnimatorStateInfo(2).IsName("Zombie Grab")){
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

        agent.destination = currentTarget.transform.position;
         if(!isStunned && !LeonAnimatorScript.isInvincible &&
         closeFromLeon(1f,true) && ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk")){
            if(LeonAnimatorScript.isGrappled){
                ZombieAnimator.ResetTrigger("waitingOnGrappleDone");
                ZombieAnimator.Play("Zombie Idle 0");
            }
            else{    
                Vector3 directionToTarget = transform.position - currentTarget.transform.position;
                float angle = Vector3.Angle(transform.forward, directionToTarget);
                 if(Mathf.Abs(angle)>150f && Mathf.Abs(angle)<210f){
                if(hasWeaopn){
                    ZombieAnimator.SetTrigger("AxeSwipe");
                }
                else{
                    if(Random.Range(1,2) == 1 && !ZombieAnimator.GetBool("Punch") && !ZombieAnimator.GetBool("isPunch")){
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
        }
        else if (!isStunned && !LeonAnimatorScript.isGrappled && !closeFromLeon(5f,false) && hasWeaopn 
        && (Time.frameCount % 120 == 0) && (Random.Range(1,5) ==1)
        && ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Walk"))
        {
            ZombieAnimator.Play("Throw Axe2");
            hasWeaopn = false;
        }

        if (Input.GetKeyDown(KeyCode.P)){
            StunZombie();
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


            float rotationspeed = 0.025f;
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


        if (ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Stun") && isStunned){
           if(closeFromLeon(1f,true)){
                LeonAnimatorScript.stunnedZombieScipt = this;
                stabToKill = true;
           }else{
            stabToKill = false;
           }
        }else{
            stabToKill = false;
        }        

    }

    public void ThrowAxe(){
        if(isDead){
            return;
        } 
        Axe.GetComponent<AxeThrowProjectile>().enabled = true;
    }

    public void GetHit(int damage){
        if(isDead){
            return;
        }
        print(damage);
        if(damage>= health){
            if(isStunned){
                killWhileStunned();
            }else{
                ZombieAnimator.Play("Zombie Death 2");
                agent.enabled = false;
                knockedParticle.SetActive(false);
                isDead = true;
                // generateGold();
                GetComponent<ZombieMain>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }else{
            if(isStunned){
                ZombieAnimator.SetTrigger("UnStun");
            }else if(!ZombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie UnStun")){
                ZombieAnimator.Play("Zombie Reaction hit",-1,0f);
            }   
            health = health - damage;
        }
    }

    public void StunZombie(){
        if(isDead || isStunned){
            return;
        }
        isStunned = true;
        StartCoroutine(SetKnockedParticle());
        agent.updateRotation  = false;
        ZombieAnimator.Play("Zombie Stun");
        StartCoroutine(SetStunFalse());
    }

    public void checkForGrabHit(){
        if(isDead || isStunned){
            return;
        }
        if(closeFromLeon(1f,true) && !LeonAnimatorScript.isGrappled && !LeonAnimatorScript.isInvincible){
            Vector3 directionToTarget = transform.position - currentTarget.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if(Mathf.Abs(angle)>130f && Mathf.Abs(angle)<230f){
                ZombieAnimator.SetTrigger("Grapple");
            }
        }
    }

    public void chechForAxeSwipeHit(){
        if(isDead || isStunned){
            return;
        }
        if(closeFromLeon(1f,true) && !LeonAnimatorScript.isGrappled && !LeonAnimatorScript.isInvincible){
            Vector3 directionToTarget = transform.position - currentTarget.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if(Mathf.Abs(angle)>150f && Mathf.Abs(angle)<210f){
                LeonAnimatorScript.setSideHitTrigger(hasWeaopn);
            }
         }
    }


    IEnumerator SetStunFalse(){
        yield return new WaitForSeconds(5); //wait 5 seconds
        if(isStunned){
        ZombieAnimator.SetTrigger("UnStun");
        }
        isStunned = false;
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
            // generateGold();
            stabToKill = false;
            knockedParticle.SetActive(false);
            health = -3;
            isDead = true;
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
        Instantiate(coinsOnDeath, transform.position+ new Vector3(0,0.3f,0), Quaternion.identity);
    }



    
}
