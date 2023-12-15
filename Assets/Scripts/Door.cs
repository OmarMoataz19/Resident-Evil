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
    private BoxCollider[] boxColliders;

    public bool canOpen;
    public InventoryManager inventoryManager;
    void Start()
    {
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);
        boxColliders = GetComponents<BoxCollider>();
    }

    void Update()
    {
        if (trig && Input.GetKeyDown(KeyCode.E) && !opening && canOpen)
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
                foreach (var collider in boxColliders)
                {
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }

        if(!canOpen)
        {
            if (trig && Input.GetKeyDown(KeyCode.E) && !opening)
            {
                for(int i = 0; i < inventoryManager.Items.Count; i++)
                {
                    if(inventoryManager.Items[i].itemName == "Heart Key" && this.gameObject.tag == "Heart")
                    {
                        inventoryManager.Items.RemoveAt(i);
                        canOpen = true;
                        opening = true;
                        break;
                    }
                    else if (inventoryManager.Items[i].itemName == "Key Card" && this.gameObject.tag == "KeyCard")
                    {
                        inventoryManager.Items.RemoveAt(i);
                        canOpen = true;
                        opening = true;
                        break;
                    }
                    else if (inventoryManager.Items[i].itemName == "Spade Key" && this.gameObject.tag == "Spade")
                    {
                        inventoryManager.Items.RemoveAt(i);
                        canOpen = true;
                        opening = true;
                        break;
                    }
                    else if (inventoryManager.Items[i].itemName == "Club Key" && this.gameObject.tag == "Clubs")
                    {
                        inventoryManager.Items.RemoveAt(i);
                        canOpen = true;
                        opening = true;
                        break;
                    }
                    else if (inventoryManager.Items[i].itemName == "Emblem" && this.gameObject.tag == "Emblem")
                    {
                        inventoryManager.Items.RemoveAt(i);
                        canOpen = true;
                        opening = true;
                        break;
                    }
                }
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
        if (coll.tag == "Player" && canOpen)
        {
            txt.text = opening || opened ? "" : "Press E To Open Door";
            trig = true;
        }
        else if (coll.tag == "Player" && !canOpen)
        {
            HandleLockedRooms(coll);
            trig = true;
        }
    }

    private void HandleLockedRooms(Collider collider)
    {
        if (collider.tag == "Player" )
        {
            string doorTag = this.gameObject.tag;

            switch (doorTag)
            {
                case "Heart": txt.text = "You need a Hearts Key to open this door"; break; // Spades door
                case "KeyCard": txt.text = "You need a Key Card to open this door"; break; // revolver door
                case "Spade": txt.text = "You need a Spades Key to open this door"; break; // clubs door 
                case "Clubs" : txt.text = "You need an Clubs to open this door"; break; //Emblem door
                case "Emblem": txt.text = "You need an Emblem to open this door"; break; // exit door
            }

        }

    }
    public void SetOpening()
    {
        opening = true;
    }

}
