using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    // Start is called before the first frame update
    public InventoryManager invManager;
    public GameObject inventory;
    public bool inventoryActive = false;
    public GameObject HealthPanel;
    public TextMeshProUGUI textMeshProUGUI;
    public Cheats cheats;
    public GameObject bg;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;
    public AudioClip itemsAudioClip;
    public AudioClip keyAudioClip;
    public AudioClip invalidAudioClip;
    public AudioClip coinsAudioClip;

    // leon damage taken
    public AudioClip leonDamage;
    public AudioClip leonDamage2;
    public AudioClip leonDies;

    // zombie take damage
    public AudioClip zombieDamage;
    public AudioClip zombieDamage2;
    public AudioClip zombieDamage3;
    public AudioClip zombieDies;

    //zombie deal damage
    public AudioClip zombieAttack;
    public AudioClip zombieAttack2;
    public AudioClip zombieAttack3;

    //dorg
    public AudioClip dorgSound;
    public AudioClip buySound;

    public AudioClip knifeStab;

    public AudioClip zombieWalk;
    public AudioClip zombieGrowl;
    public AudioClip allZombieSounds;

    // Update is called once per frame
    private void Awake()
    {
        Instance = this;
    }
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
