using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public GameObject[] hpSegments; // Array to hold HP bar segments
    public Material highHpMaterial;
    public Material mediumHpMaterial;
    public Material lowHpMaterial;
    public Material defaultHpMaterial;

    private int maxHp;
    private int currentHp;

    void Start()
    {
        maxHp = 8; 
        SetHp(maxHp);
    }

    public void SetHp(int hp)
    {
        currentHp = hp;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < maxHp; i++)
        {
            if (i < currentHp)
            {
                hpSegments[i].SetActive(true); // Activate segment
                ChangeSegmentColor(hpSegments[i]);
            }
            else
            {
                //change to the default material
                hpSegments[i].GetComponent<Renderer>().material = defaultHpMaterial;
                // hpSegments[i].SetActive(false); // Deactivate segment
            }
        }
    }

    private void ChangeSegmentColor(GameObject segment)
    {
        Material materialToUse;

        if (currentHp > maxHp * 0.66) // More than 66% HP
        {
            materialToUse = highHpMaterial;
        }
        else if (currentHp > maxHp * 0.33) // Between 33% and 66% HP
        {
            materialToUse = mediumHpMaterial;
        }
        else // Less than 33% HP
        {
            materialToUse = lowHpMaterial;
        }

        Renderer segmentRenderer = segment.GetComponent<Renderer>();
        if (segmentRenderer != null)
        {
            segmentRenderer.material = materialToUse;
        }
    }
}