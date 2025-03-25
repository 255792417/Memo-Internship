using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class StatusController : MonoBehaviour, IDamagable
{
    public AudioManager audioManager;

    public ScoreDisplay scoreDisplay;
    public Health health;
    public Oxygen oxygen;

    public Animator anim;
    public Rigidbody2D rb;

    [Header("Base Stats")]
    public float maxOxygen = 100f; // �������ֵ
    public float maxHealth = 100f; // �������ֵ

    [Header("Current Stats")]
    public float currentOxygen; // ��ǰ����ֵ
    public float currentHealth; // ��ǰ����ֵ
    public bool isDead = false; // �Ƿ�����

    [Header("Score")]
    public int score = 0; // ����

    [Header("Fall Damage")]
    public float minFallVelocity = -0f;  // ��ʼ����˺�����С�����ٶ�
    public float maxFallVelocity = -20f;  // �������˺��������ٶ�
    public int minDamage = 5;             // ��С�˺�ֵ
    public int maxDamage = 50;            // ����˺�ֵ
    public float fallDamageMultiplier = 1.0f; // �˺�����
    private float fallVelocity;           // �����ٶ�

    [Header("Flash")]
    public float flashDuration = 1f;    // ��˸����ʱ��
    public float flashInterval = 0.1f;    // ��˸���

    public Renderer[] renderers;
    private bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;

        // ��ʼ������ֵ��ʾ
        health.maxHealth = maxHealth;
        health.SetHealth(currentHealth);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        UpdateOxygen();
        fallVelocity = rb.velocity.y;
    }

    void UpdateOxygen()
    {
        // 20 ������ÿ������ 1 ������
        int depth = (int)((-0.4f - transform.position.y) / 0.65);
        if (depth > 20)
        {
            ReduceOxygen((depth - 20) * Time.deltaTime);
        }
        // �������ÿ��ظ� 20 ������
        else if (depth < 5)
        {
            AddOxygen(20 * Time.deltaTime);
        }
    }

    public void  AddOxygen(float value)
    {
        currentOxygen += value;
        currentOxygen = Mathf.Min(currentOxygen, maxOxygen);
        oxygen.SetOxygen(currentOxygen);
    }

    public void ReduceOxygen(float value)
    {
        currentOxygen -= value;
        if(currentOxygen <= 0)
        {
            currentOxygen = 0;
            TakeDamage(10f * Time.deltaTime);
        }
        oxygen.SetOxygen(currentOxygen);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (fallVelocity < minFallVelocity)
        {
            ApplyFallDamage(fallVelocity);
        }
    }

    void ApplyFallDamage(float velocity)
    {
        velocity = Mathf.Min(velocity, 0);
        velocity = Mathf.Max(velocity, maxFallVelocity);

        // �����˺�ֵ
        float damagePercent = Mathf.InverseLerp(minFallVelocity, maxFallVelocity, velocity);
        int damage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, damagePercent) * fallDamageMultiplier);

        if (damage > maxDamage)
        {
            damage = maxDamage;
        }

        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        // �ܻ���Ч
        audioManager.Play("Hit",false);
       
        currentHealth -= damage;

        if (!isFlashing)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            // ������Ч
            audioManager.Play("Gameover",false);

            currentHealth = 0;
            isDead = true;
            // ��������
            anim.SetBool("Dead", isDead);
        }
        anim.SetTrigger("Hit");

        // ��������ֵ��ʾ
        health.SetHealth(currentHealth);
    }
    void IDamagable.TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        health.SetHealth(currentHealth);
    }
    void IDamagable.Heal(float amount)
    {
        Heal(amount);
    }

    IEnumerator FlashEffect()
    {
        isFlashing = true;

        // ������˸����
        int flashCount = Mathf.FloorToInt(flashDuration / flashInterval);

        for (int i = 0; i < flashCount; i++)
        {
            // �л�������Ⱦ���Ŀɼ���
            ToggleRenderers(false);
            yield return new WaitForSeconds(flashInterval / 2);
            ToggleRenderers(true);
            yield return new WaitForSeconds(flashInterval / 2);
        }

        // ȷ������ǿɼ���
        ToggleRenderers(true);
        isFlashing = false;
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreDisplay.SetScore(this.score);
    }

    void ToggleRenderers(bool visible)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}
