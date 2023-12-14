using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnterShop : MonoBehaviour
{
    public Collider storeCollider;
    public TextMeshProUGUI textMeshProUGUI;

    public GameObject Shop;
    public GameObject Inventory;
    public InventoryManager invManager;
    public bool shopActive = false;
    public Inventory inventory;
    public GameObject HealthPanel;
    public ShopManager shopManager;
    void Start()
    {
        
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Shop.SetActive(false);
                Inventory.SetActive(false);
                invManager.shopOpened = false;
                shopActive = false;
                invManager.ShowStorage = false;
                HealthPanel.SetActive(false);
                RefreshShop();
            }
            Cursor.lockState = shopActive || inventory.inventoryActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = shopActive || inventory.inventoryActive ? 0 : 1;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textMeshProUGUI.text = "Press E to Open Store";
            if(Input.GetKeyDown(KeyCode.E))
            {
                textMeshProUGUI.text = "";
                Shop.SetActive(true);
                Inventory.SetActive(true);
                invManager.ResetInventoryFlags();
                invManager.ListItems();
                invManager.shopOpened = true;
                shopActive = true;
                HealthPanel.SetActive(true);
            }
  
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textMeshProUGUI.text = "";
        }
    }
    public void RefreshShop()
    {
        shopManager.HideAllTabsAndPanels();
        shopManager.ShowBuyableItems();
        shopManager.LoadPanels();
        shopManager.CheckPurchasable();
        shopManager.CheckSellable();
    }
}
