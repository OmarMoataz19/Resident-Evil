using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDrawer : MonoBehaviour
{
    bool trig;
    public float smooth = 2.0f;
    public float DoorOpenAngle = -90.0f; // Target rotation in degrees
    private Quaternion defaulRot;
    private Quaternion openRot;
    public TextMeshProUGUI txt;
    private bool opening = false; 
    private bool opened = false;

    public GameObject colliderToBeEnabled;

    void Start()
    {
        defaulRot = transform.rotation; // Store the default rotation
        openRot = Quaternion.Euler(defaulRot.eulerAngles.x, defaulRot.eulerAngles.y + DoorOpenAngle, defaulRot.eulerAngles.z); // Calculate the open rotation
    }

    void Update()
    {
        if (trig && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            opening = true;
        }

        if (opening)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * smooth); // Rotate smoothly towards the open rotation

            if (Quaternion.Angle(transform.rotation, openRot) < 1.0f) // Check if the rotation is close enough to the target
            {
                transform.rotation = openRot; // Snap to final rotation
                opening = false;
                opened = true; // Prevent further opening
                colliderToBeEnabled.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            txt.text = "";
            trig = false;
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player")
        {
            txt.text = opened ? "" : "Press E To Open Cabinet";
            trig = true;
        }
    }
}
