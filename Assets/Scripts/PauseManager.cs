using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PauseCanvas; 
    public bool pauseCanvasActive = false;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    public MainController mainController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseCanvasActive)
        {
            pauseCanvasActive = true;
            mainController.isPaused = true;

            PauseCanvas.SetActive(pauseCanvasActive);
            starterAssetsInputs.LookInput(new Vector2(0f,0f));
            starterAssetsInputs.canLook = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseCanvasActive)
        {
            PauseCanvas.SetActive(false);
            pauseCanvasActive = false;
            starterAssetsInputs.canLook = true;
            mainController.isPaused = false;
        }
    }
    public void ResumeClickHandler ()
    {
        pauseCanvasActive = false;
        Time.timeScale = 1;
        PauseCanvas.SetActive(false);
    }

    public void RestartClickHandler()
    {
        Time.timeScale = 1;
        PauseCanvas.SetActive(false);
        Application.LoadLevel("Demo");
    }
    public void MainMenuClickHandler()
    {
        Time.timeScale = 1;
        PauseCanvas.SetActive(false);
        Application.LoadLevel("MainMenu");
    }
}
