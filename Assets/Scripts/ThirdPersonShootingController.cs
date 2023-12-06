using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
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
    public Transform debugTransform;
    private Coroutine autoFireCoroutine; // Coroutine for automatic firing


    // private fields for the logic 
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Transform transform;
    private Animator animator;  
        
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        transform =  GetComponent<Transform>();
    }

    void Update()
    {
        HandleAiming(starterAssetsInputs.aim);

        if (starterAssetsInputs.aim)
        {
            HandleShooting();
        }
    }

    private void HandleAiming(bool isAiming)
    {
        aimVirtualCamera.gameObject.SetActive(isAiming);
        thirdPersonController.SetSensitivity(isAiming ? aimSensitivity : normalSensitivity);
        thirdPersonController.SetRotateOnMove(!isAiming);

        if (isAiming)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            RotateTowardsTarget(mouseWorldPosition);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            animator.SetTrigger("Aim");

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
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            animator.SetBool("Aim", false);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPosition = new Vector2((Screen.width  / 2f) + 240 , (Screen.height  / 2f) - 20);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
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
                else if (currentWeapon.GetFiringMode() == Weapon.FiringMode.Automatic && autoFireCoroutine == null)
                {
                    autoFireCoroutine = StartCoroutine(AutoFire(currentWeapon));
                }
            }
            else if (currentWeapon.GetFiringMode() == Weapon.FiringMode.Automatic && autoFireCoroutine != null)
            {
                StopCoroutine(autoFireCoroutine);
                autoFireCoroutine = null;
            }
        }
    }

    private IEnumerator AutoFire(Weapon weapon)
    {
        while (true)
        {
            weapon.Shoot();
            yield return new WaitForSeconds(weapon.GetTimeBetweenShooting());
        }
    }
}
