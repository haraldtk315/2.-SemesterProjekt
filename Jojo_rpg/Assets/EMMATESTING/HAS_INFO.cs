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
    public BUTTON_HOLDER BUTTON_HOLD;
    public string Text;

    public static bool ON_UI = false;

    public enum TYPE
    {
        SIMPLE_TEXT,
        CHAMP_INFO,
        ITEM,
        ATTACK_MOVE
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

    private void OnMouseOver()
    {
        if (ON_UI == false)
        {
            if (Info_type == TYPE.SIMPLE_TEXT)
            {
                INFO.SHOW(Text, ON_UI);
            }

            if (Info_type == TYPE.CHAMP_INFO)
            {
                CHAMP = GetComponent<CHAMP_INFO>();
                Text = string.Empty;
                Text = CHAMP.Name + ": \n";
                Text += "MAX HP: " + CHAMP.MaxHp.ToString() + "\n \n";
                Text += "Description: \n";
                Text += CHAMP.descripton;
                INFO.SHOW(Text, ON_UI);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ON_UI = true;

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
            Text += ITEM_DATA.description + " " + ITEM_DATA.value.ToString();

            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }

        if (Info_type == TYPE.ATTACK_MOVE)
        {
            BUTTON_HOLD = GetComponent<BUTTON_HOLDER>();

            if (BUTTON_HOLD.ATTACK != null)
            {
                Text = BUTTON_HOLD.ATTACK.attackName + ": \n";
                
                if (BUTTON_HOLD.ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SINGLE_HIT)
                {
                    Text += "A single hit move with a " + BUTTON_HOLD.ATTACK.acc.ToString() + "% Chance of hitting its target \n";
                    Text += "When hitting this move gain " + BUTTON_HOLD.ATTACK.focus.ToString() + " focus";
                }

                if (BUTTON_HOLD.ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL_ENEMY)
                {
                    Text += "A cleaving move with a " + BUTTON_HOLD.ATTACK.acc.ToString() + "% chance of hitting all enemy targets \n";
                    Text += "When hitting this move gain " + BUTTON_HOLD.ATTACK.focus.ToString() + " focus";
                }

                if (BUTTON_HOLD.ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL)
                {
                    Text += "A dangerous move, that hits everyone (including your own guys) with a " + BUTTON_HOLD.ATTACK.acc.ToString() + "% chance of hitting \n";
                    Text += "When hitting this move gain " + BUTTON_HOLD.ATTACK.focus.ToString() + " focus";
                }

                if (BUTTON_HOLD.ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SELF)
                {
                    Text += "A self buffing move with a " + BUTTON_HOLD.ATTACK.acc.ToString() + "% success rate \n";
                    Text += "When using this move gain " + BUTTON_HOLD.ATTACK.focus.ToString() + " focus";
                }

                if (BUTTON_HOLD.ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.Party)
                {
                    Text += "A healing move with a " + BUTTON_HOLD.ATTACK.acc.ToString() + "% success rate \n";
                    Text += "heals for " + (-BUTTON_HOLD.ATTACK.damage).ToString() + " damage \n";
                    Text += "When using this move gain " + BUTTON_HOLD.ATTACK.focus.ToString() + " focus";
                }

                StopAllCoroutines();
                StartCoroutine(StartTimer());
            }

            if (BUTTON_HOLD.SPECIALS != null)
            {
                Text = string.Empty;
                Text = BUTTON_HOLD.SPECIALS.attackName + ": \n";
                Text += "Cost: " + BUTTON_HOLD.SPECIALS.cost.ToString() + " Focus \n \n";
                Text += "Description: \n";
                Text += BUTTON_HOLD.SPECIALS.Description;

                StopAllCoroutines();
                StartCoroutine(StartTimer());
            }
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(INFO.wait_time);

        INFO.INSTANT_SHOW(Text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ON_UI = false;
        BUTTON_HOLD = null;
        StopAllCoroutines();
        INFO.HIDE();
        Debug.Log("NO MORE!");
    }

    private void OnMouseExit()
    {
        BUTTON_HOLD = null;
        INFO.HIDE();
    }

    public void ButtonClicked()
    {
        ON_UI = false;
        BUTTON_HOLD = null;
        INFO.HIDE();
    }
}
