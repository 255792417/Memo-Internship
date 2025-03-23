using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Player Player;

    // 处理逐步破坏挖掘效果
    [System.Serializable]
    public class TileType
    {
        public TileBase tile;
        public float maxHealth = 100f;
        public TileBase[] damageStages;

        [Header("矿石属性")]
        public bool isOre = false; // 是否是矿石
        public GameObject orePrefab;  // 矿物预制体
    }

    [Header("Tilemap")]
    public Tilemap groundTilemap;
    public Tilemap replacementTilemap;
    public RuleTile replacementRuleTile;
    public List<TileType> tileTypes;
    public float DamagePerSecond = 100f;

    private Dictionary<Vector3Int, float> tileHealthMap = new Dictionary<Vector3Int, float>();

    // 处理音效
    [Header("Audio")]
    public AudioSource drillSound;

    private void Start()
    {
        Player = GetComponentInChildren<Player>();

        initTileHealth();
    }

    // 初始化瓦片生命值
    private void initTileHealth()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = groundTilemap.GetTile(pos);
            if (tile != null)
            {
                TileType tileType = GetTileType(tile);
                if (tileType != null)
                {
                    tileHealthMap[pos] = tileType.maxHealth;
                }
            }
        }
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        Drill();
    }

    //判断破坏
    void Drill()
    {
        if (Player.isGround)
        {
            Vector3Int cell = groundTilemap.WorldToCell(Player.transform.position);
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cell = groundTilemap.WorldToCell(Player.transform.position + new Vector3(0, -0.8f, 0));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                cell = groundTilemap.WorldToCell(Player.transform.position + new Vector3(0.75f, 0, 0));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                cell = groundTilemap.WorldToCell(Player.transform.position + new Vector3(-0.75f, 0, 0));
            }
            if (groundTilemap.GetTile(cell) != null)
            {
                DamageTile(cell, DamagePerSecond / 50);

                RefreshSurroundingTiles(cell);
            }
        }
    }


    //处理瓦片破坏 和 阶段图像
    public void DamageTile(Vector3Int position, float damage)
    {
        TileBase currentTile = groundTilemap.GetTile(position);
        if (currentTile == null) return;

        TileType tileType = GetTileType(currentTile);
        if (tileType == null) return;

        if (!tileHealthMap.ContainsKey(position))
        {
            tileHealthMap[position] = tileType.maxHealth;
        }

        tileHealthMap[position] -= damage;

        if (tileHealthMap[position] <= 0)
        {
            // 从原始Tilemap移除瓦片
            groundTilemap.SetTile(position, null);
            // 在替换Tilemap上放置RuleTile
            replacementTilemap.SetTile(position, replacementRuleTile);

            // 如果是矿石，生成矿物
            if (tileType.isOre && tileType.orePrefab != null)
            {
                SpawnOreItems(position, tileType);
            }
        }
        else
        {
            float healthPercentage = tileHealthMap[position] / tileType.maxHealth;
            int stageIndex = Mathf.FloorToInt((1 - healthPercentage) * tileType.damageStages.Length);
            stageIndex = Mathf.Clamp(stageIndex, 0, tileType.damageStages.Length - 1);

            groundTilemap.SetTile(position, tileType.damageStages[stageIndex]);
        }
    }

    TileType GetTileType(TileBase tile)
    {
        return tileTypes.Find(t => t.tile == tile || System.Array.IndexOf(t.damageStages, tile) >= 0);
    }


    // 刷新碰撞箱
    void RefreshSurroundingTiles(Vector3Int position)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int neighborPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (groundTilemap.HasTile(neighborPos))
                {
                    groundTilemap.RefreshTile(neighborPos);
                }
                if (replacementTilemap.HasTile(neighborPos))
                {
                    replacementTilemap.RefreshTile(neighborPos);
                }
            }
        }
    }

        private void SpawnOreItems(Vector3Int tilePosition, TileType tileType)
    {
        // 获取世界坐标
        Vector3 worldPos = groundTilemap.GetCellCenterWorld(tilePosition);
        // 实例化物品
        GameObject item = Instantiate(tileType.orePrefab, worldPos, Quaternion.identity);
    }

}
