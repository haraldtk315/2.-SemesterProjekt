using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GAMEMANAGER : MonoBehaviour
{
    public static GAMEMANAGER instance;

    public GameObject[] party;
    public int[] HP = { 0, 0, 0, 0, 0 };
    public List<InventoryItem> inventory = new List<InventoryItem>();


    // Return to overworld
    public string returnSceneName;
    public Vector3 returnPlayerPosition;
    public Vector2 returnPlayerFacing;
    public bool shouldRestorePlayer = false;

    // World state
    public HashSet<string> collectedPickups = new HashSet<string>();
    public HashSet<string> defeatedNPCs = new HashSet<string>();

    //current npc battle
    public string currentNPCID;
    public string pendingPostBattleNPCID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < party.Length; i++)
            {
                if (party[i] != null)
                {
                    HP[i] = party[i].GetComponent<CHAMP_INFO>().MaxHp;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SaveOverworldReturnPoint(Transform playerTransform, Vector2 facing)
    {

        returnSceneName = SceneManager.GetActiveScene().name;
        returnPlayerPosition = playerTransform.position;
        returnPlayerFacing = facing;
        shouldRestorePlayer = true;
    }

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
