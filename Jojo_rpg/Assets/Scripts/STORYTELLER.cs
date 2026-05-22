using UnityEngine;

public class STORYTELLER : MonoBehaviour
{

    public string[] Narrator1;
    public string[] Narrator2;
    public string[] Narrator3;
    public string[] Narrator4;
    public int ON_THIS;

    private void Awake()
    {
        DIALOGUEHANDLER.instance.DialogueStart(Narrator1, this.gameObject, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
