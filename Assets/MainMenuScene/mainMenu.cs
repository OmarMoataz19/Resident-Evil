using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mainMenu : MonoBehaviour
{

    public GameObject optionPanel;
    public AudioSource musicSource;
    public AudioSource effectSource;
    public Slider volumeSlider;
    public Slider effectsSlider;
    
    public Button teamCredit;
    public Button assetCredit;

    public Button backAssetCredit;
    public Button backTeamCredit;

    public Button closeButton;
    public Button optionsButton;
    public GameObject AssetsCredits;
    public GameObject TeamCredits;
    public GameObject mainControls;

    void Start()
    {
        //check for the playerprefs here ..
        //if there is no player prefs then set the default values
        if (!PlayerPrefs.HasKey("bgMusicVolume"))
        {
            PlayerPrefs.SetFloat("bgMusicVolume", 1);
            SetMusicVolume(1);
            volumeSlider.value = 1;
        }
        else
        {
            float musicVolume = PlayerPrefs.GetFloat("bgMusicVolume");
            SetMusicVolume(musicVolume);
            volumeSlider.value = musicVolume;
        }

        if (!PlayerPrefs.HasKey("soundEffectsVolume"))
        {
            PlayerPrefs.SetFloat("soundEffectsVolume", 1);
            effectSource.volume = 1;
            effectsSlider.value = 1;
        }
        else
        {
            float soundEffectsVolume = PlayerPrefs.GetFloat("soundEffectsVolume");
            effectSource.volume = soundEffectsVolume;
            effectsSlider.value = soundEffectsVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame () {
#if UNITY_EDITOR
        // If running in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }

    public void openOptions (GameObject options ) {
        options.SetActive( true );
        mainControls.SetActive(true);

        //Reset all buttons
        ResetButtonProperties(closeButton);
        ResetButtonProperties(assetCredit);
        ResetButtonProperties(teamCredit);
        ResetButtonProperties(backAssetCredit);
        ResetButtonProperties(backTeamCredit);

        //ResetButtonProperties(optionsButton);
    }
    public void closeOptions (GameObject options)
    {   
       TeamCredits.SetActive(false);
        AssetsCredits.SetActive(false);

        options.SetActive( false );
        //Reset all buttons
        ResetButtonProperties(closeButton);
        ResetButtonProperties(assetCredit);
        ResetButtonProperties(teamCredit);
        ResetButtonProperties(backAssetCredit);
        ResetButtonProperties(backTeamCredit);
    }

    public void openCredit(GameObject credit)
    {
        optionPanel.SetActive(false);
        credit.SetActive(true);
        //Reset all buttons
        ResetButtonProperties(closeButton);
        ResetButtonProperties(assetCredit);
        ResetButtonProperties(teamCredit);
        ResetButtonProperties(backAssetCredit);
        ResetButtonProperties(backTeamCredit);
        
    }

    public void closeCredit(GameObject credit)
    {
        optionPanel.SetActive(true);
        credit.SetActive(false);
        if (credit.name =="AssetsCredits") {
            ResetButtonProperties(backAssetCredit);
        }
        else 
        {
            ResetButtonProperties(backTeamCredit);
        }
    }
    public void playGame () {
        PlayerPrefs.SetFloat("bgMusicVolume", volumeSlider.value);
        PlayerPrefs.SetFloat("soundEffectsVolume", effectsSlider.value);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Demo");
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volumeSlider.value;
    }  
    public void SetSoundEffectsVolume(float volume)
    {
        effectSource.volume = effectsSlider.value;
    }
      public void ResetButtonProperties(Button button)
    {
        if (button == null)
        {
            Debug.LogError("Button is not assigned");
            return;
        }
        foreach (Transform child in button.transform)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                image.color = new Color(1, 1, 1, 1);
                continue;
            }

            // Check for TMP_Text component (for texts)
            TMP_Text text = child.GetComponent<TMP_Text>();
            if (text != null)
            {
                if (child.name == "Hover")
                {
                    text.color = new Color(0, 0, 0, 1);
                }
                else if (child.name == "original")
                {
                    text.color = new Color(1, 1, 1, 0);
                }
            }
        }
    }
}