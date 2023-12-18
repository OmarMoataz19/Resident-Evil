using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip clickEffect;
    public AudioClip hoverEffect;
    public AudioSource audioSourceEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickSound() {
        audioSourceEffect.PlayOneShot(clickEffect);
    }

    public void onHoverSound() {
        audioSourceEffect.PlayOneShot(hoverEffect);
    }
}

