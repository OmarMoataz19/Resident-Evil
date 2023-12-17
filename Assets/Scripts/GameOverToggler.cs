using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverToggler : MonoBehaviour
{
    // Start is called before the first frame update
    public MainController mainController ;
    void Start()
    {
        
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            mainController.won = true;
        }

    }
}
