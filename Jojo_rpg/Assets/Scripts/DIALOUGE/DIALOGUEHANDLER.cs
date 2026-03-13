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

    private TextMeshProUGUI dialogueBoxText;

    public float textSpeed;

    private int currDialogueIndex;
    private string[] dialogue;
    private bool dialogueActive = false;

    private InputAction nextAction;

    private PlayerControlToggle currentPlayerControls;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        nextAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (nextAction != null && nextAction.WasPressedThisFrame())
        {
            if (dialogueActive)
            {
                OnNextAction();
            }
        }
    }

    public void DialogueStart(string[] _dialogue, GameObject player)
    {
        if (!dialogueActive)
        {
            dialogueActive = true;
            dialogue = _dialogue;
            currDialogueIndex = 0;

            if (player != null)
            {
                currentPlayerControls = player.GetComponent<PlayerControlToggle>();

                if (currentPlayerControls != null)
                {
                    currentPlayerControls.DisableControls();
                }
            }

            dialogueBox = Instantiate(dialogueBoxPrefab);
            dialogueBoxText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

            Canvas canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null)
            {
                dialogueBox.transform.SetParent(canvas.transform, false);
            }

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
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialogue = Array.Empty<string>();

        if (dialogueBox != null)
        {
            Destroy(dialogueBox);
        }

        if (currentPlayerControls != null)
        {
            currentPlayerControls.EnableControls();
            currentPlayerControls = null;
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