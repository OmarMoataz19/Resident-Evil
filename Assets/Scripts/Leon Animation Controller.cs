using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class LeonAnimationController : MonoBehaviour
{
    public MainController mainController;
    Animator LeonAnimator;
    StarterAssets.StarterAssetsInputs playerInputController;
    public GameObject knife;
    public GameObject invertedKnife;
    public ZombieMain holdingZombieScipt;
    public ZombieMain stunnedZombieScipt;
    public TextMeshProUGUI UI;
    public bool test;
    
    public bool isGrappled;
    public bool isInvincible;
    private bool AxeSwipe;
    public bool threwGrenade;
    public bool actualGrenade;
    
    private bool showText;

    public int LeonHP = 8;

    public DamageHud damageHud;


    

    // Start is called before the first frame update
    void Start()
    {
        LeonAnimator = GetComponent<Animator>();
        playerInputController = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

     
    // Update is called once per frame
    void Update()
    {

        if(showText){
        if( mainController.GetCurrentDurability() >=2){
            UI.text = "Press E to Stab \n";
        }
        if(mainController.GetCurrentGrenade() != null){
            UI.text += "Press G to throw grenade";
        }
        } 
        else{
            UI.text = "";
        }

        if(UI.text == "" && stunnedZombieScipt!=null && stunnedZombieScipt.stabToKill && mainController.GetCurrentDurability() >=1){
            UI.text = "Press E to Stab";
        }
    
        //Leon Uses his grenade
        if(isGrappled && showText && mainController.GetCurrentGrenade() != null && Input.GetKeyDown(KeyCode.G)){
            LeonAnimator.Play("Leon Throw Up Grenade");
            threwGrenade = true;
            holdingZombieScipt.grappleBroken = true;
            showText = false;
            actualGrenade = true;
        }

        if (isGrappled&& mainController.GetCurrentDurability() >=2 && showText && Input.GetKeyDown(KeyCode.E)){
            LeonAnimator.SetTrigger("StabWhileGrappled");
            Inventory.Instance.audioSource4.PlayOneShot(Inventory.Instance.knifeStab);
            holdingZombieScipt.grappleBroken = true;
            showText = false;
            threwGrenade = true;
            mainController.SetKnifeDurability(mainController.GetCurrentDurability() - 2);
        }

        if (stunnedZombieScipt!=null && stunnedZombieScipt.stabToKill &&
         mainController.GetCurrentDurability() >=1 && Input.GetKeyDown(KeyCode.E)){
            LeonAnimator.Play("Leon Stab Zombie 3");
            LeonAnimator.Play("Leon Stand To Kneel 2");
            mainController.SetKnifeDurability(mainController.GetCurrentDurability() - 1);
            Inventory.Instance.audioSource4.PlayOneShot(Inventory.Instance.knifeStab);
            stunnedZombieScipt.killWhileStunned();
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

        if(LeonAnimator.GetCurrentAnimatorStateInfo(3).IsName("Leon Throw Up Grenade")){
           if(threwGrenade){
            threwGrenade = false;
            holdingZombieScipt.distractedByGrenade();
            LeonAnimator.SetTrigger("ReleaseFast");

           }
           if(LeonAnimator.GetCurrentAnimatorStateInfo(3).normalizedTime>0.96f){
            startMovment();
           }
        }






        if (LeonAnimator.GetCurrentAnimatorStateInfo(1).IsName("Leon Bite")){
            showText = false;
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
        showText = false;
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
        playerInputController.aim = false; 
        playerInputController.canAim = false;   
        isGrappled = true;
        showText = true;

        hideWeapons();
    }

    public void startMovment(){
        playerInputController.canMove = true;
        playerInputController.canJump = true;
        playerInputController.canSprint = true; 
        playerInputController.canAim = true;   
        isGrappled = false;
        ShowWeapon();

    }

        public void stopMovment2(){
        playerInputController.move = new Vector2(0f,0f);
        playerInputController.canMove = false;
        playerInputController.canJump = false;
        playerInputController.canSprint = false;  
        playerInputController.aim = false; 
        playerInputController.canAim = false;   
        hideWeapons();
    }

        public void startMovment2(){
        playerInputController.canMove = true;
        playerInputController.canJump = true;
        playerInputController.canSprint = true; 
        playerInputController.canAim = true;   
        ShowWeapon();
    }

    public void setSideHitTrigger(bool hasWeaopn){
        AxeSwipe = hasWeaopn;
        LeonAnimator.SetTrigger("SideHit");
    }

    public void dealDamage(int x){
        if(x>=mainController.GetHp()){
            LeonHP = 0;
            mainController.SetHp(LeonHP);
            LeonAnimator.SetTrigger("Death");
            stopMovment();
        }
        else{
            LeonHP = mainController.GetHp() - x;
            mainController.SetHp(LeonHP);
        }
        damageHud.TakeDamage();
        if(Random.Range(0,2)==0){
            Inventory.Instance.audioSource2.PlayOneShot(Inventory.Instance.leonDamage);
        }
        else{
            Inventory.Instance.audioSource2.PlayOneShot(Inventory.Instance.leonDamage2);
        }

         if (Random.Range(0,3)==0){
            Inventory.Instance.audioSource3.PlayOneShot(Inventory.Instance.zombieAttack);
        }
        else if (Random.Range(0,3)==1){
            Inventory.Instance.audioSource3.PlayOneShot(Inventory.Instance.zombieAttack2);
        }
        else{
            Inventory.Instance.audioSource3.PlayOneShot(Inventory.Instance.zombieAttack3);
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

    public void hideWeapons(){
        mainController.HideWeapons();
    }

    public void ShowWeapon(){
        mainController.ShowWeapon();
    }


    
}
