using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;
    public bool canEquip;

    [HideInInspector] public int defaultItemHeld;

    private void OnEnable()
    {
        #if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    defaultItemHeld = itemHeld;
                }
        #endif
    }

    // 转换为可序列化数据
    public ItemData ToData()
    {
        return new ItemData
        {
            itemName = this.itemName,
            itemHeld = this.itemHeld,
            canEquip = this.canEquip
        };
    }

    // 从可序列化数据加载
    public void LoadFromData(ItemData data)
    {
        // 只更新运行时可变的属性
        this.itemHeld = data.itemHeld;
    }

    // 重置到默认值
    public void ResetToDefault()
    {
        this.itemHeld = defaultItemHeld;
    }
}
