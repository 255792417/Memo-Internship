using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image hpImg;
    public Image hpEffectImg;

    public float maxHealth = 100f; // 最大生命值
    public float currentHealth = 100f; // 当前生命值
    public float healthEffectTime = 0.5f; // 生命缓降效果时间

    private Coroutine updateCoroutine;
    
    private void Start()
    {
        UpdateHealthDisplay();
    }

    // 更改生命值
    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    // 增加生命值
    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    // 减少生命值
    public void ReduceHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth - value, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    // 更新生命值显示
    private void UpdateHealthDisplay()
    {
        float healthPercentage = currentHealth / maxHealth;
        hpImg.fillAmount = healthPercentage;

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        // 启动协程
        updateCoroutine = StartCoroutine(UpdateHealthEffect(healthPercentage));
    }

    // 生命缓降效果实现
    private IEnumerator UpdateHealthEffect(float healthPercentage)
    {
        float prePercentage = hpEffectImg.fillAmount;
        float timer = 0f;
        while (timer < healthEffectTime)
        {
            timer += Time.deltaTime;
            hpEffectImg.fillAmount = Mathf.Lerp(prePercentage, healthPercentage, timer / healthEffectTime);
            yield return null;
        }
        hpEffectImg.fillAmount = healthPercentage;
        updateCoroutine = null;
    }
}
