using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemsManager : MonoBehaviour
{
    //单例模式
    public static ItemsManager Instance;

    public AudioManager audioManager; // 音效管理
    public Animator anim; // 动画
    public StatusController playerStatus; // 玩家状态管理
    public GameObject bombPrefab; // 炸弹预制体
    public Transform playerTransform; // 玩家位置（用于生成炸弹）
    public GameObject bagPanel; // 背包界面

    // 对象池
    public ObjectPool<GameObject> bombPool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 创建对象池
        bombPool = new ObjectPool<GameObject>(
            createFunc: CreateBomb,       // 创建对象的方法
            actionOnGet: OnGetBomb,       // 获取对象时的操作
            actionOnRelease: OnReleaseBomb, // 释放对象时的操作
            actionOnDestroy: OnDestroyBomb, // 销毁对象时的操作
            collectionCheck: true,          // 是否检查对象是否已经在池中
            defaultCapacity: 10,            // 默认容量
            maxSize: 100                    // 最大容量
            );
    }


    private GameObject CreateBomb()
    {
        GameObject bombObj = Instantiate(bombPrefab);
        bombObj.GetComponent<Bomb>().audioManager = audioManager;

        return bombObj;
    }

    private void OnGetBomb(GameObject bomb)
    {
        bomb.transform.position = playerTransform.position;
        bomb.SetActive(true);
    }

    private void OnReleaseBomb(GameObject bomb)
    {
        bomb.SetActive(false);
    }

    private void OnDestroyBomb(GameObject bomb)
    {
        Destroy(bomb);
    }

    private void Update()
    {
        // 按 1 恢复氧气
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerStatus.AddOxygen(playerStatus.maxOxygen);
            anim.SetTrigger("OxygenCapsule");
            audioManager.Play("OxygenCapsule", false);
        }

        // 按 2 恢复生命
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerStatus.Heal(playerStatus.maxHealth);
            anim.SetTrigger("MedicalKit");
            audioManager.Play("MedicalKit", false);
        }

        // 按 3 生成炸弹
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject bomb = bombPool.Get();
        }

        // 按 4 传送回地面
        if (Input.GetKeyDown(KeyCode.Alpha4) && playerTransform.position.y < 0)
        {
            playerStatus.transform.position = new Vector3(0, 0, 0);
            anim.SetTrigger("Teleport");
            audioManager.Play("Teleport", false);
        }

        // 按 E 打开背包
        if (Input.GetKeyDown(KeyCode.E) && !bagPanel.activeSelf)
        {
            bagPanel.SetActive(true);
            InventoryManager.instance.RefreshAllItems();
        }

        // 按 Q 关闭背包
        if (Input.GetKeyDown(KeyCode.Q) && bagPanel.activeSelf)
        {
            bagPanel.SetActive(false);
        }
    }
}
