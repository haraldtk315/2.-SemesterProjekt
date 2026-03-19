using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcID;
    public string[] message;
    public string[] messageAfterBattle;

    public GameObject[] enemies;


    public void Interact(PlayerInteract player)
    {
        Debug.Log("=== NPC INTERACT ===");
        Debug.Log("NPC ID: " + npcID);
        Debug.Log("Defeated? " + GAMEMANAGER.instance.defeatedNPCs.Contains(npcID));

        if (!string.IsNullOrEmpty(npcID) && GAMEMANAGER.instance.defeatedNPCs.Contains(npcID))
        {
            if (messageAfterBattle == null && messageAfterBattle.Length > 0)
            {
                Debug.Log("using after battle message");
                DIALOGUEHANDLER.instance.ENEMIES = new GameObject[0];
                DIALOGUEHANDLER.instance.DialogueStart(messageAfterBattle, player.gameObject);
                return;
            }
            //else
            //{
            //    DIALOGUEHANDLER.instance.ENEMIES = new GameObject[0];
            //    DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject);
            //}
            return;
        }

        Debug.Log("using normal message and starting battle");

        //gemmer hvor spilleren stod før battle
        GAMEMANAGER.instance.SaveOverworldReturnPoint(player.transform, player.facing);

        //hvilken NPC som startede battle
        GAMEMANAGER.instance.currentNPCID = npcID;

        //sender enemies til dialoguehandler så den kan starte battle efter dialog
        DIALOGUEHANDLER.instance.ENEMIES = enemies;
        DIALOGUEHANDLER.instance.DialogueStart(message, player.gameObject);
    }

}