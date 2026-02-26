using UnityEngine;

public class MUSICMANAGER : MonoBehaviour
{
    public static MUSICMANAGER instance;

    public AudioClip[] THEMES;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
