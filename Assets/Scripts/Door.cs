using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Door : MonoBehaviour
{
    bool trig;
    public float smooth = 2.0f;
    public float DoorOpenAngle = 90.0f;
    private Vector3 defaulRot;
    private Vector3 openRot;
    public TextMeshProUGUI txt;
    private bool opening = false; 
    private bool opened = false;

    void Start()
    {
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);
    }

    void Update()
    {
        if (trig && Input.GetKeyDown(KeyCode.E) && !opening)
        {
            opening = true; 
        }

        if (opening)
        {

            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);

 
            if (Vector3.Distance(transform.eulerAngles, openRot) < 0.1f)
            {
                transform.eulerAngles = openRot; 
                opening = false;
                opened = true;
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
            txt.text = opening || opened ? "" : "Press E To Open Door";
            trig = true;
        }
    }
}
