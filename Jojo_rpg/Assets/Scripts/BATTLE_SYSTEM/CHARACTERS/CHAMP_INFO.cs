using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CHAMP_INFO : MonoBehaviour
{
    //Animator
    public Animator ANI;
    const string IDLE = "IDLE";
    const string DEAD = "DEAD"; //This is not used actually
    const string ATTACK = "ATTACK";
    const string MISS = "MISS";
    const string HEAL = "HEAL";
    const string FOCUS = "FOCUS";
    const string DEATH = "DEATH";
    const string BUFF = "BUFF";

    //Sprites
    public SpriteRenderer SR;
    public GameObject TARGETINDICATOR;
    public Collider Collider;
    public ParticleSystem PAR;

    //Audio
    public AudioSource AUD;
    public AudioClip[] Hitsounds;
    public AudioClip Healing;

    //MANAGERS
    public GAMEMANAGER GM;
    public BATTLEHANDLER BH;

    public string champID;
    public string Name;
    public int MaxHp;
    public int MaxFocus;
    public int Level;
    public string descripton;
    public int Party_order;
    public bool Team_player;
    public bool dead = false;

    public int hp;
    public int focus;
    public float Damage_buff = 1;
    public int Shield_buff = 1;

    public float height_from_ground;
    public float height_cam;

    public float time;

    public BASIC_ATTACKS[] ATTACKS;
    public SPECIALS[] SPECIALS;

    public GameObject On_hit_effekt;
    public GameObject Player_UI;

    public GameObject Outline;
    public GameObject[] Out_sprites;

    public GameObject BUFF_INDICATOR;

    private float heal_pitch = 0.75f;
    private float focus_pitch = 0.6f;

    public string[] Talk_each_round;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        BH = FindAnyObjectByType<BATTLEHANDLER>();
        ANI = GetComponentInChildren<Animator>();
        SR = GetComponentInChildren<SpriteRenderer>();
        PAR = GetComponentInChildren<ParticleSystem>();
        AUD = GetComponent<AudioSource>();

        //UI
        On_hit_effekt = BH.On_hit_text;

        for (int i = 0; i < Out_sprites.Length; i++)
        {
            Out_sprites[i].GetComponent<SpriteRenderer>().sprite = SR.sprite;
        }
    }
   
    
    
    private void Update()
    {
        ON_HIT(); // I think everything will still work without it, but it is much safer to check everyframe since it does not take that much processing power.

        time += Time.deltaTime / 2f; 

        if (Damage_buff > 1)
        {
            BUFF_INDICATOR.SetActive(true);
            BUFF_INDICATOR.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            BUFF_INDICATOR.SetActive(false);
        }

        //Is here to fix a bug were you can heal a unit that is dead and make their sprite turn into the heal sprite
        if (dead == true)
        {
            ANI.Play(DEATH);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("CLICKED");

        BH.TARGET_CLICKED(this.gameObject);
    }

    private void OnMouseOver()
    {
        if (BH.CURRENT_STATE == BATTLEHANDLER.STATEMACHINE.ITEM_SELECT || BH.CURRENT_STATE == BATTLEHANDLER.STATEMACHINE.TARGET)
        {
            Debug.Log("Mouse over unit");

            Outline.SetActive(true);
        }
        else
        {
            Outline.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        Outline.SetActive(false);
    }

    public void Item_used(InventoryItem Item)
    {
        if (Item.itemData.Type == ItemData.ItemType.health)
        {
            PlaySound(Healing, heal_pitch);

            ANI.Play(HEAL);

            hp += Item.itemData.value;
            GameObject hit_effekt = Instantiate(On_hit_effekt, transform.position, Quaternion.identity);
            hit_effekt.GetComponent<Text_hit_effekt>().hit(Item.itemData.value, false, true);

            if (hp >= MaxHp)
            {
                hp = MaxHp;
            }
        }

        if (Item.itemData.Type == ItemData.ItemType.focus)
        {
            SELF_BUFF();

            focus += Item.itemData.value;
            GameObject hit_effekt = Instantiate(On_hit_effekt, transform.position, Quaternion.identity);
            hit_effekt.GetComponent<Text_hit_effekt>().hit(Item.itemData.value, false, false, true);

            if (focus >= MaxFocus)
            {
                focus = MaxFocus;
            }
        }

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
            ANI.Play(DEATH);

            for (int i = 0; i < Out_sprites.Length; i++)
            {
                Name = "Pile of bones";
                descripton = "Not even a trace of flesh remain on these bones.\nRest in peace";
                Out_sprites[i].GetComponent<SpriteRenderer>().sprite = SR.sprite;
            }

            if (hp <= -MaxHp)
            {
                Destroy(this.gameObject);
            }
        }

        if (hp > MaxHp)
        {
            hp = MaxHp;
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

        if (Damage < 0)
        {
            TARGET.GetComponent<CHAMP_INFO>().PlaySound(Healing, heal_pitch);
            TARGET.GetComponent<CHAMP_INFO>().ANI.Play(HEAL);

            GameObject hit_effekt = Instantiate(On_hit_effekt, TARGET.transform.position, Quaternion.identity);
            hit_effekt.GetComponent<Text_hit_effekt>().hit(-Damage, false, true);
        }

        if (Damage >= 0) 
        {
            ANI.Play(ATTACK);

            int random = Random.Range(0, Hitsounds.Length);
            PlaySound(Hitsounds[random], 1, (float)((Damage + 10) * 2.5f) / 100);

            if (TARGET.GetComponent<CHAMP_INFO>().PAR != null) //Only here just incase the TARGET does not have a particle effekt.
            {
                TARGET.GetComponent<CHAMP_INFO>().PAR.Play();
                GameObject hit_effekt = Instantiate(On_hit_effekt, TARGET.transform.position, Quaternion.identity);
                hit_effekt.GetComponent<Text_hit_effekt>().hit(Damage, TARGET.GetComponent<CHAMP_INFO>().MaxHp <= Damage);
            }
        }
    }

    //Basic MISS animation
    public void MISS_ATTACK()
    {
        ANI.Play(MISS);

        GameObject hit_effekt_miss = Instantiate(On_hit_effekt, transform.position, Quaternion.identity);
        hit_effekt_miss.GetComponent<Text_hit_effekt>().Miss();
    }

    public void SELF_BUFF(int BuffType = 0)
    {
        PlaySound(Healing, focus_pitch);
        if (BuffType == 0)
        {
            ANI.Play(FOCUS);
        }
        
        if (BuffType == 1)
        {
            ANI.Play(BUFF);
        }
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

    public void PlaySound(AudioClip Clip, float pitch = 1, float volume = 0.75f)
    {
        AUD.pitch = pitch;
        AUD.volume = volume;
        AUD.clip = Clip;
        AUD.Play();
    }
}
