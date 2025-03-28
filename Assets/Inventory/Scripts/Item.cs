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

    // ת��Ϊ�����л�����
    public ItemData ToData()
    {
        return new ItemData
        {
            itemName = this.itemName,
            itemHeld = this.itemHeld,
            canEquip = this.canEquip
        };
    }

    // �ӿ����л����ݼ���
    public void LoadFromData(ItemData data)
    {
        // ֻ��������ʱ�ɱ������
        this.itemHeld = data.itemHeld;
    }

    // ���õ�Ĭ��ֵ
    public void ResetToDefault()
    {
        this.itemHeld = defaultItemHeld;
    }
}
