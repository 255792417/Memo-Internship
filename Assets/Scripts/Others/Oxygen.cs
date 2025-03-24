using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public Image oxygenImg;
    public Image lowOxygenImg;
    public Image pointer;

    public float maxOxygen = 100f;
    public float currentOxygen = 100f;
    public float threshold = 50f;

    public void SetOxygen(float newOxygen)
    {
        currentOxygen = Mathf.Clamp(newOxygen, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    public void AddOxygen(float value)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + value, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    public void ReduceOxygen(float value)
    {
        currentOxygen = Mathf.Clamp(currentOxygen - value, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    private void UpdateOxygenDisplay()
    {
        float oxygenPercentage = currentOxygen / maxOxygen;
        
        if (currentOxygen < threshold)
        {
            oxygenImg.enabled = false;
            lowOxygenImg.enabled = true;
            lowOxygenImg.fillAmount = oxygenPercentage;
        }
        else
        {
            oxygenImg.enabled = true;
            lowOxygenImg.enabled = false;
            oxygenImg.fillAmount = oxygenPercentage;
        }

        pointer.transform.localEulerAngles = new Vector3(0f, 0f, 156f - 180f * (1f - oxygenPercentage));
    }
}
