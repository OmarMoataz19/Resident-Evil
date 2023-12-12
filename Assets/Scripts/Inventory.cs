using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager invManager;
    public GameObject inventory;
    public bool inventoryActive = false;


    // array of weapons currently with leon
    // array of keys currently with leon
    // each weapon will be a class with an ammo attribute..
    // key will be an enum with a name attribute
    // array of herbs, each herb could be an enum

    // grenades will be an enum with a name attribute
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryActive = !inventoryActive;

            inventory.SetActive(inventoryActive);

            invManager.ResetInventoryFlags();
            invManager.ListItems();
            
            Cursor.lockState = inventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = inventoryActive ? 0 : 1;
        }
    }
}
