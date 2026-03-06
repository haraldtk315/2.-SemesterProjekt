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

    private int hp;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        BH = GameObject.FindGameObjectWithTag("BH").GetComponent<BATTLEHANDLER>();
    }

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");
    }
}
