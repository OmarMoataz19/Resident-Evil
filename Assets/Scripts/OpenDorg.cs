using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OpenDorg : MonoBehaviour
{
    public TextMeshProUGUI txt;
    private bool opened = false;
    public GameObject colliderToBeEnabled;
    public Animator animator;
    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            txt.text = "";
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player")
        {
        if (Input.GetKeyDown(KeyCode.E) && !opened)
        {
            animator.SetTrigger("DorgOpen");
            opened = true;
            Invoke("SetColliderToTrue",0.6f);
            Inventory.Instance.audioSource4.PlayOneShot(Inventory.Instance.dorgSound);
        }
            txt.text = opened ? "" : "Press E To Open Drawer";
        }

    }
    public void SetColliderToTrue ()
    {
        colliderToBeEnabled.GetComponent<BoxCollider>().enabled = true;
    }
}
