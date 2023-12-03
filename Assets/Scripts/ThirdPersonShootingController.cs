using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
public class ThirdPersonShootingController : MonoBehaviour
{
    public CinemachineVirtualCamera aimVirtualCamera;
    public LayerMask aimColliderLayerMask = new LayerMask();
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
  
    //variables for controlling ads
    public float normalSensitivity = 1f;
    public float aimSensitivity = 0.5f;
    // Start is called before the first frame update

    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        //calculate where leon is pointing 
        Vector2 screenCenterPostition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPostition);

        Transform targetHit = null; //will be used with hitscan..
        if(Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask)){
            mouseWorldPosition = hit.point;
            targetHit = hit.transform; //can check to decrease hp of the zombie etc.
        }

        if(starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);

            //disable the rotation from the camera..
            thirdPersonController.SetRotateOnMove(false);

            //rotate leon to face where he is aiming
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f ); //experimental value 20..

            //change the layers weight taking the index..
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f)); //lerp to make it smooth..
        }
        else{
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);

            //enable the rotation from the camera..
            thirdPersonController.SetRotateOnMove(true);

            //change the layers weight taking the index..
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f)); //lerp to make it smooth..
        }

        if(starterAssetsInputs.shoot)
        {
            //todo: add the logic to check if holding gun etc && sufficient ammo..
            // check for the logic of automatic and semi automatic here as well..

            //shoot the gun
            Debug.Log("Shoot");
            //check if we hit something
            if(targetHit != null)
            {
                //check if we hit a zombie
                //ZombieController zombieController = targetHit.GetComponent<ZombieController>();
                // if(zombieController != null)
                // {
                //     //we hit a zombie
                //     //todo: add the logic to check which weapon is being used and decrease the hp accordingly..
                //     zombieController.TakeDamage(10f);
                // }
            }
            starterAssetsInputs.shoot = false;
        }
    }
}
