using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
public class ThirdPersonShootingController : MonoBehaviour
{
    //to get the current weapon and switch between the layers & masks
    public MainController mainController;
    
    //colliders
    public LayerMask aimColliderLayerMask = new LayerMask();

    //variables for controlling ads & shooting
    public float normalSensitivity = 1f;
    public float aimSensitivity = 0.5f;
    public CinemachineVirtualCamera aimVirtualCamera;
    public GameObject crosshair;
    public Transform debugTransform;
    public RigBuilder rigBuilder;

    // private fields for the logic 
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private float autoFireTimer = 0f;  // Timer for automatic firing
    private Transform transform;
    private Animator animator;  
    private int currentLayerIndex;
        
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        transform =  GetComponent<Transform>();
        currentLayerIndex = 6;
        //get the hud textmeshpro element based on the current weapon

    }

    void Update()
    {
        HandleAiming(starterAssetsInputs.aim);

        if (starterAssetsInputs.aim)
        {
            HandleShooting();   
        }
       // Update the auto fire timer: check this
        if (autoFireTimer > 0)
        {
            autoFireTimer -= Time.deltaTime;
        }

        if(mainController.GetCurrentWeapon().name == "pistol" || mainController.GetCurrentWeapon().name == "revolver")
        {
            currentLayerIndex = 6;
            animator.SetLayerWeight(7, Mathf.Lerp(animator.GetLayerWeight(7), 0f, Time.deltaTime * 10f));
        }
        else //todo :check if the user can have no weapon equipped..
        {
            currentLayerIndex = 7;
            animator.SetLayerWeight(currentLayerIndex, Mathf.Lerp(animator.GetLayerWeight(currentLayerIndex), 1f, Time.deltaTime * 10f));
        }
        HandleReload();

    }

    private void HandleAiming(bool isAiming)
    {
        aimVirtualCamera.gameObject.SetActive(isAiming);
        thirdPersonController.SetSensitivity(isAiming ? aimSensitivity : normalSensitivity);
        thirdPersonController.SetRotateOnMove(!isAiming);

        if (isAiming)
        {
            mainController.GetCurrentWeapon().EnableWeaponHUD();
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            RotateTowardsTarget(mouseWorldPosition);
            animator.SetLayerWeight(6, Mathf.Lerp(animator.GetLayerWeight(6), 1f, Time.deltaTime * 10f));
            if( currentLayerIndex == 6)
            {
                animator.SetBool("Aim",true);
            }
            else
            {
                animator.SetBool("AimRifle",true);
            }
            crosshair.SetActive(true);
            starterAssetsInputs.sprint = false;
            rigBuilder.enabled = true;

            //rotate the weapon if it is a pistol or a revolver weapon
            if (mainController.GetCurrentWeapon().name == "revolver")
            {
                Transform weaponTransform = mainController.GetCurrentWeapon().transform;
                
                // Adjust these values in the editor to find the correct orientation
                Quaternion targetRotation = Quaternion.Euler(-102.59f, 90, 180);
                Vector3 targetPosition = new Vector3(0.027f, 0.05f, -0.03f);
                
                // Apply smooth transition
                weaponTransform.localRotation = Quaternion.Lerp(weaponTransform.localRotation, targetRotation, Time.deltaTime * 10f);
                weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, targetPosition, Time.deltaTime * 10f);
            }
            if (mainController.GetCurrentWeapon().name == "pistol")
            {
                Transform weaponTransform = mainController.GetCurrentWeapon().transform;
                
                // Adjust these values in the editor to find the correct orientation
                Quaternion targetRotation = Quaternion.Euler(-101.753f, 90, 180);
                Vector3 targetPosition = new Vector3(-0.0003734734f, -0.07866945f, -0.03000128f);
                
                // Apply smooth transition
                weaponTransform.localRotation = Quaternion.Lerp(weaponTransform.localRotation, targetRotation, Time.deltaTime * 10f);
                weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, targetPosition, Time.deltaTime * 10f);
            }

        }
        else
        {
            mainController.GetCurrentWeapon().DisableWeaponHUD();
            animator.SetLayerWeight(6, Mathf.Lerp(animator.GetLayerWeight(6), 0f, Time.deltaTime * 10f));
            crosshair.SetActive(false);
            rigBuilder.enabled = false;
            if( currentLayerIndex == 6)
            {
                animator.SetBool("Aim", false);
            }
            else
            {
                animator.SetBool("AimRifle", false);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        float tempX = 0f;
        float tempY = 0f;
        if (mainController.GetCurrentWeapon().name == "pistol" || mainController.GetCurrentWeapon().name == "revolver" )
        {
            tempX = 240f;
            tempY = -20f;
        }
        else
        {
            tempX = 400;
            tempY = -20f;
        }
        Vector2 screenCenterPosition = new Vector2((Screen.width  / 2f) + tempX , (Screen.height  / 2f) + tempY);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = hit.point;
            return hit.point;
        }
        return Vector3.zero;
    }

    private void RotateTowardsTarget(Vector3 target)
    {
        target.y = transform.position.y;
        Vector3 aimDirection = (target - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f); // experimental value
    }

    private void HandleShooting()
    {
        Weapon currentWeapon = mainController.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            if (starterAssetsInputs.shoot)
            {
                if (currentWeapon.GetFiringMode() == Weapon.FiringMode.SingleShot)
                {
                    currentWeapon.Shoot();
                    starterAssetsInputs.shoot = false;
                }
                else if (currentWeapon.GetFiringMode() == Weapon.FiringMode.Automatic)
                {
                    //check if the user is holding left mouse click on the weapon
                    if (autoFireTimer <= 0 && Input.GetMouseButton(0))
                    {
                        currentWeapon.Shoot();
                        autoFireTimer = currentWeapon.GetTimeBetweenShooting();
                    }
                }
            }
        }
    }
    private void HandleReload()
    {
        Weapon currentWeapon = mainController.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if(mainController.GetCurrentWeapon().GetIsReloading() == false && mainController.GetCurrentWeapon().GetBulletsLeft() != mainController.GetCurrentWeapon().GetMagazineSize())
                {   
                    currentWeapon.Reload();
                    animator.SetBool("AimRifle",false);
                    animator.SetBool("Aim",false);
                    starterAssetsInputs.aim = false;
                    if ((mainController.GetCurrentWeapon().name == "pistol" || mainController.GetCurrentWeapon().name == "revolver") && mainController.GetCurrentWeapon().GetIsReloading() == true) 
                    {
                        animator.SetLayerWeight(8,1f);
                    }                    
                    animator.SetTrigger("PerformReload");

                    starterAssetsInputs.reload = false;
                }
            }
        }
    }
}
