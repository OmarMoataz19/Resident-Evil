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
            // addAMOUNT();
            int x = mainController.GetGold();
            mainController.SetGold(x + amount);
            Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        //var hanafy = UI.GetComponent<TMPro.TextMeshPro>();
        if (other.CompareTag("Player"))
        {
            UI.text = "Press E to Pickup \n" + amount + " coins";
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
                UI.text="";
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.text="";
        }
    }


}
