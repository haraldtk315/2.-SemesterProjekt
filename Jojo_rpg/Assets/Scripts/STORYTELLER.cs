using UnityEngine;

public class STORYTELLER : MonoBehaviour
{

    public string[] Narrator1;
    public string[] Narrator2;
    public string[] Narrator3;
    public string[] Narrator4;
    public int ON_THIS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DIALOGUEHANDLER.instance.DialogueStart(Narrator1, this.gameObject, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
