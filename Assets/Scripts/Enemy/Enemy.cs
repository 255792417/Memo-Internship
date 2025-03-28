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
    public float maxHealth = 100f; // �������ֵ
    public float currentHealth; // ��ǰ����ֵ
    public bool isDead = false; // ��û��

    [Header("Animation")]
    public Animator anim;
    public int animState;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    public PatrolState patrolState = new PatrolState(); // Ѳ��״̬

    [Header("Tilemap")]
    public Tilemap tilemap;
    public RuleTile ruleTile;

    [Header("Turn Settings")]
    public float turnCooldown = 0.5f; // ת����ȴʱ��
    private float lastTurnTime; // �ϴ�ת��ʱ��

    // ��ȷ���峯��
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
        // ��ʼ��״̬
        currentHealth = maxHealth;

        // ������Ѳ��״̬
        TransitionToState(patrolState);


        // ��ʼ��Ŀ���
        if (targetPoint == null)
        {
            SwitchPoint();
        }

        // ���ݳ�ʼĿ������ó�ʼ����
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

    public void TransitionToState(EnemyBaseState state) // �л�״̬
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        // ���Ŀ����Ƿ����
        if (targetPoint == null)
        {
            SwitchPoint();
            return;
        }

        // ����Ŀ��㷽��ȷ���ƶ�����
        movingRight = targetPoint.position.x > transform.position.x;

        // ����������ƶ�����һ�£����³���
        if (facingRight != movingRight)
        {
            facingRight = movingRight;
            UpdateSpriteDirection();
        }

        // �ƶ�
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // �������ȴʱ���ڣ������ת��
        if (Time.time - lastTurnTime < turnCooldown)
        {
            return;
        }

        // ����·��Ƿ��е���
        currentCell = tilemap.WorldToCell(transform.position);
        nextCell = currentCell + new Vector3Int(movingRight ? 1 : -1, -1, 0);
        TileBase nextTile = tilemap.GetTile(nextCell);

        // ���ǰ���Ƿ���ǽ
        Vector3Int frontCell = currentCell + new Vector3Int(movingRight ? 1 : -1, 0, 0);
        TileBase frontTile = tilemap.GetTile(frontCell);

        bool shouldTurn = false;

        // ���1: �·�û��Tile
        if (nextTile == null || nextTile == ruleTile)
        {
            shouldTurn = true;
        }

        // ���2: ǰ����ǽ
        if (frontTile != null && frontTile != ruleTile)
        {
            shouldTurn = true;
        }

        // ���3: ����Ŀ���
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            shouldTurn = true;
        }

        // �����Ҫת��
        if (shouldTurn)
        {
            // �л�Ŀ���
            SwitchPoint();

            // �����ƶ�����ͳ���
            if (targetPoint != null)
            {
                movingRight = targetPoint.position.x > transform.position.x;
                facingRight = movingRight;
                UpdateSpriteDirection();
            }

            lastTurnTime = Time.time; // ��¼ת��ʱ��
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
        // ȷ�������㶼����
        if (pointA == null || pointB == null)
            return;

        // ѡ������Զ�ĵ���ΪĿ��
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

    // ����
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

    // ����������Ч
    void PlayAudio()
    {
        audioManager.Play("EnemyDie",false);
    }

    void DestoryEnemy()
    {
        Destroy(gameObject);
    }
}
