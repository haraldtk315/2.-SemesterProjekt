using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BATTLEHANDLER : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject[] SPAWNS;
    [SerializeField] private GameObject[] SPAWNS_ENEMY;

    //GAMEMANAGER
    public GAMEMANAGER GM;

    //Cam
    public Camera Cam;
    public GameObject Cam_holder;
    public Animator Cam_ani;

    public float x_value = 0;
    public float y_value = 0;
    public float z_value = 0;

    public RawImage Texture;

    //BUTTONS
    public GameObject MAIN_Buttons;
    public GameObject SELECT_Buttons;

    public GameObject[] MOVES_BUTTON;

    //PARTY INFORMATION
    private int party_size = 0;
    private GameObject SINGLE_PLAYER;
    public GameObject[] ORDER = {null, null, null, null, null};
    public GameObject[] MONSTER_ORDER = { null, null, null, null, null };

    public int ON_CURRENT_CHAMP = 0;
    public BASIC_ATTACKS Current_ATTACK;
    public SPECIALS Current_SPECIAL;

    //ENEMY INFORMATION
    [SerializeField] private int enemy_count = 0;
    public GameObject only_monster;

    public GameObject TARGET_ENEMY;

    public enum STATEMACHINE
    {
        INPUT,
        SELECT_NORMAL,
        SELECT_SPECIAL,
        TARGET,
        RHYTHM,
        BATTLE,
        NEXT,
    }
    
    //EHM IF THINGS DON'T WORK IT IS BECAUSE IT ALWAYS STARTS AS INPUT!!!!!
    public STATEMACHINE CURRENT_STATE = STATEMACHINE.INPUT;

    private void Start()
    {
        MAIN_Buttons.SetActive(false);

        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Cam_ani = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();

        SpawnCharactors();
        Invoke("RemoveText", 1.75f);
    }

    private void RemoveText()
    {
        Texture.enabled = false;
        Invoke("START_STATEMACHINE", 2f);
    }

    private void START_STATEMACHINE()
    {
        for (int i = 0; i < ORDER.Length; i++)
        {
            if (ORDER[i] == null)
            {
                continue;
            }

            if (ORDER[i] != null)
            {
                ON_CURRENT_CHAMP = i;
                break;
            }
        }

        StateMachine(CURRENT_STATE);
    }

    private void SpawnCharactors()
    {
        //TEAM SPAWNS
        for (int i = 0; i < GM.party.Length; i++)
        {
            if (GM.party[i] == null)
            {
                continue;
            }

            if (GM.party[i] != null)
            {
                party_size++;
                SINGLE_PLAYER = GM.party[i];
            }
        }

        for (int i = 0; i < GM.party.Length; i++)
        {
            if (GM.party[i] == null)
            {
                continue;
            }

            InstantiateChamp(i, true);
        }

        //ENEMY SPAWNS
        enemy_count = Random.Range(1, 6);

        for (int i = 0; i < enemy_count; i++)
        {
            if (enemy_count == 1)
            {
                InstantiateChamp(4, false);
                break;
            }

            InstantiateChamp(i, false);
        }
    }

    private void InstantiateChamp(int position_spawn, bool Player)
    {
        //player charactors = true
        if (Player)
        {
            if (party_size == 1)
            {
                GameObject Ally = Instantiate(SINGLE_PLAYER, SPAWNS[4].transform.position, Quaternion.identity);

                ORDER[0] = Ally;
            }
            else
            {
                GameObject Ally = Instantiate(GM.party[position_spawn], SPAWNS[position_spawn].transform.position, Quaternion.identity);
                Ally.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
                Ally.GetComponent<CHAMP_INFO>().Team_player = true;

                ORDER[position_spawn] = Ally;
            }
        }


        //Enemy charactors = false
        if (!Player)
        {
            GameObject monster = Instantiate(only_monster, SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
            monster.transform.eulerAngles = new Vector3(0, 180, 0);
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;

            MONSTER_ORDER[position_spawn] = monster;
        }
    }


    void StateMachine(STATEMACHINE Current)
    {
        //AWAITING INPUT FROM PLAYER
        if (Current == STATEMACHINE.INPUT)
        {
            MAIN_Buttons.SetActive(true);
            Cam_holder.transform.position = new Vector3(ORDER[ON_CURRENT_CHAMP].transform.position.x + x_value, ORDER[ON_CURRENT_CHAMP].transform.position.y - y_value, ORDER[ON_CURRENT_CHAMP].transform.position.z + z_value);
        }

        //SELECT ATTACK MOVES
        if (Current == STATEMACHINE.SELECT_NORMAL)
        {
            SELECT_Buttons.SetActive(true);

            for (int i = 0; i < MOVES_BUTTON.Length; i++)
            {
                if (MOVES_BUTTON[i] == null)
                {
                    Debug.Log("THERE IS NO BUTTON");
                    continue;
                }

                if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i] == null)
                {
                    MOVES_BUTTON[i].SetActive(false);
                    continue;
                }

                //Takes the script on the button and ands the attack into the button script, so that we can use that information later
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().ATTACK = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i];
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().SPECIALS = null;

                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].name + "\n" + " DAMAGE: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].damage.ToString();
                MOVES_BUTTON[i].SetActive(true);
            }
        }

        //SELECT SPECIAL MOVES
        if (Current == STATEMACHINE.SELECT_SPECIAL) 
        {
            SELECT_Buttons.SetActive(true);

            for (int i = 0; i < MOVES_BUTTON.Length; i++)
            {
                if (MOVES_BUTTON[i] == null)
                {
                    Debug.Log("THERE IS NO BUTTON");
                    continue;
                }

                if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i] == null)
                {
                    MOVES_BUTTON[i].SetActive(false);
                    continue;
                }

                //Takes the script on the button and ands the attack into the button script, so that we can use that information later
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().SPECIALS = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i];
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().ATTACK = null;

                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].name + "\n" + " DAMAGE: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].damage.ToString();
                MOVES_BUTTON[i].SetActive(true);
            }
        }

        //TARGET A MONSTER TO ATTACK
        if (Current == STATEMACHINE.TARGET)
        {
            MAIN_Buttons.SetActive(false);
            SELECT_Buttons.SetActive(false);
            Cam_holder.transform.position = Vector3.zero;

            for (int i = 0; i < MONSTER_ORDER.Length; i++)
            {
                if (MONSTER_ORDER[i] == null)
                {
                    continue;
                }

                if (MONSTER_ORDER[i] != null)
                {
                    MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);
                    
                }
            }
        }

        //DAMAGE HAPPENDS
        if (Current == STATEMACHINE.BATTLE)
        {
            for (int i = 0; i < MONSTER_ORDER.Length; i++)
            {
                if (MONSTER_ORDER[i] == null)
                {
                    continue;
                }

                if (MONSTER_ORDER[i] != null)
                {
                    MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(false);
                }
            }

            TARGET_ENEMY.GetComponent<CHAMP_INFO>().hp -= Current_ATTACK.damage;
            TARGET_ENEMY.GetComponent<CHAMP_INFO>().ON_HIT();

            Debug.Log("IN BATTLE");

            CURRENT_STATE = STATEMACHINE.NEXT;
            StateMachine(STATEMACHINE.NEXT);
        }

        if (Current == STATEMACHINE.NEXT)
        {
            for (int i = ON_CURRENT_CHAMP + 1; i < ORDER.Length; i++)
            {
                if (ORDER[i] == null)
                {
                    continue;
                }

                if (ORDER[i] != null)
                {
                    ON_CURRENT_CHAMP = i;
                    break;
                }
            }

            CURRENT_STATE = STATEMACHINE.INPUT;
            StateMachine(STATEMACHINE.INPUT);
        }
    }

    public void TARGET_CLICKED(GameObject Target)
    {
        TARGET_ENEMY = Target;

        if (CURRENT_STATE == STATEMACHINE.TARGET)
        {
            CURRENT_STATE = STATEMACHINE.BATTLE;
            StateMachine(STATEMACHINE.BATTLE);
        }
    }

    //buttons
    public void ATTACK()
    {
        CURRENT_STATE = STATEMACHINE.SELECT_NORMAL;
        StateMachine(CURRENT_STATE);
    }

    public void SPECIAL()
    {
        CURRENT_STATE = STATEMACHINE.SELECT_SPECIAL;
        StateMachine(CURRENT_STATE);
    }

    public void ITEM()
    {

    }

    public void RUN()
    {

    }

    public void MOVESELECT(GameObject button)
    {
       if (button.GetComponent<BUTTON_HOLDER>().ATTACK != null)
       {
            Current_ATTACK = button.GetComponent<BUTTON_HOLDER>().ATTACK;
       }

       if (button.GetComponent<BUTTON_HOLDER>().SPECIALS != null)
       {
            Current_SPECIAL = button.GetComponent<BUTTON_HOLDER>().SPECIALS;
       }

        CURRENT_STATE = STATEMACHINE.TARGET;
        StateMachine(CURRENT_STATE);
    }

    public void BACK()
    {
        SELECT_Buttons.SetActive(false);
        CURRENT_STATE = STATEMACHINE.INPUT;
        StateMachine(CURRENT_STATE);
    }
}
