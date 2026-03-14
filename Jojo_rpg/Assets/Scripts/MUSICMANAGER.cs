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
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    //hvorfor laver du singleton på denne mpåde? -harald

    // Update is called once per frame
    void Update()
    {
        
    }
}
