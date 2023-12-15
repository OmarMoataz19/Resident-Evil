using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager invManager;
    public GameObject inventory;
    public bool inventoryActive = false;
    public GameObject HealthPanel;
    public TextMeshProUGUI textMeshProUGUI;
    public Cheats cheats;
    public GameObject bg;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;
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
            textMeshProUGUI.text = "";
            bg.SetActive(true);
            starterAssetsInputs.LookInput(new Vector2(0f,0f));
            starterAssetsInputs.canLook = false;
            Time.timeScale = inventoryActive ? 0 : cheats.isSlowMotion? 0.5f : 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryActive)
        {
            inventoryActive = false;
            HealthPanel.SetActive(false);
            inventory.SetActive(inventoryActive);
            Cursor.lockState = inventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            bg.SetActive(false);  
            starterAssetsInputs.canLook = true;
            Time.timeScale = inventoryActive ? 0 : 1;
        }
    }
}
