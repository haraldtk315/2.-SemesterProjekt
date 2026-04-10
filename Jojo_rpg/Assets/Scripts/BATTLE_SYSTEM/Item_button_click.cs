using UnityEngine;

public class Item_button_click : MonoBehaviour
{

    public BATTLEHANDLER BH;

    public InventoryItem Item;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BH = GameObject.FindGameObjectWithTag("BH").GetComponent<BATTLEHANDLER>();
    }

    private void Awake()
    {
        BH = GameObject.FindGameObjectWithTag("BH").GetComponent<BATTLEHANDLER>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ON_epic_button_click()
    {
        BH.Item_click(Item);
    }
}
