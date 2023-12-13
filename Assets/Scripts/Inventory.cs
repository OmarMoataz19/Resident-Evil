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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !invManager.shopOpened )
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
