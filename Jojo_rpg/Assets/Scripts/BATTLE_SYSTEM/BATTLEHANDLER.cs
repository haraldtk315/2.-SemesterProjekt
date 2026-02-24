using UnityEngine;

public class BATTLEHANDLER : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject[] SPAWNS;
    [SerializeField] private GameObject[] SPAWNS_ENEMY;

    //GAMEMANAGER
    public GAMEMANAGER GM;
    public Camera Cam;

    //PARTY INFORMATION
    private int party_size = 0;
    private GameObject SINGLE_PLAYER;

    //ENEMY INFORMATION
    [SerializeField] private int enemy_count = 0;
    public GameObject only_monster;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        SpawnCharactors();
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

            if (GM.party[i] != this)
            {
                party_size++;
                SINGLE_PLAYER = GM.party[i];
            }
        }

        for (int i = 0; i < GM.party.Length; i++)
        {
            if (party_size == 1)
            {
                InstantiateChamp(4, true);
                break;
            }

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
            GameObject Ally = Instantiate(GM.party[position_spawn], SPAWNS[position_spawn].transform.position, Quaternion.identity);
            Ally.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            Ally.GetComponent<CHAMP_INFO>().Team_player = true;
        }


        //Enemy charactors = false
        if (!Player)
        {
            GameObject monster = Instantiate(only_monster, SPAWNS_ENEMY[position_spawn].transform.position, Quaternion.identity);
            monster.GetComponent<SpriteRenderer>().flipX = true;
            monster.GetComponent<CHAMP_INFO>().Party_order = position_spawn;
            monster.GetComponent<CHAMP_INFO>().Team_player = false;
        }
    }
}
