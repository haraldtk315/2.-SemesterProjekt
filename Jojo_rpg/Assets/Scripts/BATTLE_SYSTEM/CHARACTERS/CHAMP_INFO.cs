using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CHAMP_INFO : MonoBehaviour
{
    //Animator
    public Animator ANI;
    const string IDLE = "IDLE";
    const string DEAD = "DEAD";
    const string ATTACK = "ATTACK";
    const string MISS = "MISS";

    //Sprites
    public SpriteRenderer SR;
    public GameObject TARGETINDICATOR;
    public Collider Collider;
    public ParticleSystem PAR;

    //Audio
    public AudioSource AUD;
    public AudioClip[] Hitsounds;

    //MANAGERS
    public GAMEMANAGER GM;
    public BATTLEHANDLER BH;

    public string Name;
    public int MaxHp;
    public int MaxFocus;
    public int Level;
    public int Party_order;
    public bool Team_player;
    public bool dead = false;

    public int hp;
    public int focus;

    public float time;

    public BASIC_ATTACKS[] ATTACKS;
    public SPECIALS[] SPECIALS;

    public GameObject On_hit_effekt;
    public GameObject Player_UI;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        BH = GameObject.FindGameObjectWithTag("BH").GetComponent<BATTLEHANDLER>();
        ANI = GetComponentInChildren<Animator>();
        SR = GetComponentInChildren<SpriteRenderer>();
        PAR = GetComponentInChildren<ParticleSystem>();
        AUD = GetComponent<AudioSource>();

        //UI
        On_hit_effekt = BH.On_hit_text;
    }
   
    
    
    private void Update()
    {
        ON_HIT(); // I think everything will still work without it, but it is much safer to check everyframe since it does not take that much processing power.

        time += Time.deltaTime / 2f; 
    }

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");

        BH.TARGET_CLICKED(this.gameObject);
    }

    public void Item_used()
    {
        
    }

    public void ON_HIT()
    {
        if (Team_player == true)
        {
            UI_UDATE();
        }

        if (hp <= 0)
        {
            dead = true;
            SR.enabled = false;
        }
    }

    public void UI_UDATE()
    {
        //Name
        Player_UI.GetComponent<PLAYER_UI>().NAME.text = Name;

        //Health sliders
        Player_UI.GetComponent<PLAYER_UI>().HP_SLIDE.maxValue = MaxHp;
        Player_UI.GetComponent<PLAYER_UI>().HP_SLIDE.minValue = 0;
        Player_UI.GetComponent<PLAYER_UI>().HP_SLIDE.value = hp;

        Player_UI.GetComponent<PLAYER_UI>().FILL_SLIDER_HP.color = Player_UI.GetComponent<PLAYER_UI>().grad_hp.Evaluate((float) hp / (float) MaxHp);

        //health text
        Player_UI.GetComponent<PLAYER_UI>().HP_TEXT.text = hp.ToString() + "/" + MaxHp.ToString();

        //Focus Sliders
        Player_UI.GetComponent<PLAYER_UI>().FOCUS_SLIDE.maxValue = MaxFocus;
        Player_UI.GetComponent<PLAYER_UI>().FOCUS_SLIDE.minValue = 0;
        Player_UI.GetComponent<PLAYER_UI>().FOCUS_SLIDE.value = focus;

        //rainbow focus (Maybe not worth it)
        if (Player_UI.GetComponent<PLAYER_UI>().FOCUS_SLIDE.value >= Player_UI.GetComponent<PLAYER_UI>().FOCUS_SLIDE.maxValue)
        {
            if (time >= 1)
            {
                time = 0;
            }

            Player_UI.GetComponent<PLAYER_UI>().FILL_SLIDER_FOCUS.color = Player_UI.GetComponent<PLAYER_UI>().Grad_focus.Evaluate(time);
        }

        if (dead == true)
        {
            Player_UI.GetComponent<PLAYER_UI>().DEATH_SCREEN.SetActive(true);
        }
        else
        {
            Player_UI.GetComponent<PLAYER_UI>().DEATH_SCREEN.SetActive(false);
        }

    }

    //Basic ATTACK animation
    public void NORMAL_HIT(GameObject TARGET, int Damage = 0)
    {
        ANI.Play(ATTACK);

        int random = Random.Range(0, Hitsounds.Length);
        AUD.clip = Hitsounds[random];
        AUD.volume = (float)((Damage + 10) * 2.5f) / 100;
        AUD.Play();

        if (TARGET.GetComponent<CHAMP_INFO>().PAR != null) //Only here just incase the TARGET does not have a particle effekt.
        {
            TARGET.GetComponent<CHAMP_INFO>().PAR.Play();
            GameObject hit_effekt = Instantiate(On_hit_effekt, TARGET.transform.position, Quaternion.identity);
            hit_effekt.GetComponent<Text_hit_effekt>().hit(Damage, TARGET.GetComponent<CHAMP_INFO>().MaxHp <= Damage);
        }
    }

    //Basic MISS animation
    public void MISS_ATTACK()
    {
        ANI.Play(MISS);

        GameObject hit_effekt_miss = Instantiate(On_hit_effekt, transform.position, Quaternion.identity);
        hit_effekt_miss.GetComponent<Text_hit_effekt>().Miss();
    }

    public void SELF_BUFF()
    {

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
