using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsPickUp : MonoBehaviour
{
    public TextMeshProUGUI UI;
    public GameObject Canva;
    public int amount =0;
    public MainController mainController;

    // Start is called before the first frame update
    void Start()
    {
        Canva = GameObject.FindWithTag("PickUp");
        mainController = GameObject.FindWithTag("Game Logic").GetComponent<MainController>();
        UI = Canva.GetComponent<TextMeshProUGUI>();
        amount = (Random.Range(5,51));
    }

    void Pickup()
    {
            int x = mainController.GetGold();
            mainController.SetGold(x + amount);
            Inventory.Instance.audioSource.PlayOneShot(Inventory.Instance.coinsAudioClip);
            Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && (UI.text == "Press E to Pickup \n" + amount + " coins" || UI.text == "")  && !mainController.tookItem)
        {
            UI.text = "Press E to Pickup \n" + amount + " coins";
            if (Input.GetKeyDown(KeyCode.E))
            {
                UI.text="";
                mainController.tookItem = true;
                Pickup();
                
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.text="";
            //
        }
    }


}
