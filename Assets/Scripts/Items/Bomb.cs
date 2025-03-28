using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AudioManager audioManager;
    private Animator anim;

    public float startTime; // 起始时间
    public float waitTime; // 爆炸等待时长
    public float bombDamage; // 爆炸伤害

    [Header("Check")]
    public float radius; // 作用范围
    public LayerMask targetLayer; // 目标所在层

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // 到达爆炸时间，播放爆炸动画
        if(Time.time >= startTime + waitTime)
        {
            anim.SetTrigger("Explode");
        }
    }

    // 爆炸
    public void Explotion() // Animation Event
    {
        // 获取范围内应该受伤的对象
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (Collider2D enemy in enemiesToDamage)
        {
            // 施加伤害
            enemy.GetComponent<IDamagable>().TakeDamage(bombDamage);
        }
    }

    void PlayAudio()
    {
        audioManager.Play("Bomb",false);
    }

    public void DestroyBomb() // Animation Event
    {
        // 回收炸弹
        ItemsManager.Instance.bombPool.Release(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
