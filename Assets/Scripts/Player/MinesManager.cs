using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MinesManager : MonoBehaviour
{
    private static MinesManager instance;
    private string defaultPath = "Prefabs/Items/";

    [System.Serializable]
    public class Mine
    {
        public GameObject minePrefab;
        public string name;
    }
    List<Mine> minesList;

    Dictionary<string, GameObject> minesDict;

    Dictionary<string, ObjectPool<GameObject>> minePools = new Dictionary<string, ObjectPool<GameObject>>();

    private void Awake()
    {
        InitDict();
    }

    public static MinesManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MinesManager();
            }
            return instance;
        }
    }

    public void SpawnMine(string name)
    {

    }

    private void InitDict()
    {
        foreach(var mine in minesList)
        {
            minesDict.Add(name, mine.minePrefab);
            minePools[name] = new ObjectPool<GameObject>(OnGetMine, OnReleaseMine, onDestrotMine, false, 10, 100);
        }
    }

    private GameObject OnGetMine()
    {

    }
}
