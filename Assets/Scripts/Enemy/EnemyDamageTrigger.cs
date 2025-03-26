using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    [Header("…À∫¶…Ë÷√")]
    public int contactDamage = 10;


    void OnTriggerStay2D(Collider2D other)
    {
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
