using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioManager audioManager;
    public StatusController statusController;

    private Rigidbody2D rb;
    public GameObject landCheck;
    public GameObject LeftCheck;
    public GameObject RightCheck;

    public GameObject PlayerHead;
    public GameObject PlayerBody;
    public GameObject fire;

    [Header("Movement")]
    public float speed;
    public float flySpeed;
    public float flyingIncreaseIndex;

    [Header("Animation")]
    private Animator anim;
    public bool isWalking;
    public bool isFlying;
    public SpriteRenderer headSp;

    [Header("Drilling")]
    public GameObject drillLeft;
    public GameObject drillRight;
    public GameObject drillDown;
    public bool isDrillingLeft;
    public bool isDrillingRight;
    public bool isDrillingDown;

    [Header("Ground Check")]
    public bool isGround;
    public float groundCheckRadius;
    public float LeftCheckRadius;
    public float RightCheckRadius;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        statusController = GetComponent<StatusController>();
    }

    void FixedUpdate()
    {
        // 如果死亡，停止运动
        if (statusController != null && statusController.isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Movement();
        Animation();
        GroundCheck();
        DrillCheck();
        Drill();
    }

    // 处理运动
    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        //  翻转图像
        if (horizontal != 0 && isGround)
        {
            PlayerHead.transform.localScale = new Vector3(-horizontal, 1, 1);
            PlayerBody.transform.localScale = new Vector3(-horizontal, 1, 1);
        }

        float vertical = Input.GetAxisRaw("Vertical");

        // 飞行
        if (vertical > 0)
        {
            isFlying = true;
            fire.SetActive(true);
        }
        else
        {
            isFlying = false;
            fire.SetActive(false);
        }

        // 行走
        if (rb.velocity.x != 0 && isGround)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // 处理运动
        if (isFlying)
        {
            // 处理音频
            audioManager.PlayWithFixedSource(1,"Fly",false);

            rb.velocity = new Vector2(rb.velocity.x, flySpeed);
            if (rb.velocity.y < flySpeed)
            {
                rb.AddForce(Vector2.up * flySpeed * flyingIncreaseIndex);
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    //处理钻头显示 和 音效
    void Drill()
    {
        // 处理音频
        if (isDrillingDown || isDrillingLeft || isDrillingRight)
        {
            audioManager.PlayWithFixedSource(2, "Drill", false);
        }

        if (isDrillingDown)
        {
            drillDown.SetActive(true);
        }
        else
        {
            drillDown.SetActive(false);
        }

        if (isDrillingLeft)
        {
            drillLeft.SetActive(true);
        }
        else
        {
            drillLeft.SetActive(false);
        }

        if (isDrillingRight)
        {
            drillRight.SetActive(true);
        }
        else
        {
            drillRight.SetActive(false);
        }

    }

    void DrillCheck()
    {
        // 判断左右两边是否有方块
        bool rightGround = Physics2D.OverlapCircle(RightCheck.transform.position, RightCheckRadius, groundLayer);
        bool leftGround = Physics2D.OverlapCircle(LeftCheck.transform.position, LeftCheckRadius, groundLayer);
        

        if(Input.GetKey(KeyCode.DownArrow) && isGround)
        {
            isDrillingDown = true;
            return;
        }
        else
        {
            isDrillingDown = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) && rightGround)
        {
            isDrillingRight = true;
            return;
        }
        else
        {
            isDrillingRight = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && leftGround)
        {
            isDrillingLeft = true;
            return;
        }
        else
        {
            isDrillingLeft = false;
        }

        
    }

    void Animation()
    {
        anim.SetBool("isWalking", isWalking);

        anim.SetBool("isFlying", isFlying);

        anim.SetBool("isDrillDown", isDrillingDown);
    }

    void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(landCheck.transform.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(landCheck.transform.position, groundCheckRadius);
        Gizmos.DrawWireSphere(LeftCheck.transform.position, LeftCheckRadius);
        Gizmos.DrawWireSphere(RightCheck.transform.position, RightCheckRadius);
    }
}
