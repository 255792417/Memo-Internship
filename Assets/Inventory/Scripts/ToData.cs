using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public string itemName;
    public int itemHeld;
    public bool canEquip;
}

[Serializable]
public class InventoryData
{
    public List<string> itemNames = new List<string>();
}

[Serializable]
public class GameSaveData
{
    public List<ItemData> items = new List<ItemData>();
    public InventoryData inventory = new InventoryData();
    public DateTime saveTime;
    public string saveName;
}
