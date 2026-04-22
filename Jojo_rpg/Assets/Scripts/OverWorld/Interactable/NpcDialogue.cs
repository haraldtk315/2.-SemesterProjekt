using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public bool HAS_ICON;

    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;

    public GameObject[] enemies;

    public GameObject partyReward;

    public bool destroyAfterBattleDialogue = true;

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
        if (!string.IsNullOrEmpty(npcID) &&
            GAMEMANAGER.instance.pendingPostBattleNPCID == npcID)
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
                        partyReward
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

        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing);
        GAMEMANAGER.instance.currentNPCID = npcID;
        GAMEMANAGER.instance.pendingPartyReward = partyReward;

        DIALOGUEHANDLER.instance.DialogueStart(
            message,
            player.gameObject,
            enemies,
            null,
            null,
            HAS_ICON,
            this.gameObject
        );
    }
}