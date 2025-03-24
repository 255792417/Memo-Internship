using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemsManager : MonoBehaviour
{
    public StatusController playerStatus;
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    
    private ObjectPool<Bomb> bombPool;
    private int bombPoolSize = 5;
    private int bombPoolCount = 0;
    private int bombPoolIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        
    }
}
