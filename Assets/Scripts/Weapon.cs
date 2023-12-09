using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Weapon : MonoBehaviour
{
    //enums
    public enum Range
    {
        Short,
        Medium,
        Long
    }
    public enum FiringMode
    {
        SingleShot,
        Automatic
    }

    //graphics
    public Animator animator;
    public Transform attackPoint;
    public TextMeshPro ammoText;
    public GameObject muzzleFlash , bulletHoleGraphic;
    //colliders
    public LayerMask aimColliderLayerMask; //to determine the crosshair collision
    public LayerMask whatIsEnemy; //to determine what is a zombie

    //weapon stats 
    protected int damage;
    protected float timeBetweenShooting;
    protected Range weaponRange;
    protected float reloadTime;
    protected int magazineSize;
    protected int bulletsLeft;
    protected FiringMode firingMode;

    //control variables
    protected bool isReloading , isReadyToShoot;

    public virtual void Shoot()
    {
        isReadyToShoot = true;
        if (bulletsLeft > 0 && !isReloading && isReadyToShoot)
        {
            animator.SetTrigger("Shoot");
            isReadyToShoot = false;
            Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999f, aimColliderLayerMask))
            {
                //Debug.Log(hit.collider.name);
                HandleHit(hit);
            }

            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            bulletsLeft--;
            UpdateHUD();
            Invoke(nameof(ResetShot), timeBetweenShooting);
        }
    }
    public virtual void UpdateHUD()
    {
        if (ammoText != null)
        {
            ammoText.text = ""+bulletsLeft;
        }
    }
     private void ResetShot()
    {
        isReadyToShoot = true;
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        UpdateHUD();
        isReloading = false;
    }
    protected virtual void HandleHit(RaycastHit hit)
    {
        // Apply damage to hit object, if it's a zombie
    }

    public virtual void Reload()
    {
        if (bulletsLeft == magazineSize || isReloading)
        {
            return;
        }
        //todo: check if there is enough ammo from inventory..
        isReloading = true;
        animator.SetTrigger("Reload");
        Invoke(nameof(ReloadFinished), reloadTime);
    }
    protected bool IsTargetInRange(RaycastHit hit)
    {
        float hitDistance = Vector3.Distance(hit.point, attackPoint.position);
        // range limits for Short, Medium, and Long ranges , those are experimental values
        float maxRange = weaponRange == Range.Short ? 3f : 
                        weaponRange == Range.Medium ? 7f : 
                        10f;
        return hitDistance <= maxRange;
    }
    protected void DealDamage(RaycastHit hit, int weaponDamage)
    {
        // Check if the hit object is an enemy
        if (hit.collider.gameObject.layer == 7)
        {
            float hitDistance = Vector3.Distance(hit.point, attackPoint.position);
            print(hitDistance);
            // Deal damage to the enemy
            hit.collider.gameObject.GetComponent<ZombieMain>().GetHit(weaponDamage);
        }
    }

    //will be used inside of the ThirdPersonShootingController..
    public FiringMode GetFiringMode()
    {
        return firingMode;
    }

    public float GetTimeBetweenShooting()
    {
        return timeBetweenShooting;
    }

    //disable the weapon hud
    public void DisableWeaponHUD()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(false);
        }
    }
    //show the weapon hud
    public void EnableWeaponHUD()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(true);
        }
    }
    public bool GetIsReloading()
    {
        return isReloading;
    }
    public int GetBulletsLeft()
    {
        return bulletsLeft;
    }
    public int GetMagazineSize()
    {
        return magazineSize;
    }
}
