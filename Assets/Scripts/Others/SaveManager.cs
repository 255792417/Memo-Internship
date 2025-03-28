using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 单例模式
    public static SaveManager instance;

    [Header("需要保存的资源")]
    [SerializeField] private Item[] allItems;
    [SerializeField] private Inventory playerInventory;

    [Header("存档设置")]
    [SerializeField] private string saveFileName = "gamesave.json";
    [SerializeField] private string emptyFileName = "emptysave.json";

    private Dictionary<string, Item> itemLookup = new Dictionary<string, Item>();

    // 存档路径
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);
    // 空白存档路径
    private string EmptySavePath => Path.Combine(Application.persistentDataPath, emptyFileName);

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // 初始化物品查找字典
        foreach (var item in allItems)
        {
            itemLookup[item.itemName] = item;
        }

        // 如果没有空存档，创建一个
        if (!File.Exists(EmptySavePath))
        {
            CreateEmptySave();
        }
    }

    // 创建空存档（基于默认值）
    public void CreateEmptySave()
    {
        GameSaveData emptySave = new GameSaveData
        {
            saveTime = System.DateTime.Now,
            saveName = "Empty Save"
        };

        // 保存所有物品的默认状态
        foreach (var item in allItems)
        {
            ItemData itemData = new ItemData
            {
                itemName = item.itemName,
                itemHeld = item.defaultItemHeld,
                canEquip = item.canEquip
            };

            emptySave.items.Add(itemData);
        }

        // 保存库存的默认状态
        emptySave.inventory.itemNames.AddRange(playerInventory.defaultItemNames);

        // 保存空存档
        string json = JsonUtility.ToJson(emptySave, true);
        File.WriteAllText(EmptySavePath, json);

        Debug.Log("创建了空存档: " + EmptySavePath);
    }

    // 保存当前游戏状态
    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData
        {
            saveTime = System.DateTime.Now,
            saveName = "Game Save"
        };

        // 保存所有物品
        foreach (var item in allItems)
        {
            saveData.items.Add(item.ToData());
        }

        // 保存库存
        saveData.inventory = playerInventory.ToData();

        // 将数据序列化为JSON
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("游戏已保存至: " + SavePath);
    }

    // 加载游戏
    public void LoadGame()
    {
        LoadGameFromPath(SavePath);
    }

    // 加载空存档（重置游戏）
    public void LoadEmptySave()
    {
        LoadGameFromPath(EmptySavePath);

        // 刷新UI
        RefreshInventoryUI();

        Debug.Log("游戏已重置为空存档");
    }

    // 从指定路径加载存档
    private void LoadGameFromPath(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

            // 加载物品数据
            foreach (var itemData in saveData.items)
            {
                foreach (var item in allItems)
                {
                    if (item.itemName == itemData.itemName)
                    {
                        item.LoadFromData(itemData);
                        break;
                    }
                }
            }

            // 加载库存数据
            playerInventory.LoadFromData(saveData.inventory, itemLookup);

            Debug.Log("游戏已从以下位置加载: " + path);
        }
        else
        {
            Debug.LogError("存档文件不存在: " + path);
        }
    }

    // 刷新库存UI
    private void RefreshInventoryUI()
    {
        // 清除当前UI
        if (InventoryManager.instance != null)
        {
            // 清除所有槽
            foreach (Transform child in InventoryManager.instance.slotGrid.transform)
            {
                Destroy(child.gameObject);
            }

            InventoryManager.instance.slots.Clear();

            // 重新创建所有物品槽
            foreach (var item in playerInventory.itemList)
            {
                InventoryManager.CreateNewItem(item);
            }
        }
    }
}
