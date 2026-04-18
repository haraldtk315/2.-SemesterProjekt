using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HAS_INFO : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public INFO_BOX INFO;
    public CHAMP_INFO CHAMP;
    public Item_button_click ITEM_INFO;
    public ItemData ITEM_DATA;
    public string Text;

    public enum TYPE
    {
        SIMPLE_TEXT,
        CHAMP_INFO,
        ITEM
    }

    public TYPE Info_type;

    private void Awake()
    {
        INFO = GameObject.FindGameObjectWithTag("INFO").GetComponent<INFO_BOX>();
    }

    private void OnEnable()
    {
        INFO = GameObject.FindGameObjectWithTag("INFO").GetComponent<INFO_BOX>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Info_type == TYPE.SIMPLE_TEXT)
        {
            Debug.Log("something is here");
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }

        if (Info_type == TYPE.ITEM)
        {
            ITEM_INFO = GetComponent<Item_button_click>();
            ITEM_DATA = ITEM_INFO.Item.itemData;

            Text = string.Empty;
            Text = ITEM_DATA.displayName + ": \n";
            Text += ITEM_DATA.description;

            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(INFO.wait_time);

        INFO.INSTANT_SHOW(Text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        INFO.HIDE();
        Debug.Log("NO MORE!");
    }
    
    private void OnMouseOver()
    {
        if (Info_type == TYPE.SIMPLE_TEXT)
        {
            INFO.SHOW(Text);
        }
        
        if (Info_type == TYPE.CHAMP_INFO)
        {
            CHAMP = GetComponent<CHAMP_INFO>();
            Text = string.Empty;
            Text = CHAMP.Name + ": \n";
            Text += "MAX HP: " + CHAMP.MaxHp.ToString() + "\n \n";
            Text += "Description: \n";
            Text += CHAMP.descripton;
            INFO.SHOW(Text);
        }

        if (Info_type == TYPE.ITEM)
        {
            
        }
    }

    private void OnMouseExit()
    {
        INFO.HIDE();
    }
}
