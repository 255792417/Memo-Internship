using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    [Header("伤害设置")]
    public int contactDamage = 10;

    
    void OnTriggerStay2D(Collider2D other)
    {
        // 如果碰到玩家，扣玩家的血
        if (other.CompareTag("Player"))
        {
            DealDamageToPlayer(other);
        }
    }

    void DealDamageToPlayer(Collider2D player)
    {
        IDamagable damageable = player.GetComponent<IDamagable>();
        damageable.TakeDamage(contactDamage);
    }
}
