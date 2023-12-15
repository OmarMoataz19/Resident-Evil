using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cheats : MonoBehaviour
{
    public MainController mainController;
    public Door [] doors;
    public bool isSlowMotion = false; 
    public bool isInvincible = false;
    void Update()
    {
        if(mainController.GetCheats())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Heal();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                isInvincible = !isInvincible;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isSlowMotion = !isSlowMotion;
                slowMotion();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddGold();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                OpenAllDoors(); 
            }
        }

    }

    public void Heal()
    {
        int newHp = mainController.GetHp();
        newHp += 4;
        newHp = Math.Min(8,newHp);
        mainController.SetHp(newHp);
    }
    public void slowMotion()
    {
        Time.timeScale = isSlowMotion? 0.5f : 1.0f;
    }
    public void resetMotion ()
    {
        Time.timeScale = 1f;
    }
    public void AddGold()
    {
        int newGold = mainController.GetGold();
        newGold += 1000;
        mainController.SetGold(newGold);
    }
    public void OpenAllDoors ()
    {
        foreach (var door in doors)
        {
            // door.canOpen = true;
            door.SetOpening();
        }
    }

}
