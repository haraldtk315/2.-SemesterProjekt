using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DIALOGUEHANDLER : MonoBehaviour
{
    public static DIALOGUEHANDLER instance;

    public GameObject dialogueBoxPrefab;
    private GameObject dialogueBox;
    
    private TextMeshPro dialogueBoxText => dialogueBox.GetComponentInChildren<TextMeshPro>();

    public float textSpeed;

    private int currDialogueIndex;

    private string[] dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DialogueStart(string[] _dialogue)
    {
        dialogue = _dialogue;
        currDialogueIndex = 0;
        dialogueBox = Instantiate(dialogueBoxPrefab);
        dialogueBoxText.text = "";
        StartCoroutine(WriteDialogueToBox());
    }

    public void DialogueNextLine()
    {
        if (currDialogueIndex < dialogue.Length - 1)
        {
            currDialogueIndex++;
            dialogueBoxText.text = "";
            StartCoroutine(WriteDialogueToBox());
        }
        else
        {
            dialogue = Array.Empty<string>();
            Destroy(dialogueBox);
        }
    }

    IEnumerator WriteDialogueToBox()
    {
        foreach (char letter in dialogue[currDialogueIndex].ToCharArray())
        {
            dialogueBoxText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void OnAttack(InputValue input)
    {
        if (dialogueBoxText.text == dialogue[currDialogueIndex])
        {
            DialogueNextLine();
        }
        else
        {
            StopAllCoroutines();
            dialogueBoxText.text = dialogue[currDialogueIndex];
        }
    }
}
