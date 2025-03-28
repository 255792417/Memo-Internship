using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // ����ģʽ
    public static SaveManager instance;

    [Header("��Ҫ�������Դ")]
    [SerializeField] private Item[] allItems;
    [SerializeField] private Inventory playerInventory;

    [Header("�浵����")]
    [SerializeField] private string saveFileName = "gamesave.json";
    [SerializeField] private string emptyFileName = "emptysave.json";

    private Dictionary<string, Item> itemLookup = new Dictionary<string, Item>();

    // �浵·��
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);
    // �հ״浵·��
    private string EmptySavePath => Path.Combine(Application.persistentDataPath, emptyFileName);

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // ��ʼ����Ʒ�����ֵ�
        foreach (var item in allItems)
        {
            itemLookup[item.itemName] = item;
        }

        // ���û�пմ浵������һ��
        if (!File.Exists(EmptySavePath))
        {
            CreateEmptySave();
        }
    }

    // �����մ浵������Ĭ��ֵ��
    public void CreateEmptySave()
    {
        GameSaveData emptySave = new GameSaveData
        {
            saveTime = System.DateTime.Now,
            saveName = "Empty Save"
        };

        // ����������Ʒ��Ĭ��״̬
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

        // �������Ĭ��״̬
        emptySave.inventory.itemNames.AddRange(playerInventory.defaultItemNames);

        // ����մ浵
        string json = JsonUtility.ToJson(emptySave, true);
        File.WriteAllText(EmptySavePath, json);

        Debug.Log("�����˿մ浵: " + EmptySavePath);
    }

    // ���浱ǰ��Ϸ״̬
    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData
        {
            saveTime = System.DateTime.Now,
            saveName = "Game Save"
        };

        // ����������Ʒ
        foreach (var item in allItems)
        {
            saveData.items.Add(item.ToData());
        }

        // ������
        saveData.inventory = playerInventory.ToData();

        // ���������л�ΪJSON
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("��Ϸ�ѱ�����: " + SavePath);
    }

    // ������Ϸ
    public void LoadGame()
    {
        LoadGameFromPath(SavePath);
    }

    // ���ؿմ浵��������Ϸ��
    public void LoadEmptySave()
    {
        LoadGameFromPath(EmptySavePath);

        // ˢ��UI
        RefreshInventoryUI();

        Debug.Log("��Ϸ������Ϊ�մ浵");
    }

    // ��ָ��·�����ش浵
    private void LoadGameFromPath(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

            // ������Ʒ����
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

            // ���ؿ������
            playerInventory.LoadFromData(saveData.inventory, itemLookup);

            Debug.Log("��Ϸ�Ѵ�����λ�ü���: " + path);
        }
        else
        {
            Debug.LogError("�浵�ļ�������: " + path);
        }
    }

    // ˢ�¿��UI
    private void RefreshInventoryUI()
    {
        // �����ǰUI
        if (InventoryManager.instance != null)
        {
            // ������в�
            foreach (Transform child in InventoryManager.instance.slotGrid.transform)
            {
                Destroy(child.gameObject);
            }

            InventoryManager.instance.slots.Clear();

            // ���´���������Ʒ��
            foreach (var item in playerInventory.itemList)
            {
                InventoryManager.CreateNewItem(item);
            }
        }
    }
}
