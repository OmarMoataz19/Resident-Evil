using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeonAnimationController : MonoBehaviour
{
    public MainController mainController;
    Animator LeonAnimator;
    StarterAssets.StarterAssetsInputs playerInputController;
    public GameObject knife;
    public GameObject invertedKnife;
    public ZombieMain holdingZombieScipt;
    
    public bool isGrappled;
    public bool isInvincible;
    private bool AxeSwipe;
    public bool threwGrenade;
    public bool actualGrenade;

    public int LeonHP = 8;



    // Start is called before the first frame update
    void Start()
    {
        LeonAnimator = GetComponent<Animator>();
        playerInputController = GetComponent<StarterAssets.StarterAssetsInputs>();

    }

    // Update is called once per frame
    void Update()
    {
    
        //Leon Uses his grenade
        if(isGrappled && Input.GetKeyDown(KeyCode.X)){
            LeonAnimator.Play("Leon Throw Up Grenade");
            threwGrenade = true;
            holdingZombieScipt.grappleBroken = true;
            actualGrenade = true;
        }

        if (isGrappled && Input.GetKeyDown(KeyCode.F)){
            LeonAnimator.SetTrigger("StabWhileGrappled");
            holdingZombieScipt.grappleBroken = true;
            threwGrenade = true;
        }

        if(LeonAnimator.GetCurrentAnimatorStateInfo(5).IsName("Leon Knife Grapple Stab 2")){
            if(LeonAnimator.GetCurrentAnimatorStateInfo(5).normalizedTime>0.3f && threwGrenade){
                holdingZombieScipt.distractedByGrenade();
                threwGrenade = false;
                LeonAnimator.SetTrigger("GrappleRelease");
            }
        }

        if(LeonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Leon Grapple Release")){
           if(threwGrenade){
            threwGrenade = false;
            holdingZombieScipt.distractedByGrenade();

           }
           if(LeonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.96f){
            startMovment();
           }
        }


        if (LeonAnimator.GetCurrentAnimatorStateInfo(1).IsName("Leon Bite")){
            if(LeonAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime>0.9f){
                if(!isInvincible){
                    startMovment();
                    dealDamage(5);
                }
            }
        }

        if (LeonAnimator.GetCurrentAnimatorStateInfo(2).IsName("Leon Axe Throw Hit")){
            if(LeonAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime>0.9f){
                if(!isInvincible){
                    dealDamage(3);
                }
            }
        }

        if (LeonAnimator.GetCurrentAnimatorStateInfo(2).IsName("Leon Side Hit")){
            if(LeonAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime>0.9f){
                if(!isInvincible){
                    if(AxeSwipe){
                        dealDamage(2);
                    }
                    else{
                        dealDamage(1);
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.H)){
           LeonAnimator.Play("Leon Stab Zombie 3");
           LeonAnimator.Play("Leon Stand To Kneel 2");
        }
    
    }


    public Animator getAnim(){
        return LeonAnimator;
    }

    public void startGrapple(){
        LeonAnimator.Play("Leon Grapple");
        stopMovment();
    }

    public void stopGrapple(){
        LeonAnimator.Play("Leon Grapple Release");
        startMovment();
    }

    public void getBit(){
        LeonAnimator.Play("Leon Bite Pause");
        LeonAnimator.Play("Idle Walk Run Blend");
    }

    public void axeThrowHit(){
        if(!isInvincible){
            LeonAnimator.Play("Leon Axe Throw Hit");
        }
    }

    public void sideHit(){
        LeonAnimator.Play("Leon Side Hit");
    }


    public void stopMovment(){
        playerInputController.move = new Vector2(0f,0f);
        playerInputController.canMove = false;
        playerInputController.canJump = false;
        playerInputController.canSprint = false;   
        isGrappled = true;
    }

    public void startMovment(){
        playerInputController.canMove = true;
        playerInputController.canJump = true;
        playerInputController.canSprint = true; 
        isGrappled = false;
    }

    public void setSideHitTrigger(bool hasWeaopn){
        AxeSwipe = hasWeaopn;
        LeonAnimator.SetTrigger("SideHit");
    }

    public void dealDamage(int x){
        if(x>=LeonHP){
            LeonHP = 0;
            mainController.SetHp(LeonHP);
            LeonAnimator.SetTrigger("Death");
            stopMovment();
        }
        else{
            LeonHP -= x;
            mainController.SetHp(LeonHP);
        }
        isInvincible = true;
        StartCoroutine(SetInvincibleFalse());
    }

    IEnumerator SetInvincibleFalse(){
        yield return new WaitForSeconds(2); //wait 2 seconds
        isInvincible = false;
    }


    public void showKnife(int x){
        if(x==0){
            knife.SetActive(true);
        }
        else{
            invertedKnife.SetActive(true);
        } 

    }

    public void hideKnife(int x){
        if(x==0){
            knife.SetActive(false);
        }
        else{
            invertedKnife.SetActive(false);
        } 

    }    


    
}
