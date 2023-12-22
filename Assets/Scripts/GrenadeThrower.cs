using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private Rigidbody Grenade;
    [SerializeField]
    private LineRenderer LineRenderer;
    [SerializeField]
    private Transform ReleasePosition;
    [Header("Grenade Controls")]
    [SerializeField]
    [Range(1, 100)]
    private float ThrowStrength = 10f;
    [SerializeField]
    [Range(1, 10)]
    private float ExplosionDelay = 5f;
    [SerializeField]
    private GameObject ExplosionParticleSystem;
    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;

    private Transform InitialParent;
    private Vector3 InitialLocalPosition;
    private Quaternion InitialRotation;

    private bool IsGrenadeThrowAvailable = true;
    private LayerMask GrenadeCollisionMask;
    public MainController mainController;
    public LeonAnimationController leonAnimationController;
    public InventoryManager inventoryManager;
    private void Awake()
    {
        InitialParent = Grenade.transform.parent;
        InitialRotation = Grenade.transform.localRotation;
        InitialLocalPosition = Grenade.transform.localPosition;
        Grenade.freezeRotation = true;

        int grenadeLayer = Grenade.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                GrenadeCollisionMask |= 1 << i; 
            }
        }
    }

    private void Update()
    {
        if(mainController.GetCurrentGrenade() != null)
        {
           Grenade = mainController.GetCurrentGrenade().GetComponent<Rigidbody>();
        }
        else
        {
            Animator.SetTrigger("Rage3");
            Animator.ResetTrigger("Throw Grenade");
            Animator.ResetTrigger("Hold Grenade");
        }
        if (Application.isFocused && Input.GetKey(KeyCode.G) && mainController.GetCurrentGrenade() != null && Time.timeScale !=0f)
        {
            if(!leonAnimationController.isGrappled)
            {
            Animator.transform.rotation = Quaternion.Euler(
                Animator.transform.eulerAngles.x,
                Camera.transform.rotation.eulerAngles.y,
                Animator.transform.eulerAngles.z
            );
            DrawProjection();
            Animator.ResetTrigger("Rage3");
            Animator.SetTrigger("Hold Grenade");
            mainController.HideWeapons();
            }
            else{
            Animator.SetTrigger("Rage3");
            Animator.ResetTrigger("Throw Grenade");
            Animator.ResetTrigger("Hold Grenade");
            LineRenderer.enabled = false;
            }
        }
        else
        {
            LineRenderer.enabled = false;
        }
        
        if (Input.GetKeyUp(KeyCode.G) && IsGrenadeThrowAvailable && mainController.GetCurrentGrenade() != null && !leonAnimationController.isGrappled )
        {
            IsGrenadeThrowAvailable = false;
            Animator.SetTrigger("Throw Grenade");
            Animator.ResetTrigger("Hold Grenade");
            Animator.ResetTrigger("Rage3");
            mainController.ShowWeapon();
            // ReleaseGrenade();
        }
    }

    private void DrawProjection()
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = ReleasePosition.position;
        Vector3 startVelocity = ThrowStrength * Camera.transform.forward / Grenade.mass;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            LineRenderer.SetPosition(i, point);

            Vector3 lastPosition = LineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, 
                (point - lastPosition).normalized, 
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                GrenadeCollisionMask))
            {
                LineRenderer.SetPosition(i, hit.point);
                LineRenderer.positionCount = i + 1;
                return;
            }
        }
    }

    private void ReleaseGrenade()
    {
        if(mainController.GetCurrentGrenade() == null)
        {
            LineRenderer.enabled = false;
            Animator.ResetTrigger("Hold Grenade");
            Animator.ResetTrigger("Rage3");
            mainController.ShowWeapon();
            IsGrenadeThrowAvailable = true;
            return;
        }
        Grenade.velocity = Vector3.zero;
        Grenade.angularVelocity = Vector3.zero;
        Grenade.isKinematic = false;
        Grenade.freezeRotation = false;
        Grenade.transform.SetParent(null, true);
        if(leonAnimationController.isGrappled)
        {
            Grenade.AddForce(Vector3.up * 4f, ForceMode.Impulse);
        }
        else
        {
            Grenade.AddForce(Camera.transform.forward * ThrowStrength, ForceMode.Impulse);
        }

        inventoryManager.RemoveCurrentEquippedGrenade();
        mainController.GetCurrentGrenade().equipIndex = -1;
        StartCoroutine(ExplodeGrenade());
    }

    private IEnumerator ExplodeGrenade()
    {
        yield return new WaitForSeconds(ExplosionDelay);

        //Instantiate(ExplosionParticleSystem, Grenade.transform.position, Quaternion.identity);
        mainController.GetCurrentGrenade().Explode();
        //mainController.HideGrenade();
        Grenade.GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse(new Vector3(Random.Range(-1, 1), Random.Range(0.5f, 1), Random.Range(-1, 1)));

        Grenade.freezeRotation = true;
        Grenade.isKinematic = true;
        Grenade.transform.SetParent(InitialParent, false);
        Grenade.rotation = InitialRotation;
        Grenade.transform.localPosition = InitialLocalPosition;
        IsGrenadeThrowAvailable = true;
    }
}
