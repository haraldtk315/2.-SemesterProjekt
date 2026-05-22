using System.Collections;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    public bool HAS_ICON;

    public string TRANSFER_TO_THIS;
    public Vector3 New_player_cords;

    public GameObject FADE;
    public string DisplayText_on_fade;

    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;

    public GameObject[] enemies;

    public GameObject partyReward;
    public GameObject secondPartyReward;

    public bool destroyAfterBattleDialogue = true;
    public bool joinPartyWithoutBattle = false;

    private void Start()
    {
        // Hvis NPC allerede er fjernet permanent, så fjern den med det samme
        if (!string.IsNullOrEmpty(npcID) &&
            GAMEMANAGER.instance.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

        // Hvis denne NPC lige har været i battle, så vis after-battle dialogue
        if (!string.IsNullOrEmpty(npcID) && GAMEMANAGER.instance.pendingPostBattleNPCID == npcID)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // Hvis der faktisk er after-battle message
                if (messageAfterBattle != null && messageAfterBattle.Length > 0)
                {
                    DIALOGUEHANDLER.instance.DialogueStart(
                        messageAfterBattle,
                        player,
                        null,
                        destroyAfterBattleDialogue ? gameObject : null,
                        partyReward,
                        secondPartyReward,
                        HAS_ICON,
                        gameObject,
                        null
                    );
                }
                else
                {
                    // Ingen after-battle message:
                    // giv reward hvis der er en
                    if (partyReward != null)
                    {
                        GAMEMANAGER.instance.AddPartyMember(partyReward);
                    }

                    // fjern NPC hvis den skal væk
                    if (destroyAfterBattleDialogue)
                    {
                        GAMEMANAGER.instance.removedNPCs.Add(npcID);
                        Destroy(gameObject);

                    }
                }

                if (TRANSFER_TO_THIS != string.Empty)
                {
                    Debug.Log("INVOKE_LOADING");
                    SceneManager.LoadScene(TRANSFER_TO_THIS);
                }
            }

            GAMEMANAGER.instance.pendingPostBattleNPCID = null;
        }
    }

    public void Interact(PlayerInteract player)
    {
        bool defeated = !string.IsNullOrEmpty(npcID) &&
                        GAMEMANAGER.instance.defeatedNPCs.Contains(npcID);

        if (defeated)
        {
            return;
        }

        // Hvis NPC skal joine party uden kamp
        if (joinPartyWithoutBattle)
        {
            DIALOGUEHANDLER.instance.DialogueStart(
                message,
                player.gameObject,
                null, // ingen enemies = ingen kamp
                destroyAfterBattleDialogue ? gameObject : null,
                partyReward,
                secondPartyReward,
                HAS_ICON,
                gameObject
            );

            return;
        }

        GameObject jack = GameObject.FindGameObjectWithTag("Jack");
        GameObject werner = GameObject.FindGameObjectWithTag("Werner");
        // Normal NPC med kamp
        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing, jack.transform, werner.transform);
        GAMEMANAGER.instance.currentNPCID = npcID;
        GAMEMANAGER.instance.pendingPartyReward = partyReward;
        GAMEMANAGER.instance.pendingSecondPartyReward = secondPartyReward;

        DIALOGUEHANDLER.instance.DialogueStart(
            message,
            player.gameObject,
            enemies,
            null,
            null,
            null,
            HAS_ICON,
            gameObject,
            DisplayText_on_fade
        );
    }
}