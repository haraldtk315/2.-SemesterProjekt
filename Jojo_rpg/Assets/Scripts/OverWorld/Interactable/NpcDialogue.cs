using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;
    public GameObject[] enemies;

    public bool destroyAfterBattle = true;
    public void Interact(PlayerInteract player)
    {
        bool defeated = !string.IsNullOrEmpty(npcID) &&
                        GAMEMANAGER.instance.defeatedNPCs.Contains(npcID);

        if (defeated)
        {
            DIALOGUEHANDLER.instance.DialogueStart(messageAfterBattle, player.gameObject,  null, destroyAfterBattle ? gameObject : null);
            return;
        }

        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing);
        GAMEMANAGER.instance.currentNPCID = npcID;

        DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject, enemies, null);
    }
}