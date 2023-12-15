using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    //public InventoryItem purchasedItem;    
    public InventoryItem[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;
    public Button[] mySellBtns;
    public GameObject tabHerbs;
    public GameObject tabWeapons;
    public GameObject tabAmmo;
    public GameObject tabGunPowder;
    public GameObject tabGrenades;
    public GameObject tabMixtures;
    public GameObject tabTreasures;

    public GameObject tabBuyItems;
    public GameObject tabSellItems;
    public GameObject tabStorage;
    public GameObject tabKnifeRepair;

    public Button knifeRepair;
    public TextMeshProUGUI durabilityTxt;

    public bool GGMixFlag = false;
    public bool GRMixFlag = false;

    public MainController mainController;

    public List<Sprite> shopSprites = new List<Sprite>();

    private bool purchasedShotgun = false;
    private bool purchasedRifle = false;


    // Start is called before the first frame update
    void Start()
    {
        ShowBuyableItems();
        LoadPanels();
        CheckPurchasable();
        CheckSellable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSellable();
        CheckPurchasable();
        SetBuySellBtnColor();
        CheckMixtures();

        if (mainController.GetGold() >= 100 && mainController.GetCurrentDurability()<10)
        {
            knifeRepair.interactable = true;
        }
        else
        {
            knifeRepair.interactable = false;
        }
    }

    public void SetBuySellBtnColor()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (myPurchaseBtns[i].interactable == true)
            {
                myPurchaseBtns[i].GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                myPurchaseBtns[i].GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
            }

            if (mySellBtns[i].interactable == true)
            {
                mySellBtns[i].GetComponent<Button>().image.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                mySellBtns[i].GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
            }

        }
    }

    public void AddGold()
    {
        mainController.SetGold(mainController.GetGold() + 200);
        CheckPurchasable();
    }

    public void CheckPurchasable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (mainController.GetGold() >= shopItemsSO[i].baseCost && shopItemsSO[i].baseCost != 0 )
            {
                myPurchaseBtns[i].interactable = true;
                if ((purchasedShotgun && shopItemsSO[i].itemName.Equals("Shotgun")) || (purchasedRifle && shopItemsSO[i].itemName.Equals("AK-14")))
                {
                    myPurchaseBtns[i].interactable = false;
                }
            }
            else
                myPurchaseBtns[i].interactable = false;

            if (shopItemsSO[i].baseCost == 0)
            {
                myPurchaseBtns[i].gameObject.SetActive(false);
            }
        }

    }

    public void PurchaseItem(int btnNo)
    {
        if (mainController.GetGold() >= shopItemsSO[btnNo].baseCost)
        {

            if (shopItemsSO[btnNo].ItemType == InventoryItemType.Grenade)
            {
                GrenadeItem grenades = ScriptableObject.CreateInstance<GrenadeItem>();
                grenades.id = shopItemsSO[btnNo].id;
                grenades.itemName = shopItemsSO[btnNo].itemName;
                grenades.ItemType = InventoryItemType.Grenade;
                grenades.icon = shopItemsSO[btnNo].icon;
                if (shopItemsSO[btnNo].itemName.Equals("Hand Grenade"))
                {
                    grenades.GrenadeType = GrenadeType.Hand;
                }
                if (shopItemsSO[btnNo].itemName.Equals("Flash Grenade"))
                {
                    grenades.GrenadeType = GrenadeType.Flash;
                }

                var inventoryCheck = inventoryManager.Add(grenades);
                if (inventoryCheck)
                {
                   mainController.SetGold(mainController.GetGold()-shopItemsSO[btnNo].baseCost);
                }
                else
                {
                    Debug.Log("You cannot purchase");
                }
            }


            if (shopItemsSO[btnNo].ItemType == InventoryItemType.GunPowder)
            {
                GunPowderItem gunPowders = ScriptableObject.CreateInstance<GunPowderItem>();
                gunPowders.id = shopItemsSO[btnNo].id;
                gunPowders.itemName = shopItemsSO[btnNo].itemName;
                gunPowders.ItemType = InventoryItemType.GunPowder;
                gunPowders.icon = shopItemsSO[btnNo].icon;
                if (shopItemsSO[btnNo].itemName.Equals("Normal Gun Powder"))
                {
                    gunPowders.GunPowderType = GunPowderType.Normal;
                }
                if (shopItemsSO[btnNo].itemName.Equals("HG Gun Powder"))
                {
                    gunPowders.GunPowderType = GunPowderType.HighGrade;
                }

                var inventoryCheck = inventoryManager.Add(gunPowders);
                if (inventoryCheck)
                {
                   mainController.SetGold(mainController.GetGold() - shopItemsSO[btnNo].baseCost);
                }
                else
                {
                    Debug.Log("You cannot purchase");
                }
            }

            if (shopItemsSO[btnNo].ItemType == InventoryItemType.Herbs)
            {
                HerbItem herbs = ScriptableObject.CreateInstance<HerbItem>();
                herbs.id = shopItemsSO[btnNo].id;
                herbs.itemName = shopItemsSO[btnNo].itemName;
                herbs.ItemType = InventoryItemType.Herbs;
                herbs.icon = shopItemsSO[btnNo].icon;
                if (shopItemsSO[btnNo].itemName.Equals("Green Herb"))
                {
                    herbs.HerbType = HerbType.Green;
                }
                if (shopItemsSO[btnNo].itemName.Equals("Red Herb"))
                {
                    herbs.HerbType = HerbType.Red;
                }

                var inventoryCheck = inventoryManager.Add(herbs);
                if (inventoryCheck)
                {
                    mainController.SetGold(mainController.GetGold() - shopItemsSO[btnNo].baseCost);
                }
                else
                {
                    Debug.Log("You cannot purchase");
                }
            }

            if (shopItemsSO[btnNo].ItemType == InventoryItemType.Weapon)
            {
                WeaponItem weapons = ScriptableObject.CreateInstance<WeaponItem>();
                weapons.id = shopItemsSO[btnNo].id;
                weapons.itemName = shopItemsSO[btnNo].itemName;
                weapons.ItemType = InventoryItemType.Weapon;
                weapons.icon = shopItemsSO[btnNo].icon;
                if (shopItemsSO[btnNo].itemName.Equals("AK-14"))
                {
                    weapons.AmmoType = AmmoType.AssaultRifle;
                    weapons.WeaponType = WeaponType.AssaultRifle;
                    weapons.Ammo = 30;
                    purchasedRifle = true;
                }
                if (shopItemsSO[btnNo].itemName.Equals("Shotgun"))
                {
                    weapons.AmmoType = AmmoType.ShotGun;
                    weapons.WeaponType = WeaponType.ShotGun;
                    weapons.Ammo = 8;
                    purchasedShotgun = true;
                }

                var inventoryCheck = inventoryManager.Add(weapons);
                if (inventoryCheck)
                {
                    mainController.SetGold(mainController.GetGold() - shopItemsSO[btnNo].baseCost);
                }
                else
                {
                    Debug.Log("You cannot purchase");
                }
            }

            if (shopItemsSO[btnNo].ItemType == InventoryItemType.Ammo)
            {
                Debug.Log(shopItemsSO[btnNo].id);
                AmmoItem ammos = ScriptableObject.CreateInstance<AmmoItem>();
                ammos.id = shopItemsSO[btnNo].id;
                ammos.itemName = shopItemsSO[btnNo].itemName;
                ammos.icon = shopItemsSO[btnNo].icon;
                ammos.ItemType = InventoryItemType.Ammo;
                if (shopItemsSO[btnNo].itemName.Equals("Pistol Ammo"))
                {
                    ammos.AmmoType = AmmoType.Pistol;
                    ammos.Amount = 12;
                }
                if (shopItemsSO[btnNo].itemName.Equals("Shotgun Ammo"))
                {
                    ammos.AmmoType = AmmoType.ShotGun;
                    ammos.Amount = 8;
                }
                if (shopItemsSO[btnNo].itemName.Equals("AR Ammo"))
                {
                    ammos.AmmoType = AmmoType.AssaultRifle;
                    ammos.Amount = 30;
                }
                if (shopItemsSO[btnNo].itemName.Equals("Revolver Ammo"))
                {
                    ammos.AmmoType = AmmoType.Revolver;
                    ammos.Amount = 6;
                }


                var inventoryCheck = inventoryManager.AddAmmoToInventory(ammos);
                if (inventoryCheck)
                {
                    mainController.SetGold(mainController.GetGold() - shopItemsSO[btnNo].baseCost);
                }
                else
                {
                    Debug.Log("You cannot purchase");
                }

            }

            CheckPurchasable();
        }

    }

    public void CheckSellable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            mySellBtns[i].interactable = false;
        }
        foreach (var item in inventoryManager.Items)
        {
            for (int i = 0; i < shopItemsSO.Length; i++)
            {
                if (shopItemsSO[i].sellPrice > 0)
                {
                    if (shopItemsSO[i].id == item.id)
                    {
                        mySellBtns[i].interactable = true;
                    }
                }
            }
        }
        

    }

    public void SellItem(int btnNo)
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            mySellBtns[i].interactable = false;
        }
        Debug.Log("Sell Pressed");
        mainController.SetGold(mainController.GetGold() + shopItemsSO[btnNo].sellPrice);
        inventoryManager.Sell(shopItemsSO[btnNo].id);
    } 

    public void LoadPanels()
    {
        for(int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsSO[i].itemName;
            shopPanels[i].costTxt.text = "Gold: " + shopItemsSO[i].baseCost.ToString();
            shopPanels[i].sellTxt.text = "Gold: " + shopItemsSO[i].sellPrice.ToString();

            var Images = shopPanels[i].transform.Find("icon").GetComponent<Image>();
            Images.sprite = shopSprites[i];
            
        }
    }


    public void HideAll()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(false);
        }

        SetTabColor(tabHerbs, false);
        SetTabColor(tabWeapons, false);
        SetTabColor(tabAmmo, false);
        SetTabColor(tabGunPowder, false);
        SetTabColor(tabGrenades, false);
        SetTabColor(tabMixtures, false);
        SetTabColor(tabTreasures, false);

    }

    public void SetTabColor(GameObject tab, bool isActive)
    {
        if (tab != null && tab.GetComponent<Button>() != null)
        {
            tab.GetComponent<Button>().image.color = isActive ? new Color32(212, 212, 212, 255) : new Color32(255, 255, 255, 255);
        }
    }

    public void ShowHerbs()
    {
        HideAll();
        SetTabColor(tabHerbs, true);
        for (int i = 0; i < 2 ; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }

    }

    public void ShowWeapons()
    {
        HideAll();
        SetTabColor(tabWeapons, true);
        for (int i = 2; i < 4; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowAmmo()
    {
        HideAll();
        SetTabColor(tabAmmo, true);
        for (int i = 4; i < 8; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowGunPowder()
    {
        HideAll();
        SetTabColor(tabGunPowder, true);
        for (int i = 8; i < 10; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowGrenades()
    {
        HideAll();
        SetTabColor(tabGrenades, true);
        for (int i = 10; i < 12; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowMixtures()
    {
        HideAll();
        SetTabColor(tabMixtures, true);
        for (int i = 12; i < 14; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowTreasures()
    {
        HideAll();
        SetTabColor(tabTreasures, true);
        for (int i = 14; i < 17; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
    }

    public void ShowBuyableItems()
    {
        foreach(var sells in mySellBtns)
        {
            sells.gameObject.SetActive(false);
        }
        foreach (var buys in myPurchaseBtns)
        {
            buys.gameObject.SetActive(true);
        }
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].sellTxt.gameObject.SetActive(false);
        }
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].costTxt.gameObject.SetActive(true);
        }
        ShowWeapons();
        tabMixtures.SetActive(false);
        tabTreasures.SetActive(false);
        tabWeapons.SetActive(true);
        tabAmmo.SetActive(true);
        tabGunPowder.SetActive(true);
        tabGrenades.SetActive(true);
        tabHerbs.SetActive(true);
        //shopPanelsGO[17].SetActive(false);
        Transform contentTransform = transform.Find("Scroll View/Viewport/Content");

        Transform itemTemplateTransform = contentTransform.GetChild(17);
        GameObject itemTemplateObject = itemTemplateTransform.gameObject;
        itemTemplateObject.SetActive(false);

        Transform itemTemplateTransform2 = contentTransform.GetChild(18);
        GameObject itemTemplateObject2 = itemTemplateTransform2.gameObject;
        itemTemplateObject2.SetActive(false);

        SetTabColor(tabBuyItems, true);
        SetTabColor(tabSellItems, false);
        SetTabColor(tabStorage, false);
        SetTabColor(tabKnifeRepair, false);

        inventoryManager.ShowStorage = false;


    }

    public void ShowSellableItems()
    {
        foreach (var buys in myPurchaseBtns)
        {
            buys.gameObject.SetActive(false);
        }
        foreach (var sells in mySellBtns)
        {
            sells.gameObject.SetActive(true);
        }
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].sellTxt.gameObject.SetActive(true);
        }
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].costTxt.gameObject.SetActive(false);
        }
        ShowHerbs();
        tabWeapons.SetActive(false);
        tabAmmo.SetActive(false);
        tabMixtures.SetActive(true);
        tabTreasures.SetActive(true);
        tabGunPowder.SetActive(true);
        tabGrenades.SetActive(true);
        tabHerbs.SetActive(true);
        //shopPanelsGO[17].SetActive(false);
        Transform contentTransform = transform.Find("Scroll View/Viewport/Content");

        Transform itemTemplateTransform = contentTransform.GetChild(17);
        GameObject itemTemplateObject = itemTemplateTransform.gameObject;
        itemTemplateObject.SetActive(false);

        Transform itemTemplateTransform2 = contentTransform.GetChild(18);
        GameObject itemTemplateObject2 = itemTemplateTransform2.gameObject;
        itemTemplateObject2.SetActive(false);

        SetTabColor(tabSellItems, true);
        SetTabColor(tabBuyItems, false);
        SetTabColor(tabStorage, false);
        SetTabColor(tabKnifeRepair, false);

        inventoryManager.ShowStorage = false;


    }

    public void ShowStorage()
    {
        HideAllTabsAndPanels();

        SetTabColor(tabStorage, true);
        SetTabColor(tabSellItems, false);
        SetTabColor(tabBuyItems, false);
        SetTabColor(tabKnifeRepair, false);
        //shopPanelsGO[17].SetActive(false);
        Transform contentTransform = transform.Find("Scroll View/Viewport/Content");

        Transform itemTemplateTransform = contentTransform.GetChild(17);
        GameObject itemTemplateObject = itemTemplateTransform.gameObject;
        itemTemplateObject.SetActive(false);

        Transform itemTemplateTransform2 = contentTransform.GetChild(18);
        GameObject itemTemplateObject2 = itemTemplateTransform2.gameObject;
        itemTemplateObject2.SetActive(true);

        inventoryManager.ShowStorage = true;


    }

    public void ShowKnifeRepair()
    {

        HideAllTabsAndPanels();
        durabilityTxt.text = "Durability: " + mainController.GetCurrentDurability().ToString();
        //shopPanelsGO[17].SetActive(true);
        SetTabColor(tabKnifeRepair, true);
        SetTabColor(tabStorage, false);
        SetTabColor(tabSellItems, false);
        SetTabColor(tabBuyItems, false);


        Transform contentTransform = transform.Find("Scroll View/Viewport/Content");

        Transform itemTemplateTransform = contentTransform.GetChild(17);
        GameObject itemTemplateObject = itemTemplateTransform.gameObject;
        itemTemplateObject.SetActive(true);

        Transform itemTemplateTransform2 = contentTransform.GetChild(18);
        GameObject itemTemplateObject2 = itemTemplateTransform2.gameObject;
        itemTemplateObject2.SetActive(false);

        inventoryManager.ShowStorage = false;


    }

    public void Repair()
    { 
            mainController.SetKnifeDurability(10);
            mainController.SetGold(mainController.GetGold() - 100);
            durabilityTxt.text = "Durability: " + mainController.GetCurrentDurability().ToString();     
    }

        public void HideAllTabsAndPanels()
    {
        tabHerbs.SetActive(false);
        tabWeapons.SetActive(false);
        tabAmmo.SetActive(false);
        tabGunPowder.SetActive(false);
        tabGrenades.SetActive(false);
        tabMixtures.SetActive(false);
        tabTreasures.SetActive(false);

        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(false);
        }
    }



    public void CheckMixtures()
    {
        if (GGMixFlag)
        {
            mySellBtns[12].interactable = true;
        }
        else
        {
            mySellBtns[12].interactable = false;
        }

        if (GRMixFlag)
        {
            mySellBtns[13].interactable = true;
        }
        else
        {
            mySellBtns[13].interactable = false;
        }
    }
}
