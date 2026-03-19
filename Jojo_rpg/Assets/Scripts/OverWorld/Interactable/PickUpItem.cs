using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public string pickupID;
    public ItemData itemData;
    public int amount = 1;

    public void Start()
    {
        if (GAMEMANAGER.instance != null && GAMEMANAGER.instance.collectedPickups.Contains(pickupID))
        {
            Destroy(gameObject);
        }
    }

    public void Interact(PlayerInteract player)
    {
        GAMEMANAGER.instance.AddItem(itemData, amount);
        GAMEMANAGER.instance.collectedPickups.Add(pickupID);
        Destroy(gameObject);
    }
}