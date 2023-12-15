using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHud : MonoBehaviour
{
    public CanvasGroup redScreenCanvasGroup;
    public float fadeDuration = 4f; 
    void Start()
    {
        if (redScreenCanvasGroup != null)
        {
            redScreenCanvasGroup.alpha = 0;
        }
    }

    public void TakeDamage()
    {
        if (redScreenCanvasGroup != null)
        {
            StartCoroutine(FadeOutRedScreen());
        }
    }

    private IEnumerator FadeOutRedScreen()
    {
        redScreenCanvasGroup.alpha = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            redScreenCanvasGroup.alpha = 0.5f - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        redScreenCanvasGroup.alpha = 0;
    }

}
