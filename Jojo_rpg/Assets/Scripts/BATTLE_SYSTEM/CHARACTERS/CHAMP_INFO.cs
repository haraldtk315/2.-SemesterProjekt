using Unity.VisualScripting;
using UnityEngine;

public class CHAMP_INFO : MonoBehaviour
{
    public Animator ANI;
    public GameObject TARGETINDICATOR;
    public Collider Collider;

    //MANAGERS
    public GAMEMANAGER GM;
    public BATTLEHANDLER BH;

    public string Name;
    public int MaxHp;
    public int Level;
    public int Party_order;
    public bool Team_player;
    public bool dead = false;

    public int hp;

    public BASIC_ATTACKS[] ATTACKS;
    public SPECIALS[] SPECIALS;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        BH = GameObject.FindGameObjectWithTag("BH").GetComponent<BATTLEHANDLER>();

    }

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");

        BH.TARGET_CLICKED(this.gameObject);
    }

    public void ON_HIT()
    {
        if (hp <= 0)
        {
            dead = true;
            
        }
    }
}
