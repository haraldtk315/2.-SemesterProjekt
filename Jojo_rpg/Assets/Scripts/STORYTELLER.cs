using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Animator CHILD_ANI;

    public GameObject canvas_burn;
    public GameObject FLAMES;

    public GameObject FADE;
    public string Fade_text;

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
            FLAMES.SetActive(true);
        }

        if (ON_THIS == 2)
        {
            DIALOGUEHANDLER.instance.DialogueStart(Narrator3, this.gameObject, null);
            ANI.Play("RUN");
        }

        if (ON_THIS == 3)
        {
            DIALOGUEHANDLER.instance.DialogueStart(Narrator4, this.gameObject, null);
            CHILD_ANI.Play("RUN");
        }

        if (ON_THIS == 4)
        {
            GameObject dark = Instantiate(FADE, Vector3.zero, Quaternion.identity) as GameObject;

            if (GameObject.FindGameObjectWithTag("Canvas") == true)
            {
                dark.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                dark.GetComponent<FADE>().TEXT.text = Fade_text;
            }

            Invoke("LoadNextScene", 3f);
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("INTRO");
    }
}
