using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;
    public GameObject[] enemies;

    //partymechaninc
    public GameObject partyReward;
    public bool recruitAfterDialogue;
    public bool recruitAfterBattle;

    public bool destroyAfterBattle = true;

    private void Start()
    {
        // Hvis denne NPC lige har været i battle, så vis after-battle dialogue automatisk
        if (!string.IsNullOrEmpty(npcID) &&
            GAMEMANAGER.instance.pendingPostBattleNPCID == npcID)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                DIALOGUEHANDLER.instance.DialogueStart(
                    messageAfterBattle,
                    player,
                    null,
                    destroyAfterBattle ? gameObject : null
                );
            }

            GAMEMANAGER.instance.pendingPostBattleNPCID = null;
        }
    }

    public void Interact(PlayerInteract player)
    {
        bool defeated = !string.IsNullOrEmpty(npcID) &&
                        GAMEMANAGER.instance.defeatedNPCs.Contains(npcID);

        // Hvis allerede besejret, gør ingenting eller vis evt. ikke noget
        if (defeated)
        {
            return;
        }

        if (recruitAfterDialogue && partyReward != null)
        {
            GAMEMANAGER.instance.AddPartyMember(partyReward);

        }

        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing);
        GAMEMANAGER.instance.currentNPCID = npcID;

        DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject, enemies, null);
    }
}