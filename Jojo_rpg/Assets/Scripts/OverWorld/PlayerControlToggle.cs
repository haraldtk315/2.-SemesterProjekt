using UnityEngine;

public class PlayerControlToggle : MonoBehaviour
{
    private GridMovement2D movement;
    private PlayerInteract interact;

    private void Awake()
    {
        movement = GetComponent<GridMovement2D>();
        interact = GetComponent<PlayerInteract>();
    }

    public void DisableControls()
    {
        if (movement != null) movement.enabled = false;
        if (interact != null) interact.enabled = false;
    }

    public void EnableControls()
    {
        if (movement != null) movement.enabled = true;
        if (interact != null) interact.enabled = true;
    }
}