using UnityEngine;

public class BATTLEHANDLER : MonoBehaviour
{
    [SerializeField] private Transform center;

    [SerializeField] private GameObject[] SPAWNS;

    [SerializeField] private GameObject[] SPAWNS_ENEMY;

    //GAMEMANAGER
    public GAMEMANAGER GM;

    //PARTY INFORMATION
    private int party_size = 0;
    private GameObject SINGLE_PLAYER;

    //ENEMY INFORMATION
    [SerializeField] private int enemy_count = 0;
    public GameObject only_monster;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GAMEMANAGER>();

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
                Instantiate(SINGLE_PLAYER, SPAWNS[4].transform.position, Quaternion.identity);
                break;
            }

            if (GM.party[i] == null)
            {
                continue;
            }

            Instantiate(GM.party[i], SPAWNS[i].transform.position, Quaternion.identity);
        }

        //ENEMY SPAWNS
        enemy_count = Random.Range(1, 6);

        for (int i = 0; i < enemy_count; i++)
        {
            if (enemy_count == 1)
            {
                Instantiate(only_monster, SPAWNS_ENEMY[4].transform.position, Quaternion.identity);
                break;
            }

            GameObject monster = Instantiate(only_monster, SPAWNS_ENEMY[i].transform.position, Quaternion.identity);
            monster.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
