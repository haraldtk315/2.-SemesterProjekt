using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CHAMP_INFO : MonoBehaviour
{
    public Animator ANI;
    const string IDLE = "IDLE";
    const string DEAD = "DEAD";
    const string ATTACK = "ATTACK";
    const string MISS = "MISS";

    public SpriteRenderer SR;
    public GameObject TARGETINDICATOR;
    public Collider Collider;
    public ParticleSystem PAR;

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
        ANI = GetComponentInChildren<Animator>();
        SR = GetComponentInChildren<SpriteRenderer>();
        PAR = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        ON_HIT(); // I think everything will still work without it, but it is much safer to check everyframe since it does not take that much processing power.
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
            SR.enabled = false;
        }
    }


    //Basic ATTACK animation
    public void NORMAL_HIT(GameObject TARGET)
    {
        ANI.Play(ATTACK);

        if (TARGET.GetComponent<CHAMP_INFO>().PAR != null) //Only here just incase the TARGET does not have a particle effekt.
        {
            TARGET.GetComponent<CHAMP_INFO>().PAR.Play();
        }
    }

    //Basic MISS animation
    public void MISS_ATTACK()
    {
        ANI.Play(MISS);
    }


    //Never used. It is a method that checks if the charactor is in idle animation. I made it while trying to figure out how to show animations without it going further in the statemachine.
    public bool IsIDLE()
    {
        if (ANI.GetCurrentAnimatorStateInfo(0).IsName(IDLE))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //DECIDED ON USING THE LENGTH OF THE ANIMATION TO CALL AN INVOKE instead of IsIDLE.
    public float GET_CURRENT_ANIMATION_LENGTH()
    {
        return ANI.GetCurrentAnimatorStateInfo(0).length;
    }
}
