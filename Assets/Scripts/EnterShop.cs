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
    public Cheats cheats;
    public GameObject bg;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    public MainController mainController;
    void Start()
    {
        
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape) && shopActive)
            {
                Shop.SetActive(false);
                Inventory.SetActive(false);
                invManager.shopOpened = false;
                shopActive = false;
                invManager.ShowStorage = false;
                HealthPanel.SetActive(false);
                bg.SetActive(false);
                RefreshShop();
                starterAssetsInputs.canLook = true;
            }
            Cursor.lockState = shopActive || inventory.inventoryActive || mainController.isPaused || mainController.won || mainController.lost ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = shopActive || inventory.inventoryActive || mainController.isPaused || mainController.won || mainController.lost ? 0 : cheats.isSlowMotion ? 0.5f : 1.0f;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textMeshProUGUI.text = inventory.inventoryActive? "": "Press E to Open Store";
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
                bg.SetActive(true);
                starterAssetsInputs.LookInput(new Vector2(0f,0f));
                starterAssetsInputs.canLook = false;
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
