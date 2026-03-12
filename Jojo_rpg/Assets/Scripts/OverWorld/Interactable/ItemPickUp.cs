using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public int amount = 1;

    public void Interact(PlayerInteract player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddItem(itemData, amount);
            Destroy(gameObject);
        }
    }
}