using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AudioManager audioManager;
    private Animator anim;

    public float startTime; // ��ʼʱ��
    public float waitTime; // ��ը�ȴ�ʱ��
    public float bombDamage; // ��ը�˺�

    [Header("Check")]
    public float radius; // ���÷�Χ
    public LayerMask targetLayer; // Ŀ�����ڲ�

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ﱬըʱ�䣬���ű�ը����
        if(Time.time >= startTime + waitTime)
        {
            anim.SetTrigger("Explode");
        }
    }

    // ��ը
    public void Explotion() // Animation Event
    {
        // ��ȡ��Χ��Ӧ�����˵Ķ���
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (Collider2D enemy in enemiesToDamage)
        {
            // ʩ���˺�
            enemy.GetComponent<IDamagable>().TakeDamage(bombDamage);
        }
    }

    void PlayAudio()
    {
        audioManager.Play("Bomb",false);
    }

    public void DestroyBomb() // Animation Event
    {
        // ����ը��
        ItemsManager.Instance.bombPool.Release(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
