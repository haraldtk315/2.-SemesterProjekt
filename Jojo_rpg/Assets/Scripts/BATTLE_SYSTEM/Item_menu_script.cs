using NUnit.Framework;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_menu_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI No_items_text;
    public GameObject Content;
    public GAMEMANAGER GM;

    public List<GameObject> Items;
    public GameObject Item_prefab;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();

        if (GM.inventory.Count == 0)
        {
            Debug.Log("Inventory is Empty");
            No_items_text.gameObject.SetActive(true);
        }

        for (int i = 0; i < GM.inventory.Count; i++)
        {
            GameObject Item = Instantiate(Item_prefab, Vector3.zero, Quaternion.identity, Content.transform);
            InventoryItem item_info = GM.inventory[i];
            Item.GetComponentInChildren<TextMeshProUGUI>().text = item_info.itemData.displayName + "\n X: " + item_info.amount;
            Item.GetComponent<Image>().sprite = item_info.itemData.icon;
            Item.GetComponent<Item_button_click>().Item = item_info;

            Items.Add(Item);
        }

    }

    private void OnEnable()
    {
        for (int i = 0; i < GM.inventory.Count; i++)
        {
            InventoryItem item_info = GM.inventory[i];

            if (item_info.amount <= 0)
            {
                Destroy(Items[i].gameObject);
                GM.inventory.Remove(GM.inventory[i]);
                //Items.Remove(Items[i]);

            }
            else
            {
                Items[i].GetComponentInChildren<TextMeshProUGUI>().text = item_info.itemData.displayName + "\n X: " + item_info.amount;
                Items[i].GetComponent<Item_button_click>().Item = item_info;
                Items[i].GetComponent<Image>().sprite = item_info.itemData.icon;
            }
        }

        if (GM.inventory.Count == 0)
        {
            Debug.Log("Inventory is Empty");
            No_items_text.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
    }
}
