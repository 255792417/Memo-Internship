using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();

    [HideInInspector] public List<string> defaultItemNames = new List<string>();

    private void OnEnable()
    {
        #if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    defaultItemNames.Clear();
                    foreach (var item in itemList)
                    {
                        defaultItemNames.Add(item.itemName);
                    }
                }
        #endif
    }

    public void AddItem(Item item)
    {
        if (!itemList.Contains(item))
        {
            itemList.Add(item);
            InventoryManager.CreateNewItem(item);
        }
        else
        {
            item.itemHeld += 1;
            InventoryManager.RefreshItem(item);
        }
    }

    // ת��Ϊ�����л�����
    public InventoryData ToData()
    {
        InventoryData data = new InventoryData();

        foreach (var item in itemList)
        {
            data.itemNames.Add(item.itemName);
        }

        return data;
    }

    // �ӿ����л����ݼ���
    public void LoadFromData(InventoryData data, Dictionary<string, Item> itemLookup)
    {
        itemList.Clear();

        foreach (var itemName in data.itemNames)
        {
            foreach (var item in itemLookup.Values)
            {
                if (item.itemName == itemName)
                {
                    itemList.Add(item);
                    break;
                }
            }
        }
    }

    // ���õ�Ĭ��ֵ
    public void ResetToDefault(Dictionary<string, Item> itemLookup)
    {
        itemList.Clear();

        foreach (var itemName in defaultItemNames)
        {
            foreach (var item in itemLookup.Values)
            {
                if (item.itemName == itemName)
                {
                    itemList.Add(item);
                    break;
                }
            }
        }
    }
}
