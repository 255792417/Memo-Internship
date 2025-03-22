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
    }

    [Header("Tilemap")]
    public Tilemap tilemap;
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
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
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
            Vector3Int cell = tilemap.WorldToCell(Player.transform.position);
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cell = tilemap.WorldToCell(Player.transform.position + new Vector3(0, -0.8f, 0));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                cell = tilemap.WorldToCell(Player.transform.position + new Vector3(0.75f, 0, 0));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                cell = tilemap.WorldToCell(Player.transform.position + new Vector3(-0.75f, 0, 0));
            }
            if (tilemap.GetTile(cell) != null)
            {
                DamageTile(cell, DamagePerSecond / 50);

                RefreshSurroundingTiles(tilemap, cell);
            }
        }
    }


    //处理瓦片破坏 和 阶段图像
    public void DamageTile(Vector3Int position, float damage)
    {
        TileBase currentTile = tilemap.GetTile(position);
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
            tilemap.SetTile(position, replacementRuleTile);
            tileHealthMap.Remove(position);
        }
        else
        {
            float healthPercentage = tileHealthMap[position] / tileType.maxHealth;
            int stageIndex = Mathf.FloorToInt((1 - healthPercentage) * tileType.damageStages.Length);
            stageIndex = Mathf.Clamp(stageIndex, 0, tileType.damageStages.Length - 1);

            tilemap.SetTile(position, tileType.damageStages[stageIndex]);
        }
    }

    TileType GetTileType(TileBase tile)
    {
        return tileTypes.Find(t => t.tile == tile || System.Array.IndexOf(t.damageStages, tile) >= 0);
    }


    // 刷新碰撞箱
    void RefreshSurroundingTiles(Tilemap tilemap, Vector3Int position)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int neighborPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (tilemap.HasTile(neighborPos))
                {
                    tilemap.RefreshTile(neighborPos);
                }
            }
        }
    }

}
