using UnityEngine;

public class GAMEMANAGER : MonoBehaviour
{
    public static GAMEMANAGER instance;

    public GameObject[] party;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
