using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    private Animator animator;
    //Gun stats
    private int damage;
    private float timeBetweenShooting, range, reloadTime; //range??
    private int magazineSize, bulletsPerTap;
    private int bulletsLeft, bulletsShot;
    //bools 
    private bool readyToShoot, reloading;

    //Reference
    public Transform attackPoint;
    //public RaycastHit rayHit;
    public LayerMask whatIsEnemy; //Mask for the enemy

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    public LayerMask aimColliderLayerMask = new LayerMask();
    private void Awake()
    {
        bulletsLeft = 12;
        readyToShoot = true;
    }

    // start is called before the first frame update
    private void Start()
    {
        damage = 2;
        magazineSize = 12;
        timeBetweenShooting = 0.2f;
        bulletsPerTap = 1;
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        if(bulletsLeft > 0 && readyToShoot && !reloading) 
        {
            animator.SetTrigger("Shoot");
            bulletsShot = bulletsPerTap;
            readyToShoot = false;
            Vector2 screenCenterPostition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPostition);
            Vector3 mouseWorldPosition = Vector3.zero;
            Transform targetHit = null; //will be used with hitscan..
            if(Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask)){
                Debug.Log(hit.collider.name);
                mouseWorldPosition = hit.point;
                targetHit = hit.transform; 
                //can check to decrease hp of the zombie etc.
            }

            //ShakeCamera
           //camShake.Shake(camShakeDuration, camShakeMagnitude);

            //Graphics
            //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            bulletsLeft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting); 

        }
        // if(bulletsShot > 0 && bulletsLeft > 0)
        //Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    public void Reload()
    {
        if(bulletsLeft < magazineSize && !reloading) // to prevent reloading when full
        reloading = true;
        animator.SetTrigger("Reload");
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
