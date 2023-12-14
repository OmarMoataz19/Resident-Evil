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
    public GameObject HealthPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !invManager.shopOpened )
        {
            inventoryActive = true;

            inventory.SetActive(inventoryActive);
            HealthPanel.SetActive(true);

            invManager.ResetInventoryFlags();
            invManager.ListItems();
            
            Cursor.lockState = inventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = inventoryActive ? 0 : 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryActive)
        {
            inventoryActive = false;
            HealthPanel.SetActive(false);
            inventory.SetActive(inventoryActive);
            Cursor.lockState = inventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = inventoryActive ? 0 : 1;
        }
    }
}
