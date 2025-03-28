using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;

    public List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        // 初始化UI
        RefreshAllItems();
    }

    public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
        instance.slots.Add(newItem);
    }

    public static void RefreshItem(Item item)
    {
        foreach (Slot slot in instance.slots)
        {
            if (slot.slotItem == item)
            {
                slot.slotNum.text = item.itemHeld.ToString();
            }
        }
    }

    // 清除并重新创建所有物品槽
    public void RefreshAllItems()
    {
        // 清除所有槽
        foreach (Transform child in slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        slots.Clear();

        // 重新创建所有物品槽
        foreach (var item in myBag.itemList)
        {
            CreateNewItem(item);
        }
    }
}
