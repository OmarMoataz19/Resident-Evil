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
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            animator.SetBool("Aim", false);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
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
