using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image hpImg;
    public Image hpEffectImg;

    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float healthEffectTime = 0.5f;

    private Coroutine updateCoroutine;
    
    private void Start()
    {
        UpdateHealthDisplay();
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    public void ReduceHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth - value, 0f, maxHealth);
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        float healthPercentage = currentHealth / maxHealth;
        hpImg.fillAmount = healthPercentage;

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHealthEffect(healthPercentage));
    }

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
