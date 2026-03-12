using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData itemData, int amount)
    {
        InventoryItem existing = items.Find(i => i.itemData == itemData);

        if (existing != null)
        {
            existing.amount += amount;
        }
        else
        {
            items.Add(new InventoryItem(itemData, amount));
        }

        Debug.Log($"Added {amount}x {itemData.displayName}");
        PrintInventory();
    }

    public void PrintInventory()
    {
        Debug.Log("=== INVENTORY ===");

        foreach (var item in items)
        {
            Debug.Log(item.itemData.displayName + " x" + item.amount);
        }
    }
}