using NUnit.Framework.Internal;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BATTLEHANDLER : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject[] SPAWNS;
    [SerializeField] private GameObject[] SPAWNS_ENEMY;

    //GAMEMANAGER
    public GAMEMANAGER GM;

    //DIALOGUEHANDLER
    public DIALOGUEHANDLER DH;

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
    public BASIC_ATTACKS[] ENEMY_MOVES;
    public int Enemy_Attack;
    public int ON_TARGET_ENEMY = 0;

    public int CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE; //Major problems with animations on zombies since it is done insinde a for loop so we need a seperate state for the animation/attacks of monsters
    private bool forced_combat = false;

    //STATEMACHINE

    float WAIT_TIME; //Used if we need small breaks between the states.
    float Extra_time = 1; //Used since it is quite akward to end an animation immidiate

    public enum STATEMACHINE
    {
        INPUT,
        SELECT_NORMAL,
        SELECT_SPECIAL,
        TARGET,
        RHYTHM,
        BATTLE,
        NEXT,
        ENEMY,
        ENEMY_BATTLE, //THE ENEMY STATE ALONE WAS NOT ENOUGH TO MAKE SURE THE ENEMIES COULD HAVE THEIR OWN ANIMATIONS AND STUFF LIKE THAT SO WE NEED A LITTLE EXTRA STEP FOR ANIMATIONS
        END
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

        //-Jeg kunne rigtig godt tænke mig at vi mpåske ikke brugte tags men referede til gameobjects istedet :D :D - harald

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

                //Kinda messed up here but it should work ;3
                ORDER[0] = Ally;
                HEALTH_UI_FOR_PLAYERS[0].SetActive(true);
                Ally.GetComponent<CHAMP_INFO>().Player_UI = HEALTH_UI_FOR_PLAYERS[0];
                Ally.GetComponent<CHAMP_INFO>().Team_player = true;
            }
            else
            {
                GameObject Ally = Instantiate(GM.party[position_spawn], SPAWNS[position_spawn].transform.position, Quaternion.identity);
                Ally.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
                Ally.GetComponent<CHAMP_INFO>().Team_player = true;

                //player health and focus UI
                HEALTH_UI_FOR_PLAYERS[position_spawn].SetActive(true);
                Ally.GetComponent<CHAMP_INFO>().Player_UI = HEALTH_UI_FOR_PLAYERS[position_spawn];

                ORDER[position_spawn] = Ally;
            }
        }

        //Enemy charactors = false
        if (!Player && forced_combat == false)
        {
            GameObject monster = Instantiate(only_monster, SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
            monster.transform.eulerAngles = new Vector3(0, 180, 0);
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;

            MONSTER_ORDER[position_spawn] = monster;
        }

        //Forced_combat moment
        if (!Player && forced_combat == true)
        {
            GameObject monster = Instantiate(DH.ENEMIES[position_spawn], SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
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
            ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(true);
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
                MOVES_BUTTON[i].GetComponentInChildren<TextMeshProUGUI>().text += "  |  " + "ACC: " + ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().ATTACKS[i].acc + "%";
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
            ORDER[ON_CURRENT_CHAMP].GetComponent<CHAMP_INFO>().TARGETINDICATOR.SetActive(false);

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
            TARGET_ATTACK(ORDER[ON_CURRENT_CHAMP], TARGET_ENEMY, Current_ATTACK.damage, Current_ATTACK.acc);

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
                            CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE = j;

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
            TARGET_ATTACK(MONSTER_ORDER[ON_TARGET_ENEMY], ORDER[CURRENTLY_ATTACKING_THISGUY_FROM_ENEMY_STATE], ENEMY_MOVES[Enemy_Attack].damage, ENEMY_MOVES[Enemy_Attack].acc);

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

            if (lost == true)
            {
                SceneManager.LoadScene("TITLE");
            }

            if (win == true)
            {
                    Debug.Log("Current trainer before save: " + GAMEMANAGER.instance.currentNPCID);

                    if (!string.IsNullOrEmpty(GAMEMANAGER.instance.currentNPCID))
                    {
                        GAMEMANAGER.instance.defeatedNPCs.Add(GAMEMANAGER.instance.currentNPCID);

                        Debug.Log("Trainer added to defeatedNPCs: " + GAMEMANAGER.instance.currentNPCID);
                        Debug.Log("Total defeated NPCs: " + GAMEMANAGER.instance.defeatedNPCs.Count);

                        GAMEMANAGER.instance.currentNPCID = null;
                    }

                    SceneManager.LoadScene(GAMEMANAGER.instance.returnSceneName);
                
            }

            if (win == false && lost == false)
            {
                Debug.Log("GOING BACK TO PLAYER INPUT");
                CancelInvoke(); //I think it is in the ENEMY state where something happends so that we have a function call that gets called twice leading to end state being called twice. This is a workaround solution.
                Invoke("STATEGOTOINPUT", 2f);
            }
        }
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

    //THE BASIC ATTACK (Could potentially also work for the future special move)
    public void TARGET_ATTACK(GameObject SENDER, GameObject TARGET, int Damage, int acc)
    {
        //ZOOM OUT CAM
        Cam_holder.transform.position = Vector3.zero;

        //WILL ATTACK HIT?
        bool attack_HITS = Check_if_attack_lands(acc);

        //ANIMATION
        if(attack_HITS == true)
        {
            SENDER.GetComponent<CHAMP_INFO>().NORMAL_HIT(TARGET, Damage);
            WAIT_TIME = SENDER.GetComponent<CHAMP_INFO>().GET_CURRENT_ANIMATION_LENGTH() + Extra_time;

            //DO DAMAGE TO TARGET (Could potentially be moved into the NORMAL_HIT() METHOD)
            TARGET.GetComponent<CHAMP_INFO>().hp -= Damage;
            TARGET.GetComponent<CHAMP_INFO>().ON_HIT(); //MAKING SURE THE TARGET IS DEAD!!!
            Debug.Log(TARGET.GetComponent<CHAMP_INFO>().Name + " GOT ATTACKED BY " + SENDER.GetComponent<CHAMP_INFO>().Name + " " + Damage.ToString() + " DAMAGE DEALT");
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
