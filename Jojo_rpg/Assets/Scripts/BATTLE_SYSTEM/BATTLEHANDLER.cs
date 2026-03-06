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

    public RawImage Texture;
    public GameObject MAIN_Buttons;

    //PARTY INFORMATION
    private int party_size = 0;
    private GameObject SINGLE_PLAYER;
    private GameObject[] ORDER = {null, null, null, null, null};
    private GameObject[] MONSTER_ORDER = { null, null, null, null, null };

    private int ON_CURRENT_CHAMP = 0;
    

    //ENEMY INFORMATION
    [SerializeField] private int enemy_count = 0;
    public GameObject only_monster;

    public enum STATEMACHINE
    {
        INPUT,
        SELECT,
        TARGET,
        RHYTHM,
        BATTLE,
    }
    
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
        Invoke("SET_CAM_LOCATION", 2f);
    }

    private void SET_CAM_LOCATION()
    {
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
            monster.GetComponent<SpriteRenderer>().flipX = true;
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
            Cam_holder.transform.position = new Vector3(ORDER[ON_CURRENT_CHAMP].transform.position.x, -1.25f, 3);
        }

        //SELECT MOVE
        if (Current == STATEMACHINE.SELECT)
        {

        }

        //TARGET A MONSTER TO ATTACK
        if (Current == STATEMACHINE.TARGET)
        {
            MAIN_Buttons.SetActive(false);
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
            Debug.Log("IN BATTLE");
        }
    }

    public void ATTACK()
    {
        CURRENT_STATE = STATEMACHINE.TARGET;
        StateMachine(CURRENT_STATE);
    }

    public void SPECIAL()
    {

    }

    public void ITEM()
    {

    }

    public void RUN()
    {

    }
}
