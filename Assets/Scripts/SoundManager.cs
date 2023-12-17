using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;
    public AudioSource audioSource6;

    public MainController mainController;
    public EnterShop enterShop;
    public Inventory inventory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mainController.isPaused || enterShop.shopActive || inventory.inventoryActive || mainController.won || mainController.lost)
        {
            audioSource.Pause();
            audioSource2.Pause();
            audioSource3.Pause();
            audioSource4.Pause();
            audioSource5.Pause();
            audioSource6.Pause();
        }
        else
        {
            audioSource.UnPause();
            audioSource2.UnPause();
            audioSource3.UnPause();
            audioSource4.UnPause();
            audioSource5.UnPause();
            audioSource6.UnPause();
        }
    }
}
