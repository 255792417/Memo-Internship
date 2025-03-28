using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour, IDamagable
{
    EnemyBaseState currentState;

    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Health")]
    public float maxHealth = 100f; // 最大生命值
    public float currentHealth; // 当前生命值
    public bool isDead = false; // 死没死

    [Header("Animation")]
    public Animator anim;
    public int animState;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    public PatrolState patrolState = new PatrolState(); // 巡逻状态

    [Header("Tilemap")]
    public Tilemap tilemap;
    public RuleTile ruleTile;

    [Header("Turn Settings")]
    public float turnCooldown = 0.5f; // 转向冷却时间
    private float lastTurnTime; // 上次转向时间

    // 明确定义朝向
    public bool facingRight = true;
    public bool movingRight = true;

    private Vector3Int currentCell;
    private Vector3Int nextCell;

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
    }

    public void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化状态
        currentHealth = maxHealth;

        // 更换到巡逻状态
        TransitionToState(patrolState);


        // 初始化目标点
        if (targetPoint == null)
        {
            SwitchPoint();
        }

        // 根据初始目标点设置初始朝向
        if (targetPoint != null)
        {
            movingRight = targetPoint.position.x > transform.position.x;
            facingRight = movingRight;
            UpdateSpriteDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }
        currentState.OnUpdate(this);
        anim.SetInteger("State", animState);
    }

    public void TransitionToState(EnemyBaseState state) // 切换状态
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        // 检查目标点是否存在
        if (targetPoint == null)
        {
            SwitchPoint();
            return;
        }

        // 根据目标点方向确定移动方向
        movingRight = targetPoint.position.x > transform.position.x;

        // 如果朝向与移动方向不一致，更新朝向
        if (facingRight != movingRight)
        {
            facingRight = movingRight;
            UpdateSpriteDirection();
        }

        // 移动
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // 如果在冷却时间内，不检测转向
        if (Time.time - lastTurnTime < turnCooldown)
        {
            return;
        }

        // 检查下方是否有地面
        currentCell = tilemap.WorldToCell(transform.position);
        nextCell = currentCell + new Vector3Int(movingRight ? 1 : -1, -1, 0);
        TileBase nextTile = tilemap.GetTile(nextCell);

        // 检查前方是否有墙
        Vector3Int frontCell = currentCell + new Vector3Int(movingRight ? 1 : -1, 0, 0);
        TileBase frontTile = tilemap.GetTile(frontCell);

        bool shouldTurn = false;

        // 情况1: 下方没有Tile
        if (nextTile == null || nextTile == ruleTile)
        {
            shouldTurn = true;
        }

        // 情况2: 前方有墙
        if (frontTile != null && frontTile != ruleTile)
        {
            shouldTurn = true;
        }

        // 情况3: 到达目标点
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            shouldTurn = true;
        }

        // 如果需要转向
        if (shouldTurn)
        {
            // 切换目标点
            SwitchPoint();

            // 更新移动方向和朝向
            if (targetPoint != null)
            {
                movingRight = targetPoint.position.x > transform.position.x;
                facingRight = movingRight;
                UpdateSpriteDirection();
            }

            lastTurnTime = Time.time; // 记录转向时间
        }
    }

    public void UpdateSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }


    public void SwitchPoint()
    {
        // 确保两个点都存在
        if (pointA == null || pointB == null)
            return;

        // 选择距离更远的点作为目标
        if (targetPoint == pointA ||
            (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position)))
        {
            targetPoint = pointB;
        }
        else
        {
            targetPoint = pointA;
        }
    }

    // 受伤
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            anim.SetTrigger("Death");
        }
    }

    void IDamagable.TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    void IDamagable.Heal(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    // 播放死亡音效
    void PlayAudio()
    {
        audioManager.Play("EnemyDie",false);
    }

    void DestoryEnemy()
    {
        Destroy(gameObject);
    }
}
