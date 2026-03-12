using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string message = "Hej træner!";

    public void Interact(PlayerInteract player)
    {
        Debug.Log(message);
    }
}