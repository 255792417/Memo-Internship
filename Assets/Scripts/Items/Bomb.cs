using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AudioManager audioManager;
    private Animator anim;

    public float startTime;
    public float waitTime;
    public float bombDamage;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= startTime + waitTime)
        {
            anim.SetTrigger("Explode");
        }
    }

    public void Explotion() // Animation Event
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (Collider2D enemy in enemiesToDamage)
        {
            enemy.GetComponent<IDamagable>().TakeDamage(bombDamage);
        }
    }

    void PlayAudio()
    {
        audioManager.Play("Bomb",false);
    }

    public void DestroyBomb() // Animation Event
    {
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
