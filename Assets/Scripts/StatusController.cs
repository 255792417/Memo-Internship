using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float maxHealth = 100f;

    public float currentOxygen;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOxygen();
    }

    void UpdateOxygen()
    {
        // 20 ������ÿ������ 1 ������
        int depth = (int)((-0.4f - transform.position.y) / 0.65);
        if (depth > 20)
        {
            currentOxygen -= (depth - 20) * Time.deltaTime;
        }
        // �������ÿ��ظ� 20 ����
        else if (depth < 5 && currentOxygen < maxOxygen)
        {
            currentOxygen += 20 * Time.deltaTime;
        }

        // �����ľ���Ѫ
        if (currentOxygen <= 0)
        {
            TakeDamage(10f * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ����ֵ��������
    void Die()
    {

    }
}
