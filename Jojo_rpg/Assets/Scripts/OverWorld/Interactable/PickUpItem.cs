using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public int amount = 1;

    public void Interact(PlayerInteract player)
    {
        if (itemData == null)
        {
            Debug.LogError("PickupItem has no ItemData assigned.");
            return;
        }

        GAMEMANAGER.instance.AddItem(itemData, amount);
        Destroy(gameObject);
    }
}