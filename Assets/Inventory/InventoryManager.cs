using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> Items = new List<InventoryItem>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public EquippedPanelController equippedPanelController;

    // Inventory control buttons.
    public GameObject DiscardButton;
    public GameObject UseButton;
    public GameObject EquipButton;
    public GameObject CraftButton;


    // Inventory control flags.
    private int selectedItemIndex = -1;


    // Equipped variables.
    private InventoryItem equippedWeapon = null;


    // Crafting variables.
    private bool isCraftModeOn = false;
    private InventoryItem firstIngredient = null;
    public List<Sprite> craftedSprites = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        WeaponItem pistol = ScriptableObject.CreateInstance<WeaponItem>();
        pistol.id = 14;
        pistol.itemName = "Pistol";
        pistol.ItemType = InventoryItemType.Weapon;
        pistol.icon = craftedSprites[5];
        pistol.WeaponType = WeaponType.Pistol;
        pistol.Ammo = 12;

        AmmoItem pistolAmmo = ScriptableObject.CreateInstance<AmmoItem>();
        pistolAmmo.id = 15;
        pistolAmmo.itemName = "Pistol Ammo";
        pistolAmmo.icon = craftedSprites[2];
        pistolAmmo.ItemType = InventoryItemType.Ammo;
        pistolAmmo.AmmoType = AmmoType.Pistol;
        pistolAmmo.Amount = 12;

        HerbItem greenHerb = ScriptableObject.CreateInstance<HerbItem>();
        greenHerb.id = 7;
        greenHerb.itemName = "Green Herb";
        greenHerb.icon = craftedSprites[6];
        greenHerb.ItemType = InventoryItemType.Herbs;
        greenHerb.HerbType = HerbType.Green;

        Items.Add(pistol);
        Items.Add(pistolAmmo);
        Items.Add(greenHerb);

        equippedPanelController.EquipWeapon(pistol);
        equippedWeapon = pistol;
    }

    public void SelectItem(string itemIndex)
    {

        int index;
        if (int.TryParse(itemIndex, out index))
        {
            Debug.Log(index);
            selectedItemIndex = index;
            InventoryItem selectedItem = Items[selectedItemIndex];
            if (isCraftModeOn)
            {
                if(selectedItemIndex == Items.IndexOf(firstIngredient))
                {
                    // TODO: SHOW NOT ALLOWED
                    Debug.Log("Can't craft with the same item");
                    return;
                }
                if (firstIngredient is HerbItem)
                {
                    // check second is the same..
                    if(selectedItem is HerbItem)
                    {
                        var result = CraftPotion((firstIngredient as HerbItem),
                                                    (selectedItem as HerbItem));
                        if(result != null)
                        {
                            Items.Remove(firstIngredient);
                            Items.Remove(selectedItem);
                            Items.Add(result);
                        }
                    }
                    else
                    {
                        // TODO: Indicate that this is invalid operation.
                    }
                }
                else if (firstIngredient is GunPowderItem)
                {
                    // check second is the same..
                    if(selectedItem is GunPowderItem)
                    {
                        var result = CraftAmmo((firstIngredient as GunPowderItem), 
                                                (selectedItem as GunPowderItem));
                        Items.Remove(firstIngredient);
                        Items.Remove(selectedItem);
                        // check If you already have ammo and increment..
                        if (!AddAmmoToInventory(result))
                        {
                            Items.Add(result);
                        }
                    }
                    else
                    {
                        // TODO: Indicate that this is invalid operation.
                    }
                }
                else
                {
                    // Invalid Selected item..
                    // TODO: Indicate that this is invalid..
                }
                ResetInventoryFlags();
                ListItems();
            }
            else
            {
                ListButtons(selectedItem);
            }
        }
        
    }

    public bool AddAmmoToInventory(AmmoItem ammo)
    {
        foreach (var item in Items)
        {
            if( item is AmmoItem && (item as AmmoItem).AmmoType == ammo.AmmoType)
            {
                (item as AmmoItem).Amount += ammo.Amount;
                return true;
            }
        }
        return false;
    }

    public MixtureItem CraftPotion(HerbItem firstHerb, HerbItem secondHerb)
    {
        if(firstHerb.HerbType == HerbType.Green)
        {
            var resultPotion = ScriptableObject.CreateInstance<MixtureItem>();
            if (secondHerb.HerbType == HerbType.Green)
            {
                
                resultPotion.id = 6;
                resultPotion.itemName = "Green Green Mixture";
                resultPotion.icon = craftedSprites[0];
                resultPotion.MixtureType = MixtureType.GreenGreen;
                resultPotion.ItemType = InventoryItemType.Mixture;                
            }
            else
            {
                resultPotion.id = 8;
                resultPotion.itemName = "Green Red Mixture";
                resultPotion.icon = craftedSprites[1];
                resultPotion.MixtureType = MixtureType.GreenRed;
                resultPotion.ItemType = InventoryItemType.Mixture;
            }
            return resultPotion;
        }
        else
        {
            var resultPotion = ScriptableObject.CreateInstance<MixtureItem>();
            if (secondHerb.HerbType == HerbType.Green)
            {
                resultPotion.id = 8;
                resultPotion.itemName = "Green Red Mixture";
                resultPotion.icon = craftedSprites[1];
                resultPotion.MixtureType = MixtureType.GreenRed;
                resultPotion.ItemType = InventoryItemType.Mixture;
            }
            else
            {
                //TODO: Indicate that this is not allowed.
                return null;
            }
            return resultPotion;
        }
    }

    public AmmoItem CraftAmmo(GunPowderItem firstGunPowder, GunPowderItem secondGunPowder)
    {
        var resultAmmo = ScriptableObject.CreateInstance<AmmoItem>();
        if(firstGunPowder.GunPowderType == GunPowderType.Normal)
        {
            if(secondGunPowder.GunPowderType == GunPowderType.Normal)
            {
                // 12 Pistol Ammo
                resultAmmo.id = 15;
                resultAmmo.itemName = "Pistol Ammo";
                resultAmmo.icon = craftedSprites[2];
                resultAmmo.ItemType = InventoryItemType.Ammo;
                resultAmmo.AmmoType = AmmoType.Pistol;
                resultAmmo.Amount = 12;
                
            }
            else
            {
                // 8 Shotgun Ammo
                resultAmmo.id = 21;
                resultAmmo.itemName = "Shotgun Ammo";
                resultAmmo.icon = craftedSprites[3];
                resultAmmo.ItemType = InventoryItemType.Ammo;
                resultAmmo.AmmoType = AmmoType.ShotGun;
                resultAmmo.Amount = 8;
            }
        }
        else
        {
            if(secondGunPowder.GunPowderType == GunPowderType.HighGrade)
            {
                // 30 Assault Rifle Ammo
                resultAmmo.id = 1;
                resultAmmo.itemName = "AR Ammo";
                resultAmmo.icon = craftedSprites[4];
                resultAmmo.ItemType = InventoryItemType.Ammo;
                resultAmmo.AmmoType = AmmoType.AssaultRifle;
                resultAmmo.Amount = 30;
            }
            else
            {
                // 8 Shotgun Ammo
                resultAmmo.id = 21;
                resultAmmo.itemName = "Shotgun Ammo";
                resultAmmo.icon = craftedSprites[3];
                resultAmmo.ItemType = InventoryItemType.Ammo;
                resultAmmo.AmmoType = AmmoType.ShotGun;
                resultAmmo.Amount = 8;
            }
        }
        return resultAmmo;
    }

    public bool Add(InventoryItem item)
    {
        if(Items.Count >= 6)
        {
            // Can't pickup item
            // TODO: Indicate that.
            Debug.Log("Can't take the item");
            return false;
        }
        Items.Add(item);
        return true;
    }

    public void Remove()
    {
        if (selectedItemIndex == -1) return;

        Items.RemoveAt(selectedItemIndex);
        ListItems();
        selectedItemIndex = -1;
        DiscardButton.SetActive(false);
    }

    public void ListItems()
    {
        for (int i = 0; i < ItemContent.transform.childCount; i++)
        {
            var currentSlot = ItemContent.transform.GetChild(i).gameObject;
            GameObject slot = currentSlot.gameObject;
            var itemName = slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var ammoCount = slot.transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
            var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.gameObject.SetActive(false);
            itemIcon.gameObject.SetActive(false);
            ammoCount.gameObject.SetActive(false);
        }

        foreach (var item in Items)
        {
            for (int i = 0; i < ItemContent.transform.childCount; i++)
            {
                var currentSlot = ItemContent.transform.GetChild(i);
                if (!currentSlot.transform.Find("ItemIcon")
                    .GetComponent<Image>()
                    .IsActive())
                {
                    GameObject slot = currentSlot.gameObject;
                    var itemIndex = slot.transform.Find("ItemIndex").GetComponent<TextMeshProUGUI>();
                    var itemName = slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
                    var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();

                    itemIndex.text = i.ToString();
                    itemName.text = item.itemName;
                    itemIcon.sprite = item.icon;
                    
                 
                    itemName.gameObject.SetActive(true);
                    itemIcon.gameObject.SetActive(true);

                    if (item is AmmoItem)
                    {
                        var ammoCountText = slot.transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
                        ammoCountText.text = $"x{(item as AmmoItem).Amount}";
                        ammoCountText.gameObject.SetActive(true);
                    }

                    break;
                }
            }
            
            
        }
    }

    public void ListButtons(InventoryItem item)
    {
        DisableButtons();

        if (selectedItemIndex == -1) return;

        List<GameObject> buttonsToDraw = new List<GameObject>();

        if (IsEquipable(item) && !isCraftModeOn)
        {
            buttonsToDraw.Add(EquipButton);
        }

        if (IsUsable(item) && !isCraftModeOn)
        {
            buttonsToDraw.Add(UseButton);
        }

        if (IsCraftable(item) && !isCraftModeOn)
        {
            buttonsToDraw.Add(CraftButton);
        }

        if (!isCraftModeOn && (item is WeaponItem && !(item as WeaponItem).Equals(equippedWeapon)))
        {
            buttonsToDraw.Add(DiscardButton);
        }
        
        foreach (var btn in buttonsToDraw)
        {
            btn.SetActive(true);
        }

    }


    // Button Logic..

    public void OnEquipClicked()
    {
        if(selectedItemIndex == -1) return;

        InventoryItem item = Items[selectedItemIndex];
        
        if (item is WeaponItem)
        {
            equippedPanelController.EquipWeapon((item as WeaponItem));
            equippedWeapon = item;
            DisableButtons();
            return;
        }

        if(item is GrenadeItem)
        {
            equippedPanelController.EquipGrenade((item as GrenadeItem));
            return;
        }
    }

    public void OnUseClicked()
    {
        if(selectedItemIndex == -1) return;

        // TODO: Implement the logic
        InventoryItem selectedItem = Items[selectedItemIndex];

        Debug.Log(selectedItem.GetType());
        if(selectedItem is HerbItem)
        {
            Debug.Log("Green herb here");
            Debug.Log((selectedItem as HerbItem).HerbType);
            if ( (selectedItem as HerbItem).HerbType == HerbType.Green)
            {
                Debug.Log("Using Green herb");
            }
        }
        else if (selectedItem is MixtureItem)
        {
            if( (selectedItem as MixtureItem).MixtureType == MixtureType.GreenGreen )
            {
                Debug.Log("Using Green potion");
            } else if ( (selectedItem as MixtureItem).MixtureType == MixtureType.GreenRed )
            {
                Debug.Log("Using GreenRed potion");
            }
        }

        // Once finished remove the item from the list of items.
        Items.RemoveAt(selectedItemIndex);
        selectedItemIndex = -1;
        ListItems();
        DisableButtons();
    }

    public void OnCraftClicked()
    {
        isCraftModeOn = true;
        firstIngredient = Items[selectedItemIndex];
        selectedItemIndex = -1;
        DisableButtons();
    }
         
    public void ResetInventoryFlags()
    {
        selectedItemIndex = -1;
        isCraftModeOn = false;
    }

   
    public void DisableButtons()
    {
        EquipButton.SetActive(false);
        UseButton.SetActive(false);
        CraftButton.SetActive(false);
        DiscardButton.SetActive(false);
    }

    // Utilities..
    private bool IsUsable(InventoryItem item)
    {
        return ((item is HerbItem) && ((item as HerbItem).HerbType == HerbType.Green)) 
                || (item is MixtureItem);
    }

    private bool IsEquipable(InventoryItem item)
    {
        return (item is GrenadeItem) || (item is WeaponItem && !(item as WeaponItem).Equals(equippedWeapon));
    }

    private bool IsCraftable(InventoryItem item)
    {
        return (item is HerbItem) || (item is GunPowderItem);
    }

}
