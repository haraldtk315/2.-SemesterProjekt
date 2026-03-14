using UnityEngine;
using System.Collections.Generic;

public class GAMEMANAGER : MonoBehaviour
{
    public static GAMEMANAGER instance;

    public GameObject[] party;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    //har ændret ldit i hvordan singleton spå ud vi kan sagtens rette det igen
    // undrede mig mpåske bare over hvorfor du ikke skriver gameobjev´ct istedet? - harald



    // Inventory management
    public void AddItem(ItemData itemData, int amount)
    {
        InventoryItem existing = inventory.Find(i => i.itemData == itemData);

        if (existing != null)
        {
            existing.amount += amount;
        }
        else
        {
            inventory.Add(new InventoryItem(itemData, amount));
        }

        Debug.Log($"Added {amount}x {itemData.displayName}");
    }
    public bool RemoveItem(ItemData itemData, int amount)
    {
        InventoryItem existing = inventory.Find(i => i.itemData == itemData);

        if (existing == null || existing.amount < amount)
            return false;

        existing.amount -= amount;

        if (existing.amount <= 0)
        {
            inventory.Remove(existing);
        }

        return true;
    }
    public int GetItemAmount(ItemData itemData)
    {
        InventoryItem existing = inventory.Find(i => i.itemData == itemData);
        return existing != null ? existing.amount : 0;
    }
}
