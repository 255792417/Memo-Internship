using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class MinesManager : MonoBehaviour
{
    // 单例模式
    private static MinesManager instance;

    // 每种矿石挖了多少个
    private Dictionary<string, int> mineScoreDict = new Dictionary<string, int>();

    [System.Serializable]
    public class Mine
    {
        public GameObject minePrefab;
        public string name;
    }
    [SerializeField]
    private List<Mine> minesList = new List<Mine>();

    // 通过名称查找预制体
    private Dictionary<string, GameObject> minesPrefabDict = new Dictionary<string, GameObject>();
    // 每种矿石都单独使用一个对象池
    private Dictionary<string, ObjectPool<GameObject>> minePools = new Dictionary<string, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitDict();
    }

    public static MinesManager Instance
    {
        get
        {
            return instance;
        }
    }

    // 获取矿石已经挖到的个数
    public int GetMineScore(string name)
    {
        if(mineScoreDict.ContainsKey(name))
        {
            return mineScoreDict[name];
        }
        else
        {
            return 0;
        }
    }


    // 生成可拾取的矿石
    public GameObject SpawnMine(string name, Vector3 position)
    {
        GameObject mine = minePools[name].Get();
        mine.transform.position = position;
        return mine;
    }

    // 拾取时销毁
    public void ReleaseMine(string name, GameObject mineObject)
    {
        minePools[name].Release(mineObject);
        mineScoreDict[name] += 1;
    }

    private void InitDict()
    {
        foreach (var mine in minesList)
        {
            if (mine.minePrefab != null && !string.IsNullOrEmpty(mine.name))
            {
                mineScoreDict.Add(mine.name , 0);

                minesPrefabDict.Add(mine.name, mine.minePrefab);

                // 为每种矿物创建对象池
                minePools[mine.name] = new ObjectPool<GameObject>(
                    () => OnCreateMine(mine.name),  // 创建函数
                    OnGetMine,                      // 获取函数
                    OnReleaseMine,                  // 释放函数
                    OnDestroyMine,                  // 销毁函数
                    false,                          // 默认不收集超出容量的对象
                    10,                             // 默认容量
                    100                             // 最大容量
                );
            }
        }
    }

    private GameObject OnCreateMine(string mineName)
    {
        GameObject mineInstance = Instantiate(minesPrefabDict[mineName]);
        return mineInstance;
    }

    private void OnGetMine(GameObject mineObject)
    {
        // 从对象池获取对象时激活它
        mineObject.SetActive(true);
    }

    private void OnReleaseMine(GameObject mineObject)
    {
        // 回收对象时禁用它
        mineObject.SetActive(false);
    }

    private void OnDestroyMine(GameObject mineObject)
    {
        // 当对象池销毁对象时调用
        Destroy(mineObject);
    }
}