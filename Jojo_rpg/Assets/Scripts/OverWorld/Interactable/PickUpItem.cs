using Unity.VisualScripting;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public string pickupID;
    public ItemData itemData;
    public int amount = 1;

    public GameObject playerObject;

    public void Start()
    {
        if (GAMEMANAGER.instance != null && GAMEMANAGER.instance.collectedPickups.Contains(pickupID))
        {
            Destroy(gameObject);
        }
    }

    public void Interact(PlayerInteract player)
    {
        string[] pickupText = new string[2];
        GAMEMANAGER.instance.AddItem(itemData, amount);
        GAMEMANAGER.instance.collectedPickups.Add(pickupID);
        pickupText[0] = $"Picked up {itemData.displayName}";
        pickupText[1] = $"{itemData.description}" + $" {itemData.value}";
        DIALOGUEHANDLER.instance.DialogueStart(pickupText, playerObject, null, null, null, null, false, this.gameObject);
        //Destroy(gameObject);
    }
}