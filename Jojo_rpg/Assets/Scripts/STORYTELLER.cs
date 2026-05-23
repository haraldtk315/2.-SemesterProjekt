using UnityEngine;

public class STORYTELLER : MonoBehaviour
{

    public string[] Narrator1;
    public string[] Narrator2;
    public string[] Narrator3;
    public string[] Narrator4;
    public int ON_THIS;

    public GameObject GOOD_TOWN;
    public GameObject Destroyed_town;

    public Animator ANI;

    public GameObject canvas_burn;

    private void Awake()
    {
        DIALOGUEHANDLER.instance.DialogueStart(Narrator1, this.gameObject, null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void STARTDIAL()
    {
        if (ON_THIS == 1)
        {
            DIALOGUEHANDLER.instance.DialogueStart(Narrator2, this.gameObject, null);
            GOOD_TOWN.SetActive(false);
            Destroyed_town.SetActive(true);
            canvas_burn.SetActive(true);
        }

        if (ON_THIS == 2)
        {
            DIALOGUEHANDLER.instance.DialogueStart(Narrator3, this.gameObject, null);
        }
    }
}
