using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemPickup : MonoBehaviour
{
    public InventoryItem Item;
    public TextMeshProUGUI UI;
    void Pickup()
    {
        if (InventoryManager.Instance.Add(Item))
        {
            if (Item.ItemType.Equals(InventoryItemType.Key))
            {
                Inventory.Instance.audioSource.PlayOneShot(Inventory.Instance.keyAudioClip);
            }
            else 
            {
                Inventory.Instance.audioSource.PlayOneShot(Inventory.Instance.itemsAudioClip);
            }
            Destroy(gameObject);
        }
        else
        {
            Inventory.Instance.audioSource.PlayOneShot(Inventory.Instance.invalidAudioClip);
        }
        InventoryManager.Instance.ListItems();
    }
   
    private void OnTriggerStay(Collider other)
    {
        //var hanafy = UI.GetComponent<TMPro.TextMeshPro>();
        if (other.CompareTag("Player"))
        {
            UI.text = "Press E to Pickup " + Item.itemName;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
                UI.text="";
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.text="";
        }
    }



    

}
