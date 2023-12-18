using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class LoadingScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private AsyncOperation asyncLoad;
    public Button skipButton;

    private void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(2);
        asyncLoad.allowSceneActivation = false; 

        videoPlayer.loopPointReached += EndReached; 
        videoPlayer.Play();
        skipButton.gameObject.SetActive(false);
        StartCoroutine(EnableSkipButtonWhenReady());
    }

    void EndReached(VideoPlayer vp)
        {
            ActivateScene();
        }

    public void OnSkipButtonPressed() 
    {
        ActivateScene();
    }

    IEnumerator EnableSkipButtonWhenReady()
    {
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(25.0f);
        skipButton.gameObject.SetActive(true); 
    }

    private void ActivateScene()
    {
        asyncLoad.allowSceneActivation = true;
    }

}
