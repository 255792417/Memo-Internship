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
    public float maxOxygen = 100f; // 最大氧气值
    public float maxHealth = 100f; // 最大生命值

    [Header("Current Stats")]
    public float currentOxygen; // 当前氧气值
    public float currentHealth; // 当前生命值
    public bool isDead = false; // 是否死亡

    [Header("Score")]
    public int score = 0; // 分数

    [Header("Fall Damage")]
    public float minFallVelocity = -0f;  // 开始造成伤害的最小下落速度
    public float maxFallVelocity = -20f;  // 造成最大伤害的下落速度
    public int minDamage = 5;             // 最小伤害值
    public int maxDamage = 50;            // 最大伤害值
    public float fallDamageMultiplier = 1.0f; // 伤害倍率
    private float fallVelocity;           // 下落速度

    [Header("Flash")]
    public float flashDuration = 1f;    // 闪烁持续时间
    public float flashInterval = 0.1f;    // 闪烁间隔

    public Renderer[] renderers;
    private bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;

        // 初始化生命值显示
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
        // 20 层以下每层消耗 1 点氧气
        int depth = (int)((-0.4f - transform.position.y) / 0.65);
        if (depth > 20)
        {
            ReduceOxygen((depth - 20) * Time.deltaTime);
        }
        // 五层以上每秒回复 20 点氧气
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

        // 计算伤害值
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
        // 受击音效
        audioManager.Play("Hit",false);
       
        currentHealth -= damage;

        if (!isFlashing)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            // 死亡音效
            audioManager.Play("Gameover",false);

            currentHealth = 0;
            isDead = true;
            // 死亡动画
            anim.SetBool("Dead", isDead);
        }
        anim.SetTrigger("Hit");

        // 更新生命值显示
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

        // 计算闪烁次数
        int flashCount = Mathf.FloorToInt(flashDuration / flashInterval);

        for (int i = 0; i < flashCount; i++)
        {
            // 切换所有渲染器的可见性
            ToggleRenderers(false);
            yield return new WaitForSeconds(flashInterval / 2);
            ToggleRenderers(true);
            yield return new WaitForSeconds(flashInterval / 2);
        }

        // 确保最后是可见的
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
