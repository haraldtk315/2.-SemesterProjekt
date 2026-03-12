using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string[] message;

    public void Interact(PlayerInteract player)
    {
        DIALOGUEHANDLER.instance.DialogueStart(message);
    }
}