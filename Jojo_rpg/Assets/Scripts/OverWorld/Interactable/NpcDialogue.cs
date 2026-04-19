using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;
    public GameObject[] enemies;

    public bool destroyAfterBattle = true;

    private void Start()
    {
        if (!string.IsNullOrEmpty(npcID) &&
            GAMEMANAGER.instance.pendingPostBattleNPCID == npcID)
        {
            
            if (recruitAfterBattle && partyReward != null)
            {
                GAMEMANAGER.instance.AddPartyMember(partyReward);
            }

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

        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing);
        GAMEMANAGER.instance.currentNPCID = npcID;
        DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject, enemies, null);
        Destroy(gameObject);
    }
}