using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class MinesManager : MonoBehaviour
{
    // ����ģʽ
    private static MinesManager instance;

    // ÿ�ֿ�ʯ���˶��ٸ�
    private Dictionary<string, int> mineScoreDict = new Dictionary<string, int>();

    [System.Serializable]
    public class Mine
    {
        public GameObject minePrefab;
        public string name;
    }
    [SerializeField]
    private List<Mine> minesList = new List<Mine>();

    // ͨ�����Ʋ���Ԥ����
    private Dictionary<string, GameObject> minesPrefabDict = new Dictionary<string, GameObject>();
    // ÿ�ֿ�ʯ������ʹ��һ�������
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

    // ��ȡ��ʯ�Ѿ��ڵ��ĸ���
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


    // ���ɿ�ʰȡ�Ŀ�ʯ
    public GameObject SpawnMine(string name, Vector3 position)
    {
        GameObject mine = minePools[name].Get();
        mine.transform.position = position;
        return mine;
    }

    // ʰȡʱ����
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

                // Ϊÿ�ֿ��ﴴ�������
                minePools[mine.name] = new ObjectPool<GameObject>(
                    () => OnCreateMine(mine.name),  // ��������
                    OnGetMine,                      // ��ȡ����
                    OnReleaseMine,                  // �ͷź���
                    OnDestroyMine,                  // ���ٺ���
                    false,                          // Ĭ�ϲ��ռ����������Ķ���
                    10,                             // Ĭ������
                    100                             // �������
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
        // �Ӷ���ػ�ȡ����ʱ������
        mineObject.SetActive(true);
    }

    private void OnReleaseMine(GameObject mineObject)
    {
        // ���ն���ʱ������
        mineObject.SetActive(false);
    }

    private void OnDestroyMine(GameObject mineObject)
    {
        // ����������ٶ���ʱ����
        Destroy(mineObject);
    }
}