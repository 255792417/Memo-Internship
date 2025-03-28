using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Inventory")]
    public Item thisItem;
    public Inventory playerInventory;

    public int score = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到玩家，加分，销毁，加入背包
        if (other.CompareTag("Player"))
        {
            other.GetComponent<StatusController>().AddScore(score);
            // 截掉(Clone)
            string targetName = gameObject.name.Substring(0, gameObject.name.Length - 7);
            MinesManager.Instance.ReleaseMine(targetName, gameObject);

            AddNewItem();
        }
    }

    public void AddNewItem()
    {
        playerInventory.AddItem(thisItem);
    }
}
