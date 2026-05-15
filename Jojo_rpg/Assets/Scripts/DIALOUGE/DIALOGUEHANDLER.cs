using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DIALOGUEHANDLER : MonoBehaviour
{
    public static DIALOGUEHANDLER instance;

    public GameObject dialogueBoxPrefab;
    private GameObject dialogueBox;
    private TextMeshProUGUI dialogueBoxText;
    public GameObject ICON_CAM;

    private GameObject destroyAfterDialogueObject;
    private GameObject partyRewardAfterDialogue;
    private GameObject secondPartyRewardAfterDialogue;

    public float textSpeed;

    private int currDialogueIndex;
    private string[] dialogue;
    private bool dialogueActive = false;

    public GameObject[] ENEMIES;
    private GameObject TALK_OBJECT;

    private InputAction nextAction;
    private PlayerControlToggle currentPlayerControls;

    private GameObject ICON_CAM_PATH;
    public GameObject FADE;

    //EMMA ADDING EXTRA STUFF DON'T MIND ME <3
    private string Fade_text;

    private void Awake()
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
    }

    private void Start()
    {
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

    public void DialogueStart(string[] _dialogue, GameObject player, GameObject[] enemies = null, GameObject destroyAfterDialogue = null, GameObject partyReward = null, GameObject secondPartyreward = null, bool ICON = false, GameObject OBJECT = null, string Fade_out = null)
    {
        TALK_OBJECT = OBJECT;
        Fade_text = Fade_out;

        if (!dialogueActive)
        {
            dialogueActive = true;
            dialogue = _dialogue;
            currDialogueIndex = 0;
            ENEMIES = enemies;
            destroyAfterDialogueObject = destroyAfterDialogue;
            partyRewardAfterDialogue = partyReward;
            secondPartyRewardAfterDialogue = secondPartyreward;

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

            if (ICON == true)
            {
                Vector3 Pos = new Vector3(OBJECT.transform.position.x, OBJECT.transform.position.y + 0.1f, -100);
                GameObject ICON_CAM_OBJECT = Instantiate(ICON_CAM, Pos, Quaternion.identity);
                ICON_CAM_PATH = ICON_CAM_OBJECT;
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
            if (TALK_OBJECT != false && TALK_OBJECT.TryGetComponent<NPC>(out NPC npc))
            {
                if (npc.TRANSFER_TO_THIS == string.Empty)
                {
                    currentPlayerControls = null;
                }
            }

            if (TALK_OBJECT != false && TALK_OBJECT.TryGetComponent<PartyObstacle>(out PartyObstacle party))
            {
                currentPlayerControls = null;
            }
            currentPlayerControls.EnableControls();
        }

        bool shouldStartFight = false;

        if (ENEMIES != null)
        {
            for (int i = 0; i < ENEMIES.Length; i++)
            {
                if (ENEMIES[i] != null)
                {
                    shouldStartFight = true;
                    break;
                }
            }
        }
        if (shouldStartFight)
        {
            destroyAfterDialogueObject = null;
            partyRewardAfterDialogue = null;
            
            if (FADE != null)
            {
                Debug.Log("FADE");
                GameObject dark = Instantiate(FADE, Vector3.zero, Quaternion.identity) as GameObject;

                if (GameObject.FindGameObjectWithTag("Canvas") == true)
                {
                    dark.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                    dark.GetComponent<FADE>().TEXT.text = Fade_text;
                }
            }

            Invoke("LoadFight", 1f);
            return;
        }

        //transfer scene
        if (TALK_OBJECT != null)
        {
            if (TALK_OBJECT.TryGetComponent<NPC>(out NPC npc))
            {
                if (npc.TRANSFER_TO_THIS != string.Empty)
                {
                    if (FADE != null)
                    {
                        Debug.Log("FADE");
                        GameObject dark = Instantiate(FADE, Vector3.zero, Quaternion.identity) as GameObject;
                        dark.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                        dark.GetComponent<FADE>().TEXT.text = Fade_text;
                    }
                }

                Invoke("loading", 3f);
            }
        }

        if (partyRewardAfterDialogue != null)
        {
            GAMEMANAGER.instance.AddPartyMember(partyRewardAfterDialogue);
            partyRewardAfterDialogue = null;
        }

        if (secondPartyRewardAfterDialogue != null)
        {
            GAMEMANAGER.instance.AddPartyMember(secondPartyRewardAfterDialogue);
            secondPartyRewardAfterDialogue = null;
        }

        if (destroyAfterDialogueObject != null)
        {
            PartyObstacle obstacle = destroyAfterDialogueObject.GetComponent<PartyObstacle>();

            if (obstacle != null)
            {
                obstacle.ClearObstacle();
            }
            else
            {
                NPC npc = destroyAfterDialogueObject.GetComponent<NPC>();

                if (npc != null && !string.IsNullOrEmpty(npc.npcID))
                {
                    GAMEMANAGER.instance.removedNPCs.Add(npc.npcID);
                }

                Destroy(destroyAfterDialogueObject);
            }

            destroyAfterDialogueObject = null;
        }

        if (ICON_CAM_PATH != null)
        {
            Destroy(ICON_CAM_PATH);
        }

        ENEMIES = null;
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

    public void loading()
    {
        if (TALK_OBJECT.TryGetComponent<NPC>(out NPC npc))
        {
            if (npc.New_player_cords != Vector3.zero)
            {
                GAMEMANAGER.instance.returnPlayerPosition = TALK_OBJECT.GetComponent<NPC>().New_player_cords;
            }

            SceneManager.LoadScene(TALK_OBJECT.GetComponent<NPC>().TRANSFER_TO_THIS);
        }
    }

    public void LoadFight()
    {
        SceneManager.LoadScene("FIGHT");
    }
}