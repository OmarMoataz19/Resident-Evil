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

    public AudioSource pauseSource;

    public AudioSource dieSource7;

    public MainController mainController;
    public EnterShop enterShop;
    public Inventory inventory;
    
    private bool playedDeathSound;
    void Start()
    {
        //take from the player prefs the sound effects and the bg music volume
        float musicVolume = PlayerPrefs.GetFloat("bgMusicVolume");
        float soundEffectsVolume = PlayerPrefs.GetFloat("soundEffectsVolume");

        pauseSource.volume = musicVolume;
        audioSource6.volume = musicVolume;

        audioSource.volume = soundEffectsVolume;
        audioSource2.volume = soundEffectsVolume;
        audioSource3.volume = soundEffectsVolume;
        audioSource4.volume = soundEffectsVolume;
        audioSource5.volume = soundEffectsVolume;
        dieSource7.volume = soundEffectsVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainController.isPaused || enterShop.shopActive || inventory.inventoryActive || mainController.won || mainController.lost)
        {
            audioSource.Pause();
            audioSource2.Pause();
            audioSource3.Pause();
            //audioSource4.Pause();
            audioSource5.Pause();
            audioSource6.Pause();
            pauseSource.UnPause();
        }
        else
        {
            audioSource.UnPause();
            audioSource2.UnPause();
            audioSource3.UnPause();
            audioSource4.UnPause();
            audioSource5.UnPause();
            audioSource6.UnPause();
            pauseSource.Pause();
        }
        if(mainController.lost && !playedDeathSound)
        {
            dieSource7.Play();
            playedDeathSound = true;
        }
    }

     void LateUpdate() {
                if(mainController.isPaused || enterShop.shopActive || inventory.inventoryActive || mainController.won || mainController.lost)
        {
            audioSource.Pause();
            audioSource2.Pause();
            audioSource3.Pause();
            //audioSource4.Pause();
            audioSource5.Pause();
            audioSource6.Pause();
            pauseSource.UnPause();
        }
    }
}
