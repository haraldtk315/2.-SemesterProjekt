using NUnit.Framework.Internal;
using System.Linq.Expressions;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BATTLEHANDLER : MonoBehaviour
{
    public static BATTLEHANDLER instance;

    [SerializeField] private Transform center;
    [SerializeField] private GameObject[] SPAWNS;
    [SerializeField] private GameObject[] SPAWNS_ENEMY;

    //GAMEMANAGER
    public GAMEMANAGER GM;

    //DIALOGUEHANDLER
    public DIALOGUEHANDLER DH;

    //MICROGAMEHANDLER
    public MICROGAMEHANDLER MH;

    //Cam
    public Camera Cam;
    public GameObject Cam_holder;
    public Animator Cam_ani;

    public float x_value = 0;
    public float y_value = 0;
    public float z_value = 0;

    public RawImage Texture;
    public GameObject RENDER_CAM;

    public GameObject[] ICON_CAMS_UI;

    //BUTTONS
    public GameObject MAIN_Buttons;
    public GameObject SELECT_Buttons;

    public GameObject ITEM_PANEL;
    public bool Item_enabled = false;

    public GameObject[] MOVES_BUTTON;

    //PARTY INFORMATION
    private int party_size = 0;
    private GameObject SINGLE_PLAYER;
    public GameObject[] ORDER = {null, null, null, null, null};
    public GameObject[] MONSTER_ORDER = { null, null, null, null, null };

    public int ON_CURRENT_CHAMP = 0;
    public BASIC_ATTACKS Current_ATTACK;
    public SPECIALS Current_SPECIAL;
    public InventoryItem Current_ITEM;


    //ENEMY INFORMATION
    [SerializeField] private int enemy_count = 0;
    public GameObject only_monster;

    public GameObject TARGET_ENEMY;
    public GameObject[] Target_enemies;
    public BASIC_ATTACKS[] ENEMY_MOVES;
    public int Enemy_Attack;
    public int ON_TARGET_ENEMY = 0;

    public int CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE; //Major problems with animations on zombies since it is done insinde a for loop so we need a seperate state for the animation/attacks of monsters
    private bool forced_combat = false;

    private bool EXPANDED = false; //for quickly changing size in the input state
    private string[] Current_dial; //To store information

    //STATEMACHINE

    float WAIT_TIME; //Used if we need small breaks between the states.
    float Extra_time = 1; //Used since it is quite akward to end an animation immidiate

    public enum STATEMACHINE
    {
        INPUT,
        SELECT_NORMAL,
        SELECT_SPECIAL,
        TARGET,
        ITEM_SELECT,
        MICROGAME,
        BATTLE,
        NEXT,
        ENEMY,
        ENEMY_BATTLE, //THE ENEMY STATE ALONE WAS NOT ENOUGH TO MAKE SURE THE ENEMIES COULD HAVE THEIR OWN ANIMATIONS AND STUFF LIKE THAT SO WE NEED A LITTLE EXTRA STEP FOR ANIMATIONS
        END,
        WAITING,
        DIALOGUE
    }
    
    //EHM IF THINGS DON'T WORK IT IS BECAUSE IT ALWAYS STARTS AS INPUT!!!!!
    public STATEMACHINE CURRENT_STATE = STATEMACHINE.INPUT;

    //Okay so like... This is actually for the instaniated players that they can access it.
    public GameObject On_hit_text;
    public GameObject Player_UI;
    public GameObject[] HEALTH_UI_FOR_PLAYERS;

    private void Start()
    {

        MAIN_Buttons.SetActive(false);
        Player_UI.SetActive(false);


        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        DH = GameObject.FindGameObjectWithTag("DH").GetComponent<DIALOGUEHANDLER>();
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Cam_ani = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();

        SpawnCharactors();
        Invoke("RemoveText", 1.75f);
    }

    private void RemoveText()
    {
        Texture.enabled = false;
        RENDER_CAM.SetActive(false);
        Invoke("START_STATEMACHINE", 2f);
    }

    private void START_STATEMACHINE()
    {
        Debug.Log("STATEMACHINE HAS STARTED");

        for (int i = 0; i < ORDER.Length; i++)
        {
            if (ORDER[i] == null || ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
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
        forced_combat = false;
        enemy_count = 0;
        for (int i = 0; i < DH.ENEMIES.Length; i++)
        {
            if (DH.ENEMIES[i] == null)
            {
                continue;
            }

            if (DH.ENEMIES[i] != null)
            {
                forced_combat = true;
                enemy_count++;
            }
        }

        if (forced_combat == false)
        {
            Debug.Log("RANDOM ENCOUNTER");
            enemy_count = Random.Range(1, 6);
        }

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
                Ally.transform.position += new Vector3(0, Ally.GetComponent<CHAMP_INFO>().height_from_ground, 0);

                //Kinda messed up here but it should work ;3
                ORDER[0] = Ally;
                HEALTH_UI_FOR_PLAYERS[0].SetActive(true);
                Ally.GetComponent<CHAMP_INFO>().Player_UI = HEALTH_UI_FOR_PLAYERS[0];
                Ally.GetComponent<CHAMP_INFO>().Team_player = true;
                Ally.GetComponent<CHAMP_INFO>().hp = GM.HP[0];

                //ICON
                Vector3 Pos = new Vector3(Ally.transform.position.x, Ally.transform.position.y + Ally.GetComponent<CHAMP_INFO>().height_cam, Ally.transform.position.z - 0.50f);
                GameObject ICON_CAM_OBJECT = Instantiate(ICON_CAMS_UI[position_spawn], Pos, Quaternion.identity);
            }
            else
            {
                GameObject Ally = Instantiate(GM.party[position_spawn], SPAWNS[position_spawn].transform.position, Quaternion.identity);
                Ally.transform.position += new Vector3(0, Ally.GetComponent<CHAMP_INFO>().height_from_ground, 0);

                Ally.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
                Ally.GetComponent<CHAMP_INFO>().Team_player = true;
                Ally.GetComponent<CHAMP_INFO>().hp = GM.HP[position_spawn];

                //player health and focus UI
                HEALTH_UI_FOR_PLAYERS[position_spawn].SetActive(true);
                Ally.GetComponent<CHAMP_INFO>().Player_UI = HEALTH_UI_FOR_PLAYERS[position_spawn];

                ORDER[position_spawn] = Ally;

                //ICON
                Vector3 Pos = new Vector3(Ally.transform.position.x, Ally.transform.position.y + Ally.GetComponent<CHAMP_INFO>().height_cam, Ally.transform.position.z - 0.50f);
                GameObject ICON_CAM_OBJECT = Instantiate(ICON_CAMS_UI[position_spawn], Pos, Quaternion.identity);
            }
        }

        //Enemy charactors = false
        if (!Player && forced_combat == false)
        {
            GameObject monster = Instantiate(only_monster, SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
            monster.transform.position += new Vector3(0, monster.GetComponent<CHAMP_INFO>().height_from_ground, 0);
            monster.transform.eulerAngles = new Vector3(0, 180, 0);
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;

            MONSTER_ORDER[position_spawn] = monster;
        }

        //Forced_combat moment
        if (forced_combat == true && !Player && enemy_count == 1)
        {
            GameObject monster = Instantiate(DH.ENEMIES[0], SPAWNS_ENEMY[4].transform.position, Quaternion.identity);
            monster.transform.position += new Vector3(0, monster.GetComponent<CHAMP_INFO>().height_from_ground, 0);
            monster.transform.eulerAngles = new Vector3(0, 180, 0);
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;

            MONSTER_ORDER[position_spawn] = monster;
        }
        else if (!Player && forced_combat == true)
        {
            GameObject monster = Instantiate(DH.ENEMIES[position_spawn], SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
            monster.transform.position += new Vector3(0, monster.GetComponent<CHAMP_INFO>().height_from_ground, 0);
            monster.transform.eulerAngles = new Vector3(0, 180, 0);
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;

            MONSTER_ORDER[position_spawn] = monster;
        }
    }


    public void StateMachine(STATEMACHINE Current)
    {
        if (Current == STATEMACHINE.DIALOGUE)
        {
            CURRENT_STATE = STATEMACHINE.INPUT;
            StateMachine(STATEMACHINE.INPUT);
        }

        //AWAITING INPUT FROM PLAYER
        if (Current == STATEMACHINE.INPUT)
        {
            DIAL();

            for (int i = ON_CURRENT_CHAMP; i < ORDER.Length; i++)
            {
                if (ORDER[ON_CURRENT_CHAMP] == null || ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().dead == true)
                {
                    continue;
                }

                if (ORDER[ON_CURRENT_CHAMP] != null || ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().dead == false)
                {
                    ON_CURRENT_CHAMP = i;
                    break;
                }
            }

            MAIN_Buttons.SetActive(true);
            Player_UI.SetActive(true);
            EXPANDED = false;
            Cam_holder.transform.position = new Vector3(ORDER[ON_CURRENT_CHAMP].transform.position.x + x_value, ORDER[ON_CURRENT_CHAMP].transform.position.y - y_value, ORDER[ON_CURRENT_CHAMP].transform.position.z + z_value);
            ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);

            bool done = true;

            for (int i = 0; i < MONSTER_ORDER.Length; i++)
            {
                if (MONSTER_ORDER[i] != null)
                {
                    if (MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == false)
                    {
                        done = false;
                    }
                }
            }

            if (done == true)
            {
                CURRENT_STATE = STATEMACHINE.END;
                StateMachine(STATEMACHINE.END);
            }
        }

        //SELECT ATTACK MOVES
        if (Current == STATEMACHINE.SELECT_NORMAL)
        {
            SELECT_Buttons.SetActive(true);
            Player_UI.SetActive(false);

            for (int i = 0; i < MOVES_BUTTON.Length; i++)
            {
                MOVES_BUTTON[i].GetComponent<Button>().enabled = true;

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

                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].attackName + "\n";

                if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].damage > 0)
                {
                    MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += " DAMAGE: " + ((int)((float)ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].damage * ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().Damage_buff)).ToString();
                }
                else
                {
                    MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += " FOCUS: " + ((int)((float)ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].focus * ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().Damage_buff)).ToString();
                }

                if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].acc < 100)
                {
                    MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += "  |  " + "ACC: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].acc + "%";
                }
                else if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].damage > 0 && ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].focus > 0)
                {
                    MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += "  |  " + "FOCUS: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].focus;
                }
                
                MOVES_BUTTON[i].SetActive(true);
            }
        }

        //SELECT SPECIAL MOVES
        if (Current == STATEMACHINE.SELECT_SPECIAL)
        {
            SELECT_Buttons.SetActive(true);
            Player_UI.SetActive(false);

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

                if (ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].cost <= ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().focus)
                {
                    MOVES_BUTTON[i].GetComponent<Button>().enabled = true;
                }
                else
                {
                    MOVES_BUTTON[i].GetComponent<Button>().enabled = false;
                }

                //Takes the script on the button and ands the attack into the button script, so that we can use that information later
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().SPECIALS = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i];
                MOVES_BUTTON[i].GetComponent<BUTTON_HOLDER>().ATTACK = null;

                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].attackName + "\n" + " DAMAGE: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].damage.ToString();
                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += "  |  " + "COST: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().SPECIALS[i].cost.ToString();
                MOVES_BUTTON[i].SetActive(true);
            }
        }

        if (Current == STATEMACHINE.ITEM_SELECT)
        {
            MAIN_Buttons.SetActive(false);
            SELECT_Buttons.SetActive(false);
            Player_UI.SetActive(true);
            ITEM();
            

            Cam_holder.transform.position = Vector3.zero;
            ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(false);

            for (int i = 0; i < ORDER.Length; i++)
            {
                if (ORDER[i] == null || ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                {
                    continue;
                }

                if (ORDER[i] != null)
                {
                    ORDER[i].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);
                }
            }
        }

        //TARGET A MONSTER TO ATTACK
        if (Current == STATEMACHINE.TARGET)
        {
            //Important for player view
            MAIN_Buttons.SetActive(false);
            SELECT_Buttons.SetActive(false);
            Player_UI.SetActive(true);

            Cam_holder.transform.position = Vector3.zero;
            ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(false);

            //ATTACK IS SELF TARGET
            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SELF)
            {
                TARGET_ENEMY = ORDER[ON_CURRENT_CHAMP];

                CURRENT_STATE = STATEMACHINE.BATTLE;
                StateMachine(STATEMACHINE.BATTLE);
            }

            //ATTACK IS SINGLE TARGET
            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SINGLE_HIT)
            {
                for (int i = 0; i < MONSTER_ORDER.Length; i++)
                {
                    if (MONSTER_ORDER[i] == null || MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                    {
                        continue;
                    }

                    if (MONSTER_ORDER[i] != null)
                    {
                        MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);

                    }
                }
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.Party)
            {
                for (int i = 0; i < ORDER.Length; i++)
                {
                    if (ORDER[i] == null || ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                    {
                        continue;
                    }

                    if (ORDER[i] != null)
                    {
                        ORDER[i].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);

                    }
                }
            }

            //ATTACK HITS ALL
            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL)
            {
                TARGET_ENEMY = null;

                TARGET_CLICKED(null);
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL_PARTY)
            {
                TARGET_ENEMY = null;

                TARGET_CLICKED(null);
            }
        }

        if (Current == STATEMACHINE.MICROGAME)
        {
            SPECIALS currentSpecial = (SPECIALS)Current_ATTACK;
            MH.StartMicrogame(currentSpecial.microgame);
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

            Debug.Log("IN BATTLE");

            //THE ATTACK + ANIMATION
            if (Current_ATTACK is SPECIALS)
            {
                // Empty because else is what's needed here
            }
            else
            {
                TARGET_ATTACK(ORDER[ON_CURRENT_CHAMP], TARGET_ENEMY, Current_ATTACK.damage, Current_ATTACK.acc, Current_ATTACK.focus, Current_ATTACK.damage_buff);
            }


                Invoke("STATEGOTONEXT", WAIT_TIME);
        }

        if (Current == STATEMACHINE.NEXT)
        {
            if (ON_CURRENT_CHAMP + 1 >= ORDER.Length)
            {
                ON_TARGET_ENEMY = 0;
                CURRENT_STATE = STATEMACHINE.ENEMY;
                StateMachine(STATEMACHINE.ENEMY);
            }
            else
            {
                for (int i = ON_CURRENT_CHAMP + 1; i < ORDER.Length; i++)
                {
                    if (ORDER[i] == null || ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                    {
                        ON_CURRENT_CHAMP++;
                        CURRENT_STATE = STATEMACHINE.NEXT;
                        StateMachine(STATEMACHINE.NEXT); 
                        break;
                    }

                    if (ORDER[i] != null)
                    {
                        ON_CURRENT_CHAMP = i;
                        CURRENT_STATE = STATEMACHINE.INPUT;
                        StateMachine(STATEMACHINE.INPUT);
                        break;
                    }
                }
            }
        }

        if (Current == STATEMACHINE.ENEMY)
        {
            if (ON_TARGET_ENEMY >= MONSTER_ORDER.Length)
            {
                ON_CURRENT_CHAMP = 0;
                CURRENT_STATE = STATEMACHINE.END;
                StateMachine(STATEMACHINE.END);
            }
            else 
            {
                for (int i = ON_TARGET_ENEMY; i < MONSTER_ORDER.Length; i++)
                {
                    if (MONSTER_ORDER[i] == null || MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                    {
                        ON_TARGET_ENEMY++;
                        CURRENT_STATE = STATEMACHINE.ENEMY;
                        StateMachine(STATEMACHINE.ENEMY);
                        break;
                    }

                    ENEMY_MOVES = MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().ATTACKS;

                    Enemy_Attack = Random.Range(0, ENEMY_MOVES.Length);
                    if (ENEMY_MOVES[Enemy_Attack] == null)
                    {
                        Enemy_Attack = 0;
                    }

                    
                    //ATTACK
                    int amount = 0;
                    for (int k = 0; k < ORDER.Length; k++)
                    {
                        if (ORDER[k] == null || ORDER[k].GetComponent<CHAMP_INFO>().dead == true)
                        {
                            continue;
                        }

                        if (ORDER[k] != null && ORDER[k].GetComponent<CHAMP_INFO>().dead == false)
                        {
                            amount++;
                        }
                    }

                    for (int j = 0; j < ORDER.Length; j++)
                    {
                        if (ORDER[j] == null || ORDER[j].GetComponent<CHAMP_INFO>().dead == true)
                        {
                            continue;
                        }

                        if (ORDER[j] != null && ORDER[j].GetComponent<CHAMP_INFO>().dead == false)
                        {
                            /*
                            //THE ATTACK HAPPENDS
                            ORDER[j].GetComponent<CHAMP_INFO>().hp -= ENEMY_MOVES[Enemy_Attack].damage;
                            ORDER[j].GetComponent<CHAMP_INFO>().ON_HIT(); //TO MAKE SURE THEY UPDATE THEIR BOOLEANS
                            */

                            int random_num = Random.Range(0, amount);

                            if (random_num == 0)
                            {
                                CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE = j;
                            }

                            if (random_num >= 1)
                            {
                                j++;
                                for (int CHECK = j; CHECK < ORDER.Length; CHECK++)
                                {
                                    if (ORDER[CHECK] == null || ORDER[CHECK].GetComponent<CHAMP_INFO>().dead == true)
                                    {
                                        continue;
                                    }

                                    if (ORDER[CHECK] != null && ORDER[CHECK].GetComponent<CHAMP_INFO>().dead == false)
                                    {
                                        random_num--;
                                        if (random_num == 0)
                                        {
                                            CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE = CHECK;
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            
                            //CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE = j;

                            ON_CURRENT_CHAMP = 0; //added this because of an error might not be needed
                            CURRENT_STATE = STATEMACHINE.ENEMY_BATTLE;
                            StateMachine(STATEMACHINE.ENEMY_BATTLE);
                            break;
                        }
                    }

                    // If all the good people are dead ;c
                    ON_TARGET_ENEMY++;
                    CURRENT_STATE = STATEMACHINE.ENEMY;
                    StateMachine(STATEMACHINE.ENEMY);
                    break;
                }
            }
        }

        if (Current == STATEMACHINE.ENEMY_BATTLE)
        {
            Cam_holder.transform.position = Vector3.zero;

            Debug.Log("CURRENTLY IN ENEMY BATTLE STATE");
            Current_ATTACK = ENEMY_MOVES[Enemy_Attack];
            TARGET_ATTACK(MONSTER_ORDER[ON_TARGET_ENEMY], ORDER[CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE], ENEMY_MOVES[Enemy_Attack].damage, ENEMY_MOVES[Enemy_Attack].acc, ENEMY_MOVES[Enemy_Attack].focus, ENEMY_MOVES[Enemy_Attack].damage_buff, ENEMY_MOVES[Enemy_Attack].spawnable);
            Current_ATTACK = null;

            Invoke("STATEGOTOENEMY", WAIT_TIME);
        }

        if (Current == STATEMACHINE.END)
        {
            bool lost = true;

            //CHECKING IF ALL PLAYER CHARACTORS ARE DEAD
            for (int i = 0; i < ORDER.Length; i++)
            {
                if (ORDER[i] != null && ORDER[i].GetComponent<CHAMP_INFO>().dead == false)
                {
                    lost = false;
                }
            }

            bool win = true;

            //CHECKING IF ALL ENEMIES ARE DEAD
            for (int i = 0; i < MONSTER_ORDER.Length; i++)
            {
                if (MONSTER_ORDER[i] != null && MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == false)
                {
                    win = false;
                }
            }
            if (GAMEMANAGER.instance.pendingPartyReward != null)
            {
                GAMEMANAGER.instance.AddPartyMember(GAMEMANAGER.instance.pendingPartyReward);
                GAMEMANAGER.instance.pendingPartyReward = null;
            }

            if (lost == true)
            {
                for (int i = 0; i < GM.HP.Length; i++)
                {
                    if (ORDER[i] != null)
                    {
                        GM.HP[i] = GM.party[i].GetComponent<CHAMP_INFO>().MaxHp;
                        GM.party[i].GetComponent<CHAMP_INFO>().dead = false;
                    }
                }

                SceneManager.LoadScene("TITLE");
            }

            if (win == true)
            {
                for (int i = 0; i < GM.HP.Length; i++)
                {
                    if (ORDER[i] != null)
                    {
                        GM.HP[i] = ORDER[i].GetComponent<CHAMP_INFO>().hp;
                    }
                }

                NewMethod();
            }

            if (win == false && lost == false)
            {
                Debug.Log("GOING BACK TO PLAYER INPUT");
                CancelInvoke(); //I think it is in the ENEMY state where something happends so that we have a function call that gets called twice leading to end state being called twice. This is a workaround solution.
                Invoke("STATEGOTOINPUT", 2f);
            }
        }
    }

    private static void NewMethod()
    {
        if (!string.IsNullOrEmpty(GAMEMANAGER.instance.currentNPCID))
        {
            GAMEMANAGER.instance.defeatedNPCs.Add(GAMEMANAGER.instance.currentNPCID);
            GAMEMANAGER.instance.pendingPostBattleNPCID = GAMEMANAGER.instance.currentNPCID;
            GAMEMANAGER.instance.currentNPCID = null;
        }

        SceneManager.LoadScene(GAMEMANAGER.instance.returnSceneName);
    }

    //ALL THE GOTO STATE METHODS
    private void STATEGOTOENEMY()
    {
        Debug.Log("THE ANIMATION AND ATTACK IS FINISHED");
        ON_TARGET_ENEMY++;
        CURRENT_STATE = STATEMACHINE.ENEMY;
        StateMachine(STATEMACHINE.ENEMY);
    }

    private void STATEGOTONEXT()
    {
        CURRENT_STATE = STATEMACHINE.NEXT;
        StateMachine(STATEMACHINE.NEXT);
    }

    private void STATEGOTOINPUT()
    {
        CURRENT_STATE = STATEMACHINE.INPUT;
        START_STATEMACHINE();
    }

    private void DIAL()
    {
        for (int i = 0; i < MONSTER_ORDER.Length; i++)
        {
            if (MONSTER_ORDER[i] == null)
            {
                continue;
            }

            if (MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().talks == false)
            {
                continue;
            }

            if (MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().On_this_dial < MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().Lines_Combat.Count)
            {
                Current_dial = MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().Lines_Combat[MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().On_this_dial];
            }

            if (Current_dial != null && MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().On_this_dial <= MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().Lines_Combat.Count)
            {
                GameObject talking_enemy = MONSTER_ORDER[i];
                MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().On_this_dial++;
                
                DIALOGUEHANDLER.instance.DialogueStart(
                    Current_dial,
                    talking_enemy,
                    null,
                    null,
                    null,
                    null,
                    true,
                    talking_enemy,
                    null
                    );
            }
            else
            {
                GameObject talking_enemy = MONSTER_ORDER[i];

                DIALOGUEHANDLER.instance.DialogueStart(
                    talking_enemy.GetComponent<CHAMP_INFO>().Random_lines[Random.Range(0, talking_enemy.GetComponent<CHAMP_INFO>().Random_lines.Count)],
                    talking_enemy,
                    null,
                    null,
                    null,
                    null,
                    true,
                    talking_enemy,
                    null
                    );
            }
        }
    }
    //THE BASIC ATTACK (Could potentially also work for the future special move)
    public void TARGET_ATTACK(GameObject SENDER, GameObject TARGET, int Damage, int acc, int focus = 0, float buff = 1f, GameObject SPAWN = null)
    {
        //ZOOM OUT CAM
        Cam_holder.transform.position = Vector3.zero;

        //WILL ATTACK HIT?
        bool attack_HITS = Check_if_attack_lands(acc);

        int New_Damage = (int)((float)Damage * SENDER.GetComponent<CHAMP_INFO>().Damage_buff);
        int New_Focus = (int)((float)focus * SENDER.GetComponent<CHAMP_INFO>().Damage_buff);

        //ANIMATION
        if (attack_HITS == true)
        {
            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SINGLE_HIT || Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.Party) 
            {
                SENDER.GetComponent<CHAMP_INFO>().NORMAL_HIT(TARGET, New_Damage);
                WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

                //DO DAMAGE TO TARGET (Could potentially be moved into the NORMAL_HIT() METHOD)
                TARGET.GetComponent<CHAMP_INFO>().hp -= New_Damage;
                SENDER.GetComponent<CHAMP_INFO>().focus += focus;
                TARGET.GetComponent<CHAMP_INFO>().ON_HIT(); //MAKING SURE THE TARGET IS DEAD!!!
                Debug.Log(TARGET.GetComponent<CHAMP_INFO>().Name + " GOT ATTACKED BY " + SENDER.GetComponent<CHAMP_INFO>().Name + " " + New_Damage.ToString() + " DAMAGE DEALT");
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SELF)
            {
                if (New_Focus > 0)
                {
                    SENDER.GetComponent<CHAMP_INFO>().SELF_BUFF();
                }
                
                if (New_Damage < 0)
                {
                    SENDER.GetComponent<CHAMP_INFO>().SELF_HEAL();
                }

                if (buff > 0)
                {
                    SENDER.GetComponent<CHAMP_INFO>().SELF_BUFF(1);
                    SENDER.GetComponent<CHAMP_INFO>().Damage_buff = buff;
                }

                SENDER.GetComponent<CHAMP_INFO>().focus += New_Focus;
                SENDER.GetComponent<CHAMP_INFO>().hp += New_Damage;
                Debug.Log(SENDER.GetComponent<CHAMP_INFO>().Name + " Buffed themselves, " + New_Focus.ToString() + " Focus was gained");

                WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL_PARTY)
            {
                for (int i = 0; i < ORDER.Length; i++)
                {
                    if (ORDER[i] != null)
                    {
                        ORDER[i].GetComponent<CHAMP_INFO>().SELF_BUFF(1);
                        ORDER[i].GetComponent<CHAMP_INFO>().Damage_buff = buff;

                        WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;
                    }
                }
            }

            if(Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL_ENEMY)
            {
                for (int i = 0; i < MONSTER_ORDER.Length; i++)
                {
                    if (MONSTER_ORDER[i] != null && MONSTER_ORDER[i] != SENDER)
                    {
                        SENDER.GetComponent<CHAMP_INFO>().NORMAL_HIT(MONSTER_ORDER[i], New_Damage);
                        WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

                        MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().hp -= New_Damage;
                        SENDER.GetComponent<CHAMP_INFO>().focus += focus;
                        MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().ON_HIT(); //MAKING SURE THE TARGET IS DEAD!!!
                    }
                }
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.HIT_ALL)
            {
                for (int i = 0; i < ORDER.Length; i++)
                {
                    if (ORDER[i] != null && ORDER[i] != SENDER)
                    {
                        SENDER.GetComponent<CHAMP_INFO>().NORMAL_HIT(ORDER[i], New_Damage);
                        WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

                        ORDER[i].GetComponent<CHAMP_INFO>().hp -= New_Damage;
                        SENDER.GetComponent<CHAMP_INFO>().focus += focus;
                        ORDER[i].GetComponent<CHAMP_INFO>().ON_HIT(); //MAKING SURE THE TARGET IS DEAD!!!
                    }
                }

                for (int i = 0; i < MONSTER_ORDER.Length; i++)
                {
                    if (MONSTER_ORDER[i] != null && MONSTER_ORDER[i] != SENDER)
                    {
                        SENDER.GetComponent<CHAMP_INFO>().NORMAL_HIT(MONSTER_ORDER[i], New_Damage);
                        WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

                        MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().hp -= New_Damage;
                        SENDER.GetComponent<CHAMP_INFO>().focus += focus;
                        MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().ON_HIT(); //MAKING SURE THE TARGET IS DEAD!!!
                    }
                }
            }

            if (Current_ATTACK.type == BASIC_ATTACKS.ATTACK_TYPE.SPAWN)
            {
                if (SENDER.GetComponent<CHAMP_INFO>().Team_player == true)
                {

                }

                if (SENDER.GetComponent<CHAMP_INFO>().Team_player == false)
                {
                    for (int i = 0; i < MONSTER_ORDER.Length; i++)
                    {
                        if (MONSTER_ORDER[i] == null || MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                        {
                            if (MONSTER_ORDER[i] != null && MONSTER_ORDER[i].GetComponent<CHAMP_INFO>().dead == true)
                            {
                                Destroy(MONSTER_ORDER[i]);
                            }

                            GameObject Monster = Instantiate(SPAWN, SPAWNS_ENEMY[i].transform.position, Quaternion.identity);
                            Monster.transform.position += new Vector3(0, Monster.GetComponent<CHAMP_INFO>().height_from_ground, 0);
                            Monster.transform.eulerAngles = new Vector3(0, 180, 0);
                            Monster.GetComponent<CHAMP_INFO>().Party_order = i;
                            Monster.GetComponent<CHAMP_INFO>().Team_player = false;

                            MONSTER_ORDER[i] = Monster;
                        }
                    }
                }
            }
        }

        if (attack_HITS == false)
        {
            SENDER.GetComponent<CHAMP_INFO>().MISS_ATTACK();
            WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

            Debug.Log(SENDER.GetComponent<CHAMP_INFO>().Name + " MISSED THEIR ATTACK," + " 0 DAMAGE WAS DEALT");
        }
    }


    private bool Check_if_attack_lands(int acc)
    {
        if (acc == 100) 
        { 
            return true;
        }

        if (acc == 0)
        {
            return false;
        }

        //THE INSANE RANDOM NUM GENERATOR XD
        int LUCKY_WHELL = Random.Range(0, 101);
        Debug.Log("RANDOM NUM IS " + LUCKY_WHELL.ToString());

        if (LUCKY_WHELL <= acc)
        {
            return true;
        }

        if (LUCKY_WHELL > acc)
        {
            return false;
        }

        Debug.Log("Check_if_attack_lands, might not be working right...");
        return false;
    }

    public void TARGET_CLICKED(GameObject Target)
    {
        TARGET_ENEMY = Target;

        if (CURRENT_STATE == STATEMACHINE.TARGET)
        {
            if (Current_ATTACK is SPECIALS)
            {
                SPECIALS currentSpecial = (SPECIALS)Current_ATTACK;
                ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().focus = ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().focus - currentSpecial.cost;
                CURRENT_STATE = STATEMACHINE.MICROGAME;
                StateMachine(STATEMACHINE.MICROGAME);
            }
            else
            {
                CURRENT_STATE = STATEMACHINE.BATTLE;
                StateMachine(STATEMACHINE.BATTLE);
            }
            
        }

        if (CURRENT_STATE == STATEMACHINE.ITEM_SELECT)
        {
            Current_ITEM.amount--; //REMOVE ONE ITEM WHEN USED
            Target.GetComponent<CHAMP_INFO>().Item_used(Current_ITEM);
            Cam_holder.transform.position = new Vector3(ORDER[ON_CURRENT_CHAMP].transform.position.x + x_value, ORDER[ON_CURRENT_CHAMP].transform.position.y - y_value, ORDER[ON_CURRENT_CHAMP].transform.position.z + z_value);

            WAIT_TIME = Target.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;
            CURRENT_STATE = STATEMACHINE.WAITING;


            Invoke("BACK", WAIT_TIME);
        }
    }

    //buttons
    public void ATTACK()
    {
        CURRENT_STATE = STATEMACHINE.SELECT_NORMAL;
        StateMachine(CURRENT_STATE);
        Item_enable(false);
    }

    public void SPECIAL()
    {
        CURRENT_STATE = STATEMACHINE.SELECT_SPECIAL;
        StateMachine(CURRENT_STATE);
        Item_enable(false);
        
    }

    public void ITEM()
    {
        Item_enabled = !Item_enabled;
        
        Item_enable(Item_enabled);
    }

    public void Item_enable(bool enable)
    {
        Item_enabled = enable;
        ITEM_PANEL.SetActive(Item_enabled);
    }

    public void Item_click(InventoryItem Item)
    {
        Debug.Log("The Button is working OMG no way This is so awesome, it is so cool. It is the most incredible button the world has ever seen");

        Current_ITEM = Item;

        CURRENT_STATE = STATEMACHINE.ITEM_SELECT;
        StateMachine(CURRENT_STATE);
    }

    public void EXPAND()
    {
        EXPANDED = !EXPANDED;

        if (EXPANDED == false)
        {
            Cam_holder.transform.position = new Vector3(ORDER[ON_CURRENT_CHAMP].transform.position.x + x_value, ORDER[ON_CURRENT_CHAMP].transform.position.y - y_value, ORDER[ON_CURRENT_CHAMP].transform.position.z + z_value);
        }

        if (EXPANDED == true)
        {
            Cam_holder.transform.position = Vector3.zero;
        }
    }
    public void RUN()
    {
        Item_enable(false);
    }

    public void MOVESELECT(GameObject button)
    {
       if (button.GetComponent<BUTTON_HOLDER>().ATTACK != null)
       {
            Current_ATTACK = button.GetComponent<BUTTON_HOLDER>().ATTACK;
       }

       if (button.GetComponent<BUTTON_HOLDER>().SPECIALS != null)
       {
            Current_ATTACK = button.GetComponent<BUTTON_HOLDER>().SPECIALS;
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
