using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject GameoverDeadCanvas;
    public GameObject GameoverWinCanvas; 
    public MainController mainController;

    public void GameOver()
    {
        if (mainController.won)
        {
            GameoverWinCanvas.SetActive(true);
        }
        else if (mainController.lost )
        {
           StartCoroutine(showGameLost());
        }
    }
    public void RestartClickHandler()
    {
        Time.timeScale = 1;
        GameoverDeadCanvas.SetActive(false);
        GameoverWinCanvas.SetActive(false);
        SceneManager.LoadScene(2);
    }
    public void MainMenuClickHandler()
    {
        Time.timeScale = 1;
        GameoverDeadCanvas.SetActive(false);
        GameoverWinCanvas.SetActive(false);
        SceneManager.LoadScene(0);
    }
    IEnumerator showGameLost(){
        yield return new WaitForSeconds(2); 
        Time.timeScale = 0;
        GameoverDeadCanvas.SetActive(true);

      
    }
}
