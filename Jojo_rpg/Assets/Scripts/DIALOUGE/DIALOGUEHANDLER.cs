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
    
    private TextMeshProUGUI dialogueBoxText => dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

    public float textSpeed;

    private int currDialogueIndex;

    private string[] dialogue;

    private bool dialogueActive = false;

    InputAction nextAction;

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

        // Connect to the new Unity input system action needed for dialogue
        nextAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (nextAction.WasPressedThisFrame())
        {
            OnNextAction();
        }
    }

    public void DialogueStart(string[] _dialogue)
    {
        if (!dialogueActive)
        {
            dialogueActive = true;
            dialogue = _dialogue;
            currDialogueIndex = 0;
            dialogueBox = Instantiate(dialogueBoxPrefab);
            dialogueBox.transform.SetParent(FindAnyObjectByType<Canvas>().transform, false);
            dialogueBoxText.text = "";
            StartCoroutine(WriteDialogueToBox());
        }
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
            dialogueActive = false;
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

    public void OnNextAction()
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
