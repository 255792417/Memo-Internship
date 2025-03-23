using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    [Header("ÉËº¦ÉèÖÃ")]
    public int contactDamage = 10;
    public float damageInterval = 1f;

    private float lastDamageTime;

    void Start()
    {
        lastDamageTime = -damageInterval;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= lastDamageTime + damageInterval)
        {
            DealDamageToPlayer(other);
        }
    }

    void DealDamageToPlayer(Collider2D player)
    {
        IDamagable damageable = player.GetComponent<IDamagable>();
        damageable.TakeDamage(contactDamage);
        lastDamageTime = Time.time;
    }
}
