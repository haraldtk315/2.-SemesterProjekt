using NUnit.Framework;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

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
            Instantiate(Item_prefab, Vector3.zero, Quaternion.identity, Content.transform);
        }

    }

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
    }
}
