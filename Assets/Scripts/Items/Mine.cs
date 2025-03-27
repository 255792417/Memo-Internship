using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;

    public int score = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<StatusController>().AddScore(score);
            // ½Øµô(Clone)
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
