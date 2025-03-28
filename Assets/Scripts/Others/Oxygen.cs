using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public Image oxygenImg;
    public Image lowOxygenImg;
    public Image pointer;

    public float maxOxygen = 100f; // 最大氧气值
    public float currentOxygen = 100f; // 当前氧气值
    public float threshold = 50f; // 氧气过低阈值

    // 更改氧气值
    public void SetOxygen(float newOxygen)
    {
        currentOxygen = Mathf.Clamp(newOxygen, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    // 增加氧气值
    public void AddOxygen(float value)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + value, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    // 减少氧气值
    public void ReduceOxygen(float value)
    {
        currentOxygen = Mathf.Clamp(currentOxygen - value, 0f, maxOxygen);
        UpdateOxygenDisplay();
    }

    // 更新氧气值显示
    private void UpdateOxygenDisplay()
    {
        float oxygenPercentage = currentOxygen / maxOxygen;
        
        // 氧气值低于阈值，切换低氧气值图片
        if (currentOxygen < threshold)
        {
            oxygenImg.enabled = false;
            lowOxygenImg.enabled = true;
            lowOxygenImg.fillAmount = oxygenPercentage;
        }
        // 否则使用正常图片
        else
        {
            oxygenImg.enabled = true;
            lowOxygenImg.enabled = false;
            oxygenImg.fillAmount = oxygenPercentage;
        }

        // 旋转指针
        pointer.transform.localEulerAngles = new Vector3(0f, 0f, 156f - 180f * (1f - oxygenPercentage));
    }
}
